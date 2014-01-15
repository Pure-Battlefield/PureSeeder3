using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;
using PureSeeder.Core.Monitoring;
using PureSeeder.Core.Settings;
using PureSeeder.Forms.Extensions;
using Timer = System.Windows.Forms.Timer;

namespace PureSeeder.Forms
{
    public partial class MainForm : Form
    {
        private readonly IDataContext _context;
        private readonly Timer _refreshTimer;
        private readonly ProcessMonitor _processMonitor;
        private readonly IdleKickAvoider _idleKickAvoider;

        // CancellationTokens
        private CancellationToken _processMonitorCt;
        private CancellationToken _avoidIdleKickCt;

        private MainForm()
        {
            InitializeComponent();
        }

        public MainForm(IDataContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;

            _context.Session.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);
            _context.Settings.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);

            _refreshTimer = new Timer();

            // Todo: This should be injected
            _processMonitor = new ProcessMonitor(new CrashDetector(new CrashHandler()));
            _processMonitor.OnProcessStateChanged += HandleProcessStatusChange;
            _idleKickAvoider = new IdleKickAvoider();
        }

        #region Initialization
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CreateBindings();
            UiSetup();

            LoadBattlelog();

            geckoWebBrowser1.DocumentCompleted += BrowserChanged;
            //_context.OnContextUpdate += ContextUpdated;

            SetRefreshTimer();

            // Spin up background processes
            SpinUpProcessMonitor();
            SpinUpAvoidIdleKick();
        }

        private void UiSetup()
        {
            this.Icon = PureSeeder.Forms.Properties.Resources.PB;
            notifyIcon1.Icon = Properties.Resources.PBOff;
            notifyIcon1.Text = Constants.ApplicationName;
            this.Text = Constants.ApplicationName;
        }

        private async void SpinUpProcessMonitor()
        {
            _processMonitorCt = new CancellationTokenSource().Token;
            await _processMonitor.CheckOnProcess(_processMonitorCt, () => _context.Session.CurrentGame);
        }

        private async void SpinUpAvoidIdleKick()
        {
            _avoidIdleKickCt = new CancellationTokenSource().Token;
            await _idleKickAvoider.AvoidIdleKick(_avoidIdleKickCt, _context.Settings.IdleKickAvoidanceTimer, () => _context.Session.CurrentGame);
        }

        private void SetRefreshTimer()
        {
            int refreshTimerInterval;
            if (!int.TryParse(refreshInterval.Text, out refreshTimerInterval))
                refreshTimerInterval = _context.Settings.RefreshInterval;

            refreshTimerInterval = refreshTimerInterval*1000;

            _refreshTimer.Interval = refreshTimerInterval;

            _refreshTimer.Tick += TimedRefresh;

            _refreshTimer.Start();
        }

        private void CreateBindings()
        {
            var serversBindingSource = new BindingSource { DataSource = _context.Settings.Servers, };

            serverSelector.DataSource = serversBindingSource;
            serverSelector.DisplayMember = "Name";
            serverSelector.DataBindings.Add("SelectedIndex", _context.Settings, x => x.CurrentServer/*, false, DataSourceUpdateMode.OnPropertyChanged*/);

            SeedingMinPlayers.DataBindings.Add("Text", serversBindingSource, "MinPlayers", true, DataSourceUpdateMode.OnPropertyChanged);
            SeedingMaxPlayers.DataBindings.Add("Text", serversBindingSource, "MaxPlayers", true, DataSourceUpdateMode.OnPropertyChanged);

            username.DataBindings.Add("Text", _context.Settings, x => x.Username);
            password.DataBindings.Add("Text", _context.Settings, x => x.Password);
            email.DataBindings.Add("Text", _context.Settings, x => x.Email);

            curPlayers.DataBindings.Add("Text", _context.Session, x => x.CurrentPlayers);
            maxPlayers.DataBindings.Add("Text", _context.Session, x => x.ServerMaxPlayers);
            currentLoggedInUser.DataBindings.Add("Text", _context.Session, x => x.CurrentLoggedInUser);

            logging.DataBindings.Add("Checked", _context.Settings, x => x.EnableLogging, false, DataSourceUpdateMode.OnPropertyChanged);
            minimizeToTray.DataBindings.Add("Checked", _context.Settings, x => x.MinimizeToTray, false, DataSourceUpdateMode.OnPropertyChanged);
            autoLogin.DataBindings.Add("Checked", _context.Settings, x => x.AutoLogin, false, DataSourceUpdateMode.OnPropertyChanged);
            seedingEnabled.DataBindings.Add("Checked", _context.Session, x => x.SeedingEnabled);
            refreshInterval.DataBindings.Add("Text", _context.Settings, x => x.RefreshInterval);

            saveSettings.DataBindings.Add("Enabled", _context.Settings, x => x.DirtySettings, true, DataSourceUpdateMode.OnPropertyChanged);
        }


        #endregion Intialization

        #region EventHandlers

        void HandleProcessStatusChange(object sender, ProcessStateChangeEventArgs e)
        {
            _context.Session.BfIsRunning = e.IsRunning;

            if (_context.Session.BfIsRunning)
                notifyIcon1.Icon = Properties.Resources.PBOn;
            if (!_context.Session.BfIsRunning)
                notifyIcon1.Icon = Properties.Resources.PBOff;
        }

        private void TimedRefresh(object sender, EventArgs e)
        {
           AnyRefresh();
        }

        private void AnyRefresh()
        {
            DisableRefreshButton();
            RefreshPageAndData();
        }

        private void RefreshPageAndData()
        {
            _refreshTimer.Stop();

            // Create a single use event handler to fire AttemptSeeding after context is updated
            // This is so that only refreshes triggerd by PS will fire Seeding. This will prevent changes
            // made inside the browser (by redirect/javascript/etc.) from firing the Seeding.
            ContextUpdatedHandler handler = null;
            handler = (tSender, tE) =>
            {
                _context.OnContextUpdate -= handler;
                AttemptKick();
                AttemptSeeding();
            };
            _context.OnContextUpdate += handler;
            LoadPage();

            _refreshTimer.Start();
        }

        void BrowserChanged(object sender, EventArgs e)
        {
            UpdateContext();
        }

        // Deprecated
//        private void ContextUpdated(object sender, EventArgs e)
//        {
//            AttemptKick();
//            AttemptSeeding();
//        }

        #endregion EventHandlers

        #region BattlelogManipulation

        private void JoinServer()
        {
            // Todo: This should be moved into configuration so it can more easily be changed
            const string jsCommand = "document.getElementsByClassName('btn btn-primary btn-large large arrow')[0].click()";

            // Deprecated
//            using (var context = new AutoJSContext(geckoWebBrowser1.Window.JSContext))
//            {
//                context.EvaluateScript(jsCommand);
//            }

            RunJavascript(jsCommand)/*.RunSynchronously()*/;

            _context.JoinServer();
        }

        private void LoadBattlelog()
        {
            LoadPage();
        }

        private string GetAddress(ComboBox cb)
        {
            if (cb.Items.Count == 0)
                return Constants.DefaultUrl;

            var address = _context.CurrentServer.Address; //((Server) cb.SelectedItem).Address;

            return address;
        }

        private async void LoadPage()
        {
            var selectedUrl = GetAddress(serverSelector);
            if (selectedUrl == String.Empty)
                selectedUrl = Constants.DefaultUrl;

            //geckoWebBrowser1.Navigate(selectedUrl);
            await Navigate(selectedUrl);
        }

        private Task Navigate(string url)
        {
            //this.BeginInvoke(new Action(() => geckoWebBrowser1.Navigate(url)));
            return Task.Factory.StartNew(() => this.BeginInvoke(new Action(() => geckoWebBrowser1.Navigate(url))));
        }

        void ContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateInterface();
        }

        

        private void UpdateContext()
        {
            var source = string.Empty;

            string pageSource = string.Empty;

            if (!string.IsNullOrEmpty(geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml))
                pageSource = geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml;

            source = pageSource;

            _context.UpdateStatus(source);
        }

        private void AttemptSeeding()
        {
            if (!ShouldSeed())
                return;

            var result = MessageBoxEx.Show("Seeding in 5 seconds.", "Auto-Seeding", MessageBoxButtons.OKCancel,
                                           MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 5000);

            if (result == DialogResult.Cancel)
                return;

            JoinServer();
        }

        private void UpdateInterface()
        {
            currentLoggedInUser.ForeColor = _context.GetUserStatus() == UserStatus.Correct ? Color.Green : Color.Red;
        }

        private void AutoLogin(/*Action successfulLogin = null, Action failedLogin = null*/)
        {
            if (!_context.Settings.AutoLogin)
                return;

            if (_context.GetUserStatus() == UserStatus.Correct)
                return;
                /*if(successfulLogin != null)
                    successfulLogin.Invoke();*/

            if (_context.GetUserStatus() == UserStatus.Incorrect)
            {
                // Do not autologout
                //AutoLogout(/*() => AutoLogin(successfulLogin, failedLogin), null*/);
                return;
            }
            
            Login(/*successfulLogin, failedLogin*/);
        }

        private async void Login(/*Action successfulLogin = null, Action failedLogin = null*/)
        {
            SetStatus("Attempting login.");

            // Todo: This should be moved into configuration so it can more easily be changed
            string jsCommand = String.Format("$('#base-login-email').val('{0}');$('#base-login-password').val('{1}');$('#baseloginpersist').val() == '1';$(\"[name='submit']\").click();", email.Text, password.Text);

            RunJavascript(jsCommand);

            // Check for a while if the login worked
            //await CheckLogin(successfulLogin, failedLogin);

            await Sleep(1);
            SetStatus("");

        }

        private void RunJavascript(string javascript)
        {
            using (var context = new AutoJSContext(geckoWebBrowser1.Window.JSContext))
            {
                context.EvaluateScript(javascript);
            }
        }

//        private Task RunJavascript(string javascript)
//        {
//            return Task.Factory.StartNew(() =>
//                {
//                    using (var context = new AutoJSContext(geckoWebBrowser1.Window.JSContext))
//                    {
//                        context.EvaluateScript(javascript);
//                    }
//                });
//        }

//        private Task CheckLogin(Action successfulLogin = null, Action failedLogin = null)
//        {
//            return Task.Factory.StartNew(() =>
//                {
//                    const int loginCheckCount = 10;
//                    for (var i = 0; i < loginCheckCount; i++)
//                    {
//                        if (_context.Session.CurrentLoggedInUser != Constants.NotLoggedInUsername)
//                        {
//                            if (successfulLogin != null)
//                                successfulLogin.Invoke();
//                            return;
//                        }
//
//                        Thread.Sleep(1000); // Sleep for 1 second
//                    }
//                    if (failedLogin != null)
//                        failedLogin.Invoke();
//                });
//        }

        private /*async*/ void Logout(/*Action successfulLogout, Action failedLogout*/)
        {
            geckoWebBrowser1.Navigate("http://battlelog.battlefield.com/bf4/session/logout/");

            /*await CheckLogout(successfulLogout, failedLogout);*/
        }

        private Task CheckLogout(Action successfulLogout, Action failedLogout)
        {
            return Task.Factory.StartNew(() =>
                {
                    const int logoutCheckCount = 10;
                    for (var i = 0; i < logoutCheckCount; i++)
                    {
                        if (_context.GetUserStatus() == UserStatus.None)
                        {
                            if (successfulLogout != null)
                                successfulLogout.Invoke();

                            return;
                        }

                        Thread.Sleep(1000);
                    }
                    if (failedLogout != null)
                        failedLogout.Invoke();
                });
        }

        private void AutoLogout(/*Action logoutSuccess, Action logoutFail*/)
        {
            if (_context.GetUserStatus() == UserStatus.None)
            {
                /*if (logoutSuccess != null)
                    logoutSuccess.Invoke();*/

                return;
            }

            Logout(/*logoutSuccess, logoutFail*/);
        }

        private bool ShouldSeed()
        {
            var shouldSeed = _context.ShouldSeed();

            if (!shouldSeed.Result)
            {
                if (shouldSeed.Reason == ShouldNotSeedReason.NotLoggedIn)
                {
                    SetStatus("Cannot seed. Not logged in.", 5);
                    AutoLogin(/*RefreshPageAndData*/);
                    return false;
                }

                if (shouldSeed.Reason == ShouldNotSeedReason.SeedingDisabled)
                {
                    SetStatus("Seeding disabled.", 5);
                    return false;
                }

                if (shouldSeed.Reason == ShouldNotSeedReason.IncorrectUser)
                {
                    SetStatus("Cannot seed. Incorrect logged in user.", 5);
                    //AutoLogout(/*() => AutoLogin(RefreshPageAndData), null*/);
                    return false;
                }

                if (shouldSeed.Reason == ShouldNotSeedReason.GameAlreadyRunning)
                    return false;

                if (shouldSeed.Reason == ShouldNotSeedReason.NotInRange)
                {
                    SetStatus("Player count above min threshold, not starting seeding.");
                    return false;
                }

                if (shouldSeed.Reason == ShouldNotSeedReason.NoServerDefined)
                {
                    SetStatus("No server defined. Cannot seed.");
                    return false;
                }

                throw new NotImplementedException("Need to handle all reasons for seeding not starting.");
            }

            return true;
        }

        private /*Task*/ void AttemptKick()
        {
            // Deprecated
            /*return Task.Factory.StartNew(() =>
            {*/
                var shouldKick = _context.ShouldKick();
                if (shouldKick.Result)
                {
                    if (shouldKick.Reason == KickReason.AboveSeedingRange)
                    {
                        var result =
                            MessageBoxEx.Show(
                                "Player count above max threshold. If game is running it will be stopped in 5 seconds.",
                                "Max Player Threshold Exceeded", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 5000);

                        if (result == DialogResult.OK)
                            _context.StopGame();
                        
                        return;
                    }

                    if (shouldKick.Reason == KickReason.NoServerDefined)
                    {
                        return;
                    }

                    throw new NotImplementedException("Need to handle all reasons for kicking.");
                }
            /*});*/
        }

        #endregion BattlelogManipulation

        #region UiEvents

        private void serverSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //LoadPage();
            AnyRefresh();
        }

        private void saveSettings_Click(object sender, EventArgs e)
        {
            _context.Settings.SaveSettings();
        }

        private void joinServerButton_Click(object sender, EventArgs e)
        {
            //JoinServer();
            AttemptSeeding();
        }

        private void geckoWebBrowser1_DomContentChanged(object sender, DomEventArgs e)
        {
            //UpdateContext();
            BrowserChanged(sender, e);
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            AnyRefresh();
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void minimizeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState && _context.Settings.MinimizeToTray)
            {
                notifyIcon1.ShowBalloonTip(500, "Pure Seeder 3 Still Running", "Right click to restore the window", ToolTipIcon.Info);
                this.Hide();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettingsDialog.ShowDialog();
        }

        private void saveSettingsDialog_FileOk(object sender, CancelEventArgs e)
        {
            var fileName = saveSettingsDialog.FileName;
            _context.ExportSettings(fileName);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSettingsDialog.ShowDialog();
        }

        private void importSettingsDialog_FileOk(object sender, CancelEventArgs e)
        {
            var fileName = importSettingsDialog.FileName;
            _context.ImportSettings(fileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        #endregion UiEvents

        #region UiManipulation

        private async void DisableRefreshButton()
        {
            refresh.Enabled = false;
            await Sleep(5);
            refresh.Enabled = true;
        }


        private async void SetStatus(string status, int time = -1)
        {
            toolStripStatusLabel1.Text = status;
            statusStrip1.Refresh();

            if (time > -1)
            {
                await Sleep(time);
                SetStatus("");
            }
        }

        #endregion UiManipulation


        private Task Sleep(int seconds)
        {
            return Task.Factory.StartNew(() => Thread.Sleep(seconds * 1000));
        }
    }
}

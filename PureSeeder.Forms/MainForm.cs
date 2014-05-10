using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;
using PureSeeder.Core.Monitoring;
using PureSeeder.Core.ServerManagement;
using PureSeeder.Core.Settings;
using PureSeeder.Forms.Extensions;
using PureSeeder.Forms.Properties;
using Timer = System.Windows.Forms.Timer;

namespace PureSeeder.Forms
{
    public partial class MainForm : Form
    {
        private readonly IDataContext _context;
        private readonly IdleKickAvoider _idleKickAvoider;
        private readonly ProcessMonitor _processMonitor;
        private readonly Timer _refreshTimer;
        private readonly Timer _statusRefreshTimer;

        // CancellationTokens
        private CancellationToken _avoidIdleKickCt;
        private CancellationToken _processMonitorCt;

        private MainForm()
        {
            InitializeComponent();
        }

        public MainForm(IDataContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;

            _context.Session.PropertyChanged += ContextPropertyChanged;
            _context.Settings.PropertyChanged += ContextPropertyChanged;

            _refreshTimer = new Timer();
            _statusRefreshTimer = new Timer();

            // Todo: This should be injected
            _processMonitor =
                new ProcessMonitor(new ICrashDetector[]
                {new CrashDetector(new CrashHandler()), new DetectCrashByFaultWindow()});
            _processMonitor.OnProcessStateChanged += HandleProcessStatusChange;
            _idleKickAvoider = new IdleKickAvoider();
        }

        #region Initialization

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CreateBindings();
            UiSetup();
            
            RefreshServerStatuses(null, null);
            LoadBattlelog();

            geckoWebBrowser1.DocumentCompleted += BrowserChanged;

            FirstRunCheck();

            SetRefreshTimer();
            SetStatusRefreshTimer();

            // Spin up background processes
            SpinUpProcessMonitor();
            SpinUpAvoidIdleKick();
        }

        // Deprecated
//        private async void UpdateServerStatuses()
//        {
//            await _context.UpdateServerStatuses();
//
//            var blah = "meh";  // Todo: Handle the updated server statuses
//        }

        private void UiSetup()
        {
            Icon = Resources.PB;
            notifyIcon1.Icon = Resources.PBOff;
            notifyIcon1.Text = Constants.ApplicationName;
            Text = Constants.ApplicationName;
            closeToolStripMenuItem.Text = String.Format("Close {0}", Constants.ApplicationName);
        }

        private async void SpinUpProcessMonitor()
        {
            _processMonitorCt = new CancellationTokenSource().Token;
            await _processMonitor.CheckOnProcess(_processMonitorCt, () => _context.Session.CurrentGame);
        }

        private async void SpinUpAvoidIdleKick()
        {
            _avoidIdleKickCt = new CancellationTokenSource().Token;
            await
                _idleKickAvoider.AvoidIdleKick(_avoidIdleKickCt, _context.Settings.IdleKickAvoidanceTimer,
                    () => _context.Session.CurrentGame);
        }

        private void FirstRunCheck()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
                {
                    ShowReleaseNotes();
                }
            }
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

        private void SetStatusRefreshTimer()
        {
            int statusRefreshTimerInterval;
            if (!int.TryParse(statusRefreshInterval.Text, out statusRefreshTimerInterval))
                statusRefreshTimerInterval = _context.Settings.StatusRefreshInterval;

            statusRefreshTimerInterval = statusRefreshTimerInterval*1000;

            _statusRefreshTimer.Interval = statusRefreshTimerInterval;

            _statusRefreshTimer.Tick += RefreshServerStatuses;

            _statusRefreshTimer.Start();
        }

        private void CreateBindings()
        {
            // Controlling this manually
            //serverSelector.DataBindings.Add("SelectedIndex", _context.Settings, x => x.CurrentServer, false, DataSourceUpdateMode.OnPropertyChanged);

            username.DataBindings.Add("Text", _context.Settings, x => x.Username);
            password.DataBindings.Add("Text", _context.Settings, x => x.Password);
            email.DataBindings.Add("Text", _context.Settings, x => x.Email);

            currentLoggedInUser.DataBindings.Add("Text", _context.Session, x => x.CurrentLoggedInUser);

            logging.DataBindings.Add("Checked", _context.Settings, x => x.EnableLogging, false,
                DataSourceUpdateMode.OnPropertyChanged);
            minimizeToTray.DataBindings.Add("Checked", _context.Settings, x => x.MinimizeToTray, false,
                DataSourceUpdateMode.OnPropertyChanged);
            autoLogin.DataBindings.Add("Checked", _context.Settings, x => x.AutoLogin, false,
                DataSourceUpdateMode.OnPropertyChanged);
            seedingEnabled.DataBindings.Add("Checked", _context.Session, x => x.SeedingEnabled);
            refreshInterval.DataBindings.Add("Text", _context.Settings, x => x.RefreshInterval);
            statusRefreshInterval.DataBindings.Add("Text", _context.Settings, x => x.StatusRefreshInterval);
            autoMinimizeSeeder.DataBindings.Add("Checked", _context.Settings, x => x.AutoMinimizeSeeder, false,
                DataSourceUpdateMode.OnPropertyChanged);
            autoMinimizeGame.DataBindings.Add("Checked", _context.Settings, x => x.AutoMinimizeGame, false,
                DataSourceUpdateMode.OnPropertyChanged);

            saveSettings.DataBindings.Add("Enabled", _context.Settings, x => x.DirtySettings, true,
                DataSourceUpdateMode.OnPropertyChanged);

            var statusBindingSource = new BindingSource() {DataSource = _context.Session.ServerStatuses};

            dataGridView1.DataSource = statusBindingSource;
        }

        #endregion Intialization

        #region EventHandlers

        private void HandleProcessStatusChange(object sender, ProcessStateChangeEventArgs e)
        {
            _context.Session.BfIsRunning = e.IsRunning;

            if (_context.Session.BfIsRunning)
                notifyIcon1.Icon = Resources.PBOn;
            if (!_context.Session.BfIsRunning)
                notifyIcon1.Icon = Resources.PBOff;
        }

        private void TimedRefresh(object sender, EventArgs e)
        {
            AnyRefresh();
        }

        private async void RefreshServerStatuses(object sender, EventArgs e)
        {
            await _context.UpdateServerStatuses();
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

        /// <summary>
        /// This method is the event handler for any time the browser is refreshed
        /// </summary>
        private void BrowserChanged(object sender, EventArgs e)
        {
            UpdateContext();
        }

        #endregion EventHandlers

        #region BattlelogManipulation

        private void JoinServer()
        {
            // Todo: This should be moved into configuration so it can more easily be changed
            const string jsCommand =
                "document.getElementsByClassName('btn btn-primary btn-large large arrow')[0].click()";

            RunJavascript(jsCommand);

            AutoMinimizeSeeder();

            _context.JoinServer();
        }

        private void LoadBattlelog()
        {
            LoadPage();
        }

        // Deprecated
//        private string GetAddress(ComboBox cb)
//        {
//            if (cb.Items.Count == 0)
//                return Constants.DefaultUrl;
//
//            //string address = _context.CurrentServer.Address; // Deprecated
//
//            //return address;
//
//            return string.Empty;
//        }

        private async void LoadPage()
        {
            const string selectedUrl = Constants.DefaultUrl;
            SetStatus(String.Format("Loading: {0}", selectedUrl), 3);

            await Navigate(selectedUrl);
        }

        private Task Navigate(string url)
        {
            return Task.Factory.StartNew(() => BeginInvoke(new Action(() => geckoWebBrowser1.Navigate(url))));
        }

        private void ContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateInterface();
        }

        /// <summary>
        /// Update the context with the source from the page currently loaded in the browser
        /// </summary>
        private void UpdateContext()
        {
            var source = string.Empty;
            var pageSource = string.Empty;

            // Get the source for the page currently loaded in the browser
            if (!string.IsNullOrEmpty(geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml))
                pageSource = geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml;

            source = pageSource;
            _context.UpdateContext(source); 
        }

        private void AttemptSeeding()
        {
            if (!ShouldSeed())
                return;

            DialogResult result = MessageBoxEx.Show("Seeding in 5 seconds.", "Auto-Seeding", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 5000);

            if (result == DialogResult.Cancel)
                return;

            JoinServer();
        }

        private void UpdateInterface()
        {
            currentLoggedInUser.ForeColor = _context.GetUserStatus() == UserStatus.Correct ? Color.Green : Color.Red;
        }

        private void AutoLogin()
        {
            if (!_context.Settings.AutoLogin)
                return;

            if (_context.GetUserStatus() == UserStatus.Correct)
                return;


            if (_context.GetUserStatus() == UserStatus.Incorrect)
            {
                return;
            }

            Login();
        }

        private async void Login()
        {
            SetStatus("Attempting login.");

            // Todo: This should be moved into configuration so it can more easily be changed
            string jsCommand =
                String.Format(
                    "$('#base-login-email').val('{0}');$('#base-login-password').val('{1}');$('#baseloginpersist').val() == '1';$(\"[name='submit']\").click();",
                    email.Text, password.Text);

            RunJavascript(jsCommand);

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

        private void Logout()
        {
            geckoWebBrowser1.Navigate("http://battlelog.battlefield.com/bf4/session/logout/");
        }

        private Task CheckLogout(Action successfulLogout, Action failedLogout)
        {
            return Task.Factory.StartNew(() =>
            {
                const int logoutCheckCount = 10;
                for (int i = 0; i < logoutCheckCount; i++)
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

        private bool ShouldSeed()
        {
            ResultReason<ShouldNotSeedReason> shouldSeed = _context.ShouldSeed();

            if (!shouldSeed.Result)
            {
                if (shouldSeed.Reason == ShouldNotSeedReason.NotLoggedIn)
                {
                    SetStatus("Cannot seed. Not logged in.", 5);
                    AutoLogin();
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

        private void AttemptKick()
        {
            ResultReason<KickReason> shouldKick = _context.ShouldKick();
            if (shouldKick.Result)
            {
                if (shouldKick.Reason == KickReason.AboveSeedingRange)
                {
                    DialogResult result =
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

                if (shouldKick.Reason == KickReason.GameNotRunning)
                    return;

                throw new NotImplementedException("Need to handle all reasons for kicking.");
            }
        }

        #endregion BattlelogManipulation

        #region UiEvents

        private void serverSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
//            _context.Settings.CurrentServer = serverSelector.SelectedIndex; Deprecated
            AnyRefresh();
        }

        private void saveSettings_Click(object sender, EventArgs e)
        {
            _context.Settings.SaveSettings();
        }

        private void joinServerButton_Click(object sender, EventArgs e)
        {
            AttemptSeeding();
        }

        private void geckoWebBrowser1_DomContentChanged(object sender, DomEventArgs e)
        {
            BrowserChanged(sender, e);
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            AnyRefresh();
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void minimizeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState && _context.Settings.MinimizeToTray)
            {
                Hide();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettingsDialog.ShowDialog();
        }

        private void saveSettingsDialog_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = saveSettingsDialog.FileName;
            _context.ExportSettings(fileName);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSettingsDialog.ShowDialog();
        }

        private void importSettingsDialog_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = importSettingsDialog.FileName;
            _context.ImportSettings(fileName);
            AnyRefresh();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            else
            {
                Hide();
                WindowState = FormWindowState.Minimized;
            }
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == false)
            {
                notifyIcon1.ShowBalloonTip(500, "Pure Seeder 3 Still Running",
                    "Right click or double click to restore the window", ToolTipIcon.Info);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void editServers_Click(object sender, EventArgs e)
        {
            _refreshTimer.Stop(); // Stop the refresh timer

            var serverEditor = new ServerEditor(_context);
            var dlgResult = serverEditor.ShowDialog();

            // This should probably be injected
            var serverUpdater = new UpdateServerIds();
            await serverUpdater.Update(_context, true);

            if (dlgResult == DialogResult.OK)
                _context.Settings.SaveSettings();

            _refreshTimer.Start(); // Start the refresh timer back up
        }

        private void viewReleaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
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

        private async void AutoMinimizeSeeder()
        {
            if (!_context.Settings.AutoMinimizeSeeder)
                return;

            var cts = new CancellationTokenSource();
            cts.CancelAfter(300*1000); // Cancel the background task after 5 minutes

            CancellationToken minimizerCt = cts.Token;
            await
                new RunAction().RunActionOnGameLoad(minimizerCt, () => _context.Session.CurrentGame,
                    () => { WindowState = FormWindowState.Minimized; });
        }

        private static void ShowReleaseNotes()
        {
            new ReleaseNotes().ShowDialog();
        }

        #endregion UiManipulation

        private Task Sleep(int seconds)
        {
            return Task.Factory.StartNew(() => Thread.Sleep(seconds*1000));
        }

        private void statusRefresh_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var playerStatus = _context.GetPlayerStatus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ps = _context.GetPlayerStatus();
        }

        

    }
}
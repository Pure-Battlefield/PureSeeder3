using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private readonly Timer _gameHangProtectionTimer;
        private readonly ProcessMonitor _processMonitor;

        // CancellationTokens
        private CancellationToken _processMonitorCt;

        public MainForm(IDataContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
            //((IWebView) webControl1).ParentWindow = this.Handle;

            _context.Session.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);
            _context.Settings.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);
            _context.OnHangProtectionInvoke += HandleHangProtectionInvoked;

            _refreshTimer = new Timer();
            _gameHangProtectionTimer = new Timer();

            // Todo: This should be injected
            _processMonitor = new ProcessMonitor();
            _processMonitor.OnProcessStateChanged += HandleProcessStatusChange;
        }

        void HandleProcessStatusChange(object sender, ProcessStateChangeEventArgs e)
        {
            _context.Session.BfIsRunning = e.IsRunning;

            if (_context.Session.BfIsRunning)
                notifyIcon1.Icon = Properties.Resources.PBOn;
            if (!_context.Session.BfIsRunning)
                notifyIcon1.Icon = Properties.Resources.PBOff;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Icon = PureSeeder.Forms.Properties.Resources.PB;
            notifyIcon1.Icon = Properties.Resources.PBOff;
            notifyIcon1.Text = Constants.ApplicationName;
            this.Text = Constants.ApplicationName;

            CreateBindings();
            LoadBattlelog();

            //geckoWebBrowser1.Navigated += BrowserChanged;
            geckoWebBrowser1.DocumentCompleted += BrowserChanged;

            SetRefreshTimer();

            SpinUpProcessMonitor();
        }

        private async void SpinUpProcessMonitor()
        {
            //var ct = CancellationTokenSource.CreateLinkedTokenSource();
            _processMonitorCt = new CancellationTokenSource().Token;
            await Task.Run(() => _processMonitor.CheckOnProcess(_processMonitorCt));
        }

        private void SetRefreshTimer()
        {
            int refreshTimerInterval;
            if (!int.TryParse(refreshInterval.Text, out refreshTimerInterval))
                refreshTimerInterval = _context.Settings.RefreshInterval;

            refreshTimerInterval = refreshTimerInterval*1000;

            _refreshTimer.Interval = refreshTimerInterval;

            _refreshTimer.Tick += HandleRefresh;

            _refreshTimer.Start();
        }

        private void HandleRefresh(object sender, EventArgs e)
        {
           RefreshPageAndData();
        }

        private void RefreshPageAndData()
        {
            _refreshTimer.Stop();

            // Create a single use event handler to fire AttemptSeeding after context is updated
            ContextUpdatedHandler handler = null;
            handler = (tSender, tE) =>
            {
                _context.OnContextUpdate -= handler;
                AttemptSeeding();
            };
            _context.OnContextUpdate += handler;
            LoadPage();

            _refreshTimer.Start();
        }

        private void HandleHangProtectionInvoked(object sender, EventArgs e)
        {
            MessageBoxEx.Show(
                "In order to protect from hangs. Battlefield will be shut down. It will be restarted if seeding is still necessary.",
                "Hang Protection", MessageBoxButtons.OK, 5000);
        }

        private MainForm()
        {
            InitializeComponent();
        }

        private void CreateBindings()
        {
            var serversBindingSource = new BindingSource {DataSource = _context.Settings.Servers};

            serverSelector.DataSource = serversBindingSource;
            serverSelector.DisplayMember = "Name";
            serverSelector.DataBindings.Add("SelectedIndex", _context.Settings, x => x.CurrentServer);

            SeedingMinPlayers.DataBindings.Add("Text", serversBindingSource, "MinPlayers", true, DataSourceUpdateMode.OnPropertyChanged);
            SeedingMaxPlayers.DataBindings.Add("Text", serversBindingSource, "MaxPlayers", true, DataSourceUpdateMode.OnPropertyChanged);
            
            username.DataBindings.Add("Text", _context.Settings, x => x.Username);

            curPlayers.DataBindings.Add("Text", _context.Session, x => x.CurrentPlayers);
            maxPlayers.DataBindings.Add("Text", _context.Session, x => x.ServerMaxPlayers);
            currentLoggedInUser.DataBindings.Add("Text", _context.Session, x => x.CurrentLoggedInUser);

            gameHangDetection.DataBindings.Add("Checked", _context.Settings, x => x.EnableGameHangProtection);
            logging.DataBindings.Add("Checked", _context.Settings, x => x.EnableLogging, false, DataSourceUpdateMode.OnPropertyChanged);
            minimizeToTray.DataBindings.Add("Checked", _context.Settings, x => x.MinimizeToTray, false,
                                            DataSourceUpdateMode.OnPropertyChanged);
            seedingEnabled.DataBindings.Add("Checked", _context.Session, x => x.SeedingEnabled);
            refreshInterval.DataBindings.Add("Text", _context.Settings, x => x.RefreshInterval);

            saveSettings.DataBindings.Add("Enabled", _context.Settings, x => x.DirtySettings, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void JoinServer()
        {
            if (!CheckUsernames())
                return;

            // Todo: This should be moved into configuration so it can more easily be changed
            const string jsCommand = "document.getElementsByClassName('btn btn-primary btn-large large arrow')[0].click()";

            using (var context = new AutoJSContext(geckoWebBrowser1.Window.JSContext))
            {
                context.EvaluateScript(jsCommand);
            }

            _context.JoinServer();
        }

        private void LoadBattlelog()
        {
            LoadPage();
        }

        private string GetAddress(ComboBox cb)
        {
            var address = ((Server) cb.SelectedItem).Address;

            return address;
        }

        private void LoadPage()
        {
            //geckoWebBrowser1.Navigate(address);
            geckoWebBrowser1.Navigate(GetAddress(serverSelector));
        }

        void ContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateInterface();
        }

        void BrowserChanged(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(1000); // Testcode
            UpdateContext();
        }

        private void UpdateContext()
        {
            //var source = webControl1.ExecuteJavascriptWithResult("document.documentElement.outerHTML").ToString();

            var source = string.Empty;

            string pageSource = string.Empty;

            if (!string.IsNullOrEmpty(geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml))
                pageSource = geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml;

            source = pageSource;

            _context.UpdateStatus(source);
            refresh.Enabled = true; // Todo: This is a hacky way to avoid refresh hammering. Think of something better.
        }

        private void AttemptSeeding()
        {
            if (!CheckUsernames())
                return;

            if (!_context.ShouldSeed)
                return;

            var result = MessageBoxEx.Show("Auto-seeding in 5 seconds.", "Auto-Seeding", MessageBoxButtons.OKCancel,
                                           MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 5000);

            if (result == DialogResult.Cancel)
                return;

            JoinServer();
        }

        private void UpdateInterface()
        {
            currentLoggedInUser.ForeColor = _context.IsCorrectUser ? Color.Green : Color.Red;
        }

        private void serverSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadPage();
        }

        private void saveSettings_Click(object sender, EventArgs e)
        {
            _context.Settings.SaveSettings();
        }

        private void joinServerButton_Click(object sender, EventArgs e)
        {
            JoinServer();
        }

        private bool CheckUsernames()
        {
            if (!_context.IsCorrectUser)
            {
                var result = MessageBoxEx.Show("You don't appear to be logged in to the correct account. Are you sure you'd like to try to connect?", "Incorrect User",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2, 5000);

                if (result == DialogResult.Yes)
                    return true;

                return false;
            }
            return true;
        }

        private void geckoWebBrowser1_DomContentChanged(object sender, DomEventArgs e)
        {
            UpdateContext();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            refresh.Enabled = false;  // Todo: This is a hacky way to avoid refresh hammering. Think of something better.
            RefreshPageAndData();
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Awesomium.Core;
using Awesomium.Windows.Forms;
//using PureSeeder.Core.Configuration;
using Gecko;
using PureSeeder.Core.Context;
using PureSeeder.Core.Settings;
using PureSeeder.Forms.Extensions;

namespace PureSeeder.Forms
{
    public partial class Form1 : Form
    {
        private readonly IDataContext _context;

        public Form1(IDataContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
            //((IWebView) webControl1).ParentWindow = this.Handle;

            _context.Session.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);
            _context.Settings.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //((IWebView)webControl1).ParentWindow = this.Handle;
            CreateBindings();
            //((IWebView) webControl1).ParentWindow = browserPanel.Handle;
            //serverSelector.SelectedIndex = GetCurrentServer();
            
            LoadBattlelog();

            //webControl1.DocumentReady += BrowserChanged;

            //geckoWebBrowser1.Navigate("battlelog.battlefield.com/bf4/");

            //geckoWebBrowser1.Navigated += BrowserChanged;
            geckoWebBrowser1.DocumentCompleted += BrowserChanged;

            
        }

        // Deprecated
//        private int GetCurrentServer()
//        {
//            var serverName = _context.Settings.CurrentServer.Name;
//
//            var index = Array.FindIndex<Server>(_context.Servers.ToArray(), x => x.Name == serverName);
//            return index;
//        }

        private Form1()
        {
            InitializeComponent();
        }

        private void CreateBindings()
        {
            var serversBindingSource = new BindingSource();
            serversBindingSource.DataSource = _context.Settings.Servers;

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
            seedingEnabled.DataBindings.Add("Checked", _context.Session, x => x.SeedingEnabled);

            saveSettings.DataBindings.Add("Enabled", _context.Settings, x => x.DirtySettings, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void JoinServer()
        {
            // Todo: This should be moved into configuration so it can more easily be changed
            const string jsCommand = "document.getElementsByClassName('btn btn-primary btn-large large arrow')[0].click()";

            using (var context = new AutoJSContext(geckoWebBrowser1.Window.JSContext))
            {
                context.EvaluateScript(jsCommand);
            }
        }

        private void LoadBattlelog()
        {
            LoadPage(GetAddress(serverSelector));
        }

        private string GetAddress(ComboBox cb)
        {
            var address = ((Server) cb.SelectedItem).Address;

            return address;
        }

        private void LoadPage(string address)
        {
            geckoWebBrowser1.Navigate(address);
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
        }

        private void UpdateInterface()
        {
            currentLoggedInUser.ForeColor = _context.IsCorrectUser ? Color.Green : Color.Red;
        }

        private static void OnShowNewView(object sender, ShowCreatedWebViewEventArgs e)
        {
            
        }

        private void serverSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadPage(GetAddress((ComboBox)sender));

            // Deprecated
            //_context.Settings.CurrentServer = ((ComboBox) sender).SelectedIndex;
            //_context.CurrentServer = ((ComboBox) sender).SelectedIndex;
            //_context.CurrentServer = (Server)((ComboBox) sender).SelectedValue;
        }

        private void saveSettings_Click(object sender, EventArgs e)
        {
            _context.Settings.SaveSettings();
        }

        private void joinServerButton_Click(object sender, EventArgs e)
        {
            if (!CheckUsernames())
                return;
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
    }
}

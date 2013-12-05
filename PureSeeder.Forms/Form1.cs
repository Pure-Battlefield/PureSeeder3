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
using PureSeeder.Core.Context;
using PureSeeder.Core.Settings;

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

            _context.PropertyChanged += new PropertyChangedEventHandler(ContextPropertyChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //((IWebView)webControl1).ParentWindow = this.Handle;
            CreateBindings();
            //((IWebView) webControl1).ParentWindow = browserPanel.Handle;
            serverSelector.SelectedIndex = GetCurrentServer();
            
            LoadBattlelog();

            //webControl1.DocumentReady += BrowserChanged;

            //geckoWebBrowser1.Navigate("battlelog.battlefield.com/bf4/");

            //geckoWebBrowser1.Navigated += BrowserChanged;
            geckoWebBrowser1.DocumentCompleted += BrowserChanged;

            
        }

        private int GetCurrentServer()
        {
            var serverName = _context.CurrentServer.Name;

            var index = Array.FindIndex<Server>(_context.Servers.ToArray(), x => x.Name == serverName);
            return index;
        }

        private Form1()
        {
            InitializeComponent();
        }

        private void CreateBindings()
        {
            var serversBindingSource = new BindingSource();
            serversBindingSource.DataSource = _context.Servers;

            //serverSelector.DataSource = _context.Servers;
            serverSelector.DataSource = serversBindingSource;
            serverSelector.DisplayMember = "Name";

            //serverSelector.DataBindings.Add("SelectedValue", _context, "CurrentServer");
            //serverSelector.DataBindings.Add("SelectedIndex", _context.Servers, "CurrentServerIndex");

            // Todo: Re-add this
            //serverSelector.DataBindings.Add("SelectedItem", _context, "CurrentServer");

//            SeedingMinPlayers.DataBindings.Add("Text", _context.CurrentServer, "MinPlayers");
//            SeedingMaxPlayers.DataBindings.Add("Text", _context.CurrentServer, "MaxPlayers");
            SeedingMinPlayers.DataBindings.Add("Text", serversBindingSource, "MinPlayers", true, DataSourceUpdateMode.OnPropertyChanged);
            SeedingMaxPlayers.DataBindings.Add("Text", serversBindingSource, "MaxPlayers", true, DataSourceUpdateMode.OnPropertyChanged);
            
            // Todo: Create an extension for adding Databindings that accepts an expression tree like
            //       BindableBase.SetProperty<T1, T2>() so that this no longer relies on magic strings
            username.DataBindings.Add("Text", _context, "Username");

            curPlayers.DataBindings.Add("Text", _context, "CurrentPlayers" );
            maxPlayers.DataBindings.Add("Text", _context, "ServerMaxPlayers");
            currentLoggedInUser.DataBindings.Add("Text", _context, "CurrentLoggedInUser");

            gameHangDetection.DataBindings.Add("Checked", _context, "HangProtectionStatus");
            logging.DataBindings.Add("Checked", _context, "LoggingEnabled");
            seedingEnabled.DataBindings.Add("Checked", _context, "SeedingEnabled");

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
            //_context.CurrentServer = ((ComboBox) sender).SelectedIndex;
            _context.CurrentServer = (Server)((ComboBox) sender).SelectedValue;
        }

        
        
    }
}

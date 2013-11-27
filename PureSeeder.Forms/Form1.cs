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
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;

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
            LoadBattlelog();

            //webControl1.DocumentReady += BrowserChanged;

            //geckoWebBrowser1.Navigate("battlelog.battlefield.com/bf4/");

            //geckoWebBrowser1.Navigated += BrowserChanged;
            geckoWebBrowser1.DocumentCompleted += BrowserChanged;
        }

        private Form1()
        {
            InitializeComponent();
        }

        private void CreateBindings()
        {
            serverSelector.DataSource = _context.Servers;
            serverSelector.DisplayMember = "Name";

            curPlayers.DataBindings.Add("Text", _context, "CurrentPlayers" );
            maxPlayers.DataBindings.Add("Text", _context, "ServerMaxPlayers");
        }

        private void LoadBattlelog()
        {
            LoadPage(GetAddress(serverSelector));
        }

        private Uri GetAddressOld(ComboBox cb)
        {
            var address = ((Server) cb.SelectedItem).Address;
            return new Uri(address);
        }

        private string GetAddress(ComboBox cb)
        {
            var address = ((Server) cb.SelectedItem).Address;

            return address;
        }

        private void LoadPage(string address)
        {
            //webControl1.Source = address;
            geckoWebBrowser1.Navigate(address);
            //UpdateContext();
        }

        static void ContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
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

            // Update the context
            // Note: This is super ugly.
            //  - Awesomium doesn't have a reliable way of determining when a page is completely finished loading
            //  - Battlelog finshes loading the DOM, then JS is processed which alters it, Awesomium fires event on DOM Load
            //  - This just retries and induces an artificial delay to try to let it finish loading
            //  - Hopefully futrue versions of Awesomium will make this cleaner
//            for (var i = 0; i < 10; i++)
//            {
//                //_context.UpdateStatus(source);
//
//                if (_context.CurrentPlayers != null)
//                {
//                    return;
//                }
//                
//                // Delay 50 ms and try again
//                System.Threading.Thread.Sleep(50);
//                UpdateContext();
//            }
        }

        private static void OnShowNewView(object sender, ShowCreatedWebViewEventArgs e)
        {
            
        }

        private void serverSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadPage(GetAddress((ComboBox)sender));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var source = webControl1.ExecuteJavascriptWithResult("document.documentElement.outerHTML").ToString();
            geckoWebBrowser1.Navigate("about:plugins");
        }
    }
}

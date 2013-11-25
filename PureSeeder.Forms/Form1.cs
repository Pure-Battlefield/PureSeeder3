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
            ((IWebView) webControl1).ParentWindow = this.Handle;

            CreateBindings();
            LoadBattlelog();

            
        }

        private Form1()
        {
            InitializeComponent();
        }

        private void CreateBindings()
        {
            serverSelector.DataSource = _context.Servers;
            serverSelector.DisplayMember = "Name";
        }

        private void LoadBattlelog()
        {
            LoadPage(GetAddress(serverSelector));
        }

        private void serverSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPage(GetAddress((ComboBox)sender));
        }

        private Uri GetAddress(ComboBox cb)
        {
            var address = ((Server) cb.SelectedItem).Address;
            return new Uri(address);
        }

        private void LoadPage(Uri address)
        {
            webControl1.Source = address;
            UpdateContext();
        }

        private void UpdateContext()
        {
            var source = webControl1.ExecuteJavascriptWithResult("document.documentElement.outerHTML").ToString();

            // Update the context
        }

        private static void OnShowNewView(object sender, ShowCreatedWebViewEventArgs e)
        {
            
        }
    }
}

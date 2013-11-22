using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            webControl1.Source = GetAddress(serverSelector);
        }

        private void serverSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            webControl1.Source = GetAddress((ComboBox)sender);
        }

        private Uri GetAddress(ComboBox cb)
        {
            var address = ((Server) cb.SelectedItem).Address;
            return new Uri(address);
        }
    }
}

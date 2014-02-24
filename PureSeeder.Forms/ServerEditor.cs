using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Context;
using PureSeeder.Core.Settings;
using PureSeeder.Forms.Extensions;
using PureSeeder.Forms.Properties;

namespace PureSeeder.Forms
{
    public partial class ServerEditor : Form
    {
        private readonly IDataContext _context;

        private ServerEditor()
        {
            InitializeComponent();
        }

        public ServerEditor([NotNull] IDataContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        private void ServerEditor_Load(object sender, EventArgs e)
        {
            var serversBindingSource = new BindingSource {DataSource = _context.Settings.Servers};
            serverList.DataSource = serversBindingSource;
            serverList.DisplayMember = "Name";

            Icon = Resources.PB;

            LoadProperties();
        }

        private void LoadProperties()
        {
            if (serverList.SelectedIndex < 0)
                return;

            var server = _context.Settings.Servers[serverList.SelectedIndex];

            propertyGrid1.SelectedObject = server;
        }

        private void serverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProperties();
        }

        private void closeDialog_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addServer_Click(object sender, EventArgs e)
        {
            var newServer = new Server() {Name = "New Server"};
            _context.Settings.Servers.Add(newServer);
        }

        private void deleteServer_Click(object sender, EventArgs e)
        {
            _context.Settings.Servers.RemoveAt(serverList.SelectedIndex);
        }
    }
}

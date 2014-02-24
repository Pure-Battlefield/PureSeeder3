using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PureSeeder.Forms
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            versionLabel.Text = "";
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                versionLabel.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
        }

        private void viewReleaseNotes_Click(object sender, EventArgs e)
        {
            new ReleaseNotes().ShowDialog();
        }
    }
}

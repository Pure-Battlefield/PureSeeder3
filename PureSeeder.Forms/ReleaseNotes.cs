using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureSeeder.Forms.Properties;

namespace PureSeeder.Forms
{
    public partial class ReleaseNotes : Form
    {
        public ReleaseNotes()
        {
            InitializeComponent();
        }

        private void ReleaseNotes_Load(object sender, EventArgs e)
        {
            Icon = Resources.PB;
            webBrowser1.Url = new Uri("http://bradrhodes.github.io/PureSeeder3/");
        }
    }
}

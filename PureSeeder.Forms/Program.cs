using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Awesomium.Core;
using PureSeeder.Forms.Initalization;

namespace PureSeeder.Forms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var xulLocation = @"C:\Dev\Random\pure\new\XulRunner\xulrunner-22.0.en-US.win32\xulrunner";
            var xulLocation = @"C:\Users\Brad\Documents\Dev\PureBattlefield\new\pureseeder\lib\xulrunner";
            // Setup Gecko
            Gecko.Xpcom.Initialize(xulLocation);
            Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;

            var container = new FormsContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var webConfig = new WebConfig
                {
                    HomeURL = new Uri("http://battlelog.battlefield.com"),
                    LogLevel = LogLevel.Verbose
                };
            
            WebCore.Initialize(webConfig);

            Application.Run(container.Resolve<Form1>());
        }
    }
}

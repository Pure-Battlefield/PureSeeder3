using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureSeeder.Core.Context;
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
            // Ensure only 1 instance of the seeder can run at a time
            bool singleInstanceResult;
            var mutex = new System.Threading.Mutex(true, "PureSeeder3App", out singleInstanceResult);
            if (!singleInstanceResult)
                return;

            // Setup Gecko browser
            var xulLocation = @"xulrunner";
            Gecko.Xpcom.Initialize(xulLocation);
            Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;

            var context = Bootstrapper.GetDataContext();
            LoadSettingsAtStartup(context);
            LoadDefaultServers(context);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(Bootstrapper.GetMainForm(context));

            GC.KeepAlive(mutex);
        }

        static void LoadSettingsAtStartup(IDataContext context)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)
                return;

            if (String.IsNullOrEmpty(args[1]))
                return;

            context.ImportSettings(args.First());
        }

        static void LoadDefaultServers(IDataContext context)
        {
            if (!context.Settings.Servers.Any())
            {
                context.ImportSettings("DefaultSettings/__defaultSettings.psjson");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using PureSeeder.Core.Context;
using PureSeeder.Core.Initialization;
using PureSeeder.Core.ServerManagement;
using PureSeeder.Core.Settings;

namespace PureSeeder.Forms.Initalization
{
    public static class Bootstrapper
    {
        public static MainForm GetMainForm(IDataContext dataContext)
        {
            return new MainForm(dataContext);
        }

        public static IDataContext GetDataContext()
        {
            return new SeederContext(
                    new SessionData(),
                    new BindableSettings(
                        new SeederUserSettings()),
                    new List<IDataContextUpdater>
                        {
                            new CurrentBf4UserUpdater(),
                        }.ToArray(),
                    new ServerStatusUpdater(
                        new UpdateServerIds()),
                    new PlayerStatusGetter());
        }
    }
}
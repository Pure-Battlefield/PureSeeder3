using System;
using System.Collections.Generic;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;
using PureSeeder.Core.ServerManagement;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Initialization
{
    public class DefaultContainer : IContainer
    {
        public T Resolve<T>()
        {
            var type = typeof (T);

            return (T) CreateObject(type);
        }

        private object CreateObject(Type type)
        {
            if (type == typeof (SessionData))
                return new SessionData();

            if (type == typeof (BindableSettings))
                return new BindableSettings(
                    Resolve<SeederUserSettings>());

            if (type == typeof (IServerStatusUpdater))
                return new ServerStatusUpdater();

            if (type == typeof (IDataContext))
                return new SeederContext(
                    Resolve<SessionData>(),
                    Resolve<BindableSettings>(),
                    Resolve<IDataContextUpdater[]>(),
                    Resolve<IServerStatusUpdater>());

            if (type == typeof (SeederUserSettings))
                return new SeederUserSettings();

            if (type == typeof (IDataContextUpdater[]))
                return new List<IDataContextUpdater>
                    {
                        new Bf4PlayerCountsUpdater(),
                        new CurrentBf4UserUpdater(),
                    }.ToArray();



            throw new ArgumentException(
                String.Format("DefaultContainer cannot retrieve an instance of the required type: {0}", type.Name));

        }
    }
}
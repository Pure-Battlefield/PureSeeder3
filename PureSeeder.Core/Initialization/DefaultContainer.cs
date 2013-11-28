using System;
using System.Collections.Generic;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;
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
            if (type == typeof (IPureConfigHelper))
                return new PureConfigHelper(Constants.ConfigSectionName);
// Deprecated
//            if (type == typeof (IDataContext))
//                return new CurrentContext(
//                    Resolve<IPureConfigHelper>(),
//                    Resolve<IDataContextUpdater[]>());

            if (type == typeof (IDataContext))
                return new SeederContext(
                    Resolve<SeederUserSettings>(),
                    Resolve<IDataContextUpdater[]>());

            if (type == typeof (SeederUserSettings))
                return new SeederUserSettings();

            if (type == typeof (IDataContextUpdater[]))
                return new List<IDataContextUpdater>
                    {
                        new PlayerCountsUpdater(),
                    }.ToArray();

            throw new ArgumentException(
                String.Format("DefaultContainer cannot retrieve an instance of the required type: {0}", type.Name));

        }
    }
}
using System;
using System.Collections.Generic;
using PureSeeder.Core.Context;
using PureSeeder.Core.Initialization;
using PureSeeder.Core.ServerManagement;
using PureSeeder.Core.Settings;

namespace PureSeeder.Forms.Initalization
{
    public class FormsContainer : IContainer
    {
        private readonly IContainer _coreContainer;

        public FormsContainer()
        {
            _coreContainer = new DefaultContainer();
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            return (T) Resolve(type);
        }

        private object Resolve(Type type)
        {
            
            if (type == typeof(MainForm))
                return new MainForm(
                    _coreContainer.Resolve<IDataContext>());

            throw new ArgumentException(String.Format("FormsContainer cannot create an instance of the required type: {0}", type.Name));
            
        }
    }

    public class TempContainer : IContainer
    {
        public T Resolve<T>()
        {
            var type = typeof (T);
            return (T) Resolve(type);
        }

        private object Resolve(Type type){
            if (type == typeof (MainForm))
                return new MainForm(
                    new SeederContext(
                        new SessionData(),
                        new BindableSettings(
                            new SeederUserSettings()),
                        new List<IDataContextUpdater>
                            {
                                new Bf4PlayerCountsUpdater(),
                                new CurrentBf4UserUpdater(),
                            }.ToArray(), 
                        new ServerStatusUpdater()));

            throw new ArgumentException(String.Format("FormsContainer cannot create an instance of the required type: {0}", type.Name));
        }
    }

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
                            new Bf4PlayerCountsUpdater(),
                            new CurrentBf4UserUpdater(),
                        }.ToArray(),
                    new ServerStatusUpdater());
        }
    }
}
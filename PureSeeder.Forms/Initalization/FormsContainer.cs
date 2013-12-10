using System;
using System.Collections.Generic;
using PureSeeder.Core.Context;
using PureSeeder.Core.Initialization;
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
            
            if (type == typeof(Form1))
                return new Form1(
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
            if (type == typeof (Form1))
                return new Form1(
                    new SeederContext(
                        new SessionData(),
                        new BindableSettings(
                            new SeederUserSettings()),
                        new List<IDataContextUpdater>
                            {
                                new Bf4PlayerCountsUpdater(),
                                new CurrentBf4UserUpdater(),
                            }.ToArray()));

            throw new ArgumentException(String.Format("FormsContainer cannot create an instance of the required type: {0}", type.Name));
        }
    }
}
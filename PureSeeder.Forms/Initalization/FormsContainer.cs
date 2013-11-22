using System;
using PureSeeder.Core.Context;
using PureSeeder.Core.Initialization;

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
}
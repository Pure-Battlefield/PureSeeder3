using System;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;

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

            if (type == typeof (IDataContext))
                return new CurrentContext(
                    Resolve<IPureConfigHelper>());

            throw new ArgumentException(
                String.Format("DefaultContainer cannot retrieve an instance of the required type: {0}", type.Name));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PureSeeder.Core.Configuration
{
    public interface IPureConfigHelper
    {
        IList<Server> GetServers();
        string GetSetting(string settingName);
        T GetSetting<T>(string settingName);
        T GetSetting<T>(string settingName, T defaultValue);
    }

    public class PureConfigHelper : IPureConfigHelper
    {
        private readonly PureConfigSection _configSection;

        public PureConfigHelper(string configSectionName)
        {
            if (String.IsNullOrEmpty(configSectionName)) throw new ArgumentNullException("configSectionName");

            _configSection = ConfigurationManager.GetSection(configSectionName) as PureConfigSection;

            if (_configSection == null)
                throw new ConfigurationException("Pure Seeder configuration not found.");
        }

        public IList<Server> GetServers()
        {
            return _configSection.Servers != null ? _configSection.Servers.Cast<Server>().ToList() : null;
        }

        public string GetSetting(string settingName)
        {
            if (_configSection.Settings == null)
                return null;

            var setting = _configSection.Settings.GetItemByKey(settingName);

            return setting == null ? null : setting.Value;
        }

        public T GetSetting<T>(string settingName)
        {
            var setting = GetSetting(settingName);

            if (Nullable.GetUnderlyingType(typeof (T)) != null)
                return (T) Convert.ChangeType(setting, Nullable.GetUnderlyingType(typeof (T)));

            return (T) Convert.ChangeType(setting, typeof (T));
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            try
            {
                return this.GetSetting<T>(settingName);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
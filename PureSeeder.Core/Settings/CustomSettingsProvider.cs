using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Settings
{
    public interface ISettingsProvider<T> where T: new()
    {
        T GetSettings(bool freshCopy = false);
        void SaveSettings(T settings);
        IEnumerable<SettingsProvider.SettingDescriptor> ReadSettingMetaData();
        T ResetToDefaults();
    }

    class CustomSettingsProvider<T> : ISettingsProvider<T> where T: new()
    {
        private readonly ISettingsProvider _innerSettingsProvider;

        public CustomSettingsProvider(ISettingsProvider innerSettingsProvider)
        {
            if (innerSettingsProvider == null) throw new ArgumentNullException("innerSettingsProvider");
            _innerSettingsProvider = innerSettingsProvider;
        }

        public T GetSettings(bool freshCopy = false)
        {
            return _innerSettingsProvider.GetSettings<T>();
        }

        public void SaveSettings(T settings)
        {
            SaveSettings(settings);
        }

        public IEnumerable<SettingsProvider.SettingDescriptor> ReadSettingMetaData()
        {
            return _innerSettingsProvider.ReadSettingMetadata<T>();
        }

        public T ResetToDefaults()
        {
            return _innerSettingsProvider.ResetToDefaults<T>();
        }
    }
}

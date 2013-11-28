using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Settings
{
    class SeederUserSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("BattlelogUsername")]
        public string Username
        {
            get { return ((string) this[Constants.SettingNames.Username]); }
            set { this[Constants.SettingNames.Username] = (string) value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool EnableGameHangProtection
        {
            get { return ((bool) this[Constants.SettingNames.EnableGameHangProtection]); }
            set { this[Constants.SettingNames.EnableGameHangProtection] = (bool) value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool EnableLogging
        {
            get { return ((bool) this[Constants.SettingNames.EnableLogging]); }
            set { this[Constants.SettingNames.EnableLogging] = (bool) value; }
        }

        [UserScopedSetting()]
        public List<Server> Servers
        {
            get { return (List<Server>) this[Constants.SettingNames.Servers]; }
            set { this[Constants.SettingNames.Servers] = value; }
        }
    }

    public class Server
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
    }
}

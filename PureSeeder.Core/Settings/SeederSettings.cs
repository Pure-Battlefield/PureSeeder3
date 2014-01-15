using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Settings
{
    public class SeederUserSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("BattlelogUsername")]
        public string Username
        {
            get { return ((string) this[Constants.SettingNames.Username]); }
            set { this[Constants.SettingNames.Username] = (string) value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Email
        {
            get { return ((string) this[Constants.SettingNames.Email]); }
            set { this[Constants.SettingNames.Email] = (string) value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Password
        {
            get { return ((string) this[Constants.SettingNames.Password]); }
            set { this[Constants.SettingNames.Password] = (string) value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool EnableLogging
        {
            get { return ((bool) this[Constants.SettingNames.EnableLogging]); }
            set { this[Constants.SettingNames.EnableLogging] = (bool) value; }
        }

        [UserScopedSetting()]
        public Servers Servers
        {
            get
            {
                if (this[Constants.SettingNames.Servers] == null)
                {
                    //var defaultServers = GetDefaultServers();
                    var defaultServers = new Servers();
                    Servers = defaultServers;
                }
                return (Servers) this[Constants.SettingNames.Servers];
            }
            private set { this[Constants.SettingNames.Servers] = (Servers) value; }
        }
        
        [UserScopedSetting()]
        [DefaultSettingValue("0")]
        public int CurrentServer
        {
            get { return ((int) this[Constants.SettingNames.CurrentServer]); }
            set { this[Constants.SettingNames.CurrentServer] = (int) value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("60")]
        public int RefreshInterval
        {
            get { return ((int) this[Constants.SettingNames.RefreshInterval]); }
            set { this[Constants.SettingNames.RefreshInterval] = (int) value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool MinimizeToTray
        {
            get { return ((bool) this[Constants.SettingNames.MinimizeToTray]); }
            set { this[Constants.SettingNames.MinimizeToTray] = (bool) value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("false")]
        public bool AutoLogin
        {
            get { return ((bool) this[Constants.SettingNames.AutoLogin]); }
            set { this[Constants.SettingNames.AutoLogin] = (bool) value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("60")]
        public int IdleKickAvoidanceTimer
        {
            get { return ((int) this[Constants.SettingNames.IdleKickAvoidanceTimer]); }
            set { this[Constants.SettingNames.IdleKickAvoidanceTimer] = (int) value; }
        }
    }
}

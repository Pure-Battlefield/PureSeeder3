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
        public Servers Servers
        {
            get
            {
                if (this[Constants.SettingNames.Servers] == null)
                {
                    var defaultServers = GetDefaultServers();
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

        // Todo: Move this out of code and into configuration
        private static Servers GetDefaultServers()
        {
            var servers = new Servers()
                {
                    new Server()
                        {
                            Name = "Pure Server 2 - 32 Player Mixed Mode",
                            Address = "http://bf4-server2.purebattlefield.org",
                            MinPlayers = 18,
                            MaxPlayers = 32
                        },
                    new Server()
                        {
                            Name = "Pure Server 1 - 64 Player Conquest",
                            Address = "http://bf4-server1.purebattlefield.org",
                            MinPlayers = 32,
                            MaxPlayers = 64
                        }
                };
            return servers;
        }
    }
}

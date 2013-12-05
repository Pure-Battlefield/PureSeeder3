using System;
using System.Collections.Generic;
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
            get { return (Servers) this[Constants.SettingNames.Servers]; }
            set { this[Constants.SettingNames.Servers] = value; }
        }

        [UserScopedSetting()]
        public Server CurrentServer
        {
            get { return ((Server) this[Constants.SettingNames.CurrentServer]); }
            set { this[Constants.SettingNames.CurrentServer] = value; }
        }

        public void SetDefaultServers()
        {
            Servers = new Servers()
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
            Servers.CurrentServerIndex = 0;
        }
    }

    public class Servers : List<Server>
    {
        public int CurrentServerIndex { get; set; }
    }

    public class Server
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool SeedingEnabled { get; set; }
    }
}

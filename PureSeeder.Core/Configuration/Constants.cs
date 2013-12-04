using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Configuration
{
    public static class Constants
    {
        public const string ConfigSectionName = "PureConfig";

        public static class SettingNames
        {
            public const string EnableGameHangProtection = "EnableGameHangProtection";
            public const string DisplayPlayerCount = "DisplayPlayerCount";
            public const string SleepWhenNotSeeding = "SleepWhenNotSeeding";
            public const string SleepWhenSeeding = "SleepWhenSeeding";
            public const string EnableLogging = "EnableLogging";
            public const string Username = "Username";
            public const string Servers = "Servers";
            public const string CurrentServer = "CurrentServer";
        }

        public enum Game
        {
            BF3,
            BF4
        }
    }
}

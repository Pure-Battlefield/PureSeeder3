using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Configuration
{
    public static class Constants
    {
        public const string ApplicationName = "Pure Seeder 3";
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
            public const string RefreshInterval = "RefreshInterval";
            public const string MinimizeToTray = "MinimizeToTray";
        }

        public enum Game
        {
            BF3,
            BF4
        }

        public static class ProcessNames
        {
            //public const string Bf4 = "BF4";
            public const string Bf4 = "Crash";
            public const string Bf3 = "BF3";
        }

        public static class WindowTitles
        {
            //public const string Bf4FaultWindow = "Battlefield 4";
            public const string Bf4FaultWindow = "Crash";
        }

        public const int GameHangProtectionTimerInterval = 30; // minutes
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PureSeeder.Core.Configuration
{
    public static class Constants
    {
        public const string ApplicationName = "Pure Seeder 3";
        public const string ConfigSectionName = "PureConfig";

        public const string NotLoggedInUsername = "Not logged in";
        public const string DefaultUrl = "http://battlelog.battlefield.com/bf4/";

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
            public const string AutoLogin = "AutoLogin";
            public const string IdleKickAvoidanceTimer = "IdleKickAvoidanceTimer";
            public const string Email = "Email";
            public const string Password = "Password";
        }

        public const int GameHangProtectionTimerInterval = 30; // minutes

        // Todo: This should be moved to a custom config section
        public static readonly IEnumerable<GameInfo> Games = new List<GameInfo>
            {
                new GameInfo()
                    {
                        GameName = "Battlefield 4",
                        ProcessName = "Bf4",
                        WindowTitle = /*"Battlefield 4™"*/ "Battlefield 4",
                        UrlMatch = new Regex(@"/bf4/")
                    },
                /*new GameInfo()
                    {
                        GameName = "Crash",
                        ProcessName = "Crash",
                        WindowTitle = "Crash",
                        UrlMatch = new Regex(".*")
                    },*/
                new GameInfo()
                    {
                        GameName = "Battlefield 3",
                        ProcessName = "Bf3",
                        WindowTitle = "Battlefield 3™",
                        UrlMatch = new Regex(@"/bf3/")
                    }
            }; 

        public enum ShouldNotSeedReason
        {
            NotLoggedIn,
            IncorrectUser,
            NotInRange,
            GameAlreadyRunning
        }
    }
}

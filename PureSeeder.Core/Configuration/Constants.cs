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
            public const string StatusRefreshInterval = "StatusRefreshInterval";
            public const string MinimizeToTray = "MinimizeToTray";
            public const string AutoLogin = "AutoLogin";
            public const string IdleKickAvoidanceTimer = "IdleKickAvoidanceTimer";
            public const string Email = "Email";
            public const string Password = "Password";
            public const string AutoMinimizeSeeder = "AutoMinimizeSeeder";
            public const string AutoMinimizeGame = "AutoMinimizeGame";
        }

        public static class BattlelogUrlTemplates
        {
            public const string ServerStatus =
                "http://battlelog.battlefield.com/bf4/servers/getNumPlayersOnServer/pc/{0}/";
        }

        public const int GameHangProtectionTimerInterval = 30; // minutes

        public static class Games
        {
            public static GameInfo Bf4 = new GameInfo()
                {
                    GameName = "Battlefield 4",
                    ProcessName = "bf4",
                    WindowTitle = "Battlefield 4",
                    FaultWindowTitle = "Battlefield 4™",
                    UrlMatch = new Regex(@"/bf4/")
                };

            // Note: Right now we only support BF4
//            public static GameInfo Bf3 = new GameInfo()
//                {
//                    GameName = "Battlefield 3",
//                    ProcessName = "Bf3",
//                    WindowTitle = "Battlefield 3™",
//                    UrlMatch = new Regex(@"/bf3/")
//                };
        }
    }

    public enum ShouldNotSeedReason
    {
        NotLoggedIn,
        IncorrectUser,
        NotInRange,
        GameAlreadyRunning,
        NoServerDefined,
        SeedingDisabled
    }

    public enum KickReason
    {
        AboveSeedingRange,
        NoServerDefined,
        GameNotRunning
    }

    public enum UserStatus
    {
        Correct,
        Incorrect,
        None
    }
}

using System.Collections.Generic;
using System.ComponentModel;

namespace PureSeeder.Core.Settings
{
    public class PureSettings
    {
        [DisplayName("Server Addresses")]
        public List<string> ServerAddresses { get; set; }

        [DisplayName("Minimum Players")]
        public int MinimumPlayers { get; set; }

        [DisplayName("Maximum Players")]
        public int MaximumPlayers { get; set; }

        [DisplayName("SleepWhenNotSeeding")]
        public int SleepWhenNotSeeding { get; set; }

        [DisplayName("SleepWhenSeeding")]
        public int SleepWhenSeeding { get; set; }

        [DisplayName("Enable Logging")]
        public bool EnableLogging { get; set; }

        [DisplayName("Enable Game Hang Protection")]
        public bool EnableGameHangProtection { get; set; }
    }
}
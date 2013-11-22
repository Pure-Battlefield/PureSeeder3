using System;
using System.Collections.Generic;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Context
{
    public interface IDataContext
    {
        IList<Server> Servers { get; }
        int CurrentPlayers { get; set; }
        int ServerMaxPlayers { get; set; }
        bool HangProtectionStatus { get; }
        bool SeedStatus { get; set; }
    }

    public class CurrentContext : IDataContext
    {
        private readonly IPureConfigHelper _configHelper;

        public CurrentContext(IPureConfigHelper configHelper)
        {
            if (configHelper == null) throw new ArgumentNullException("configHelper");
            _configHelper = configHelper;
        }

        public IList<Server> Servers
        {
            get { return _configHelper.GetServers(); }
        }

        public int CurrentPlayers { get; set; }
        public int ServerMaxPlayers { get; set; }
        public bool HangProtectionStatus
        {
            get { return _configHelper.GetSetting<bool>(Constants.SettingNames.GameHangProtectionEnabled); }
        }
        public bool SeedStatus { get; set; }
    }
}
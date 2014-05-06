using System.Collections.Generic;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    public class SessionData : BindableBase
    {
        public SessionData()
        {
            _serverStatuses = new ServerStatusCollection();
        }
        
        private bool _seedingEnabled = true;
        private string _currentLoggedInUser;
        private GameInfo _currentGame = Constants.Games.Bf4;
        private bool _bfIsRunning;
        private ServerStatusCollection _serverStatuses;

        public bool SeedingEnabled
        {
            get { return this._seedingEnabled; }
            set { SetField(ref this._seedingEnabled, value); }
        }

        public string CurrentLoggedInUser
        {
            get { return this._currentLoggedInUser; }
            set { SetField(ref _currentLoggedInUser, value); }
        }

        public GameInfo CurrentGame
        {
            // Note: Right now we only support BF4
            get { return this._currentGame; }
            set { /*SetField(ref _currentGame, value);*/ }
        }

        public bool BfIsRunning
        {
            get { return this._bfIsRunning; }
            set { SetField(ref _bfIsRunning, value); }
        }

        public ServerStatusCollection ServerStatuses { get { return _serverStatuses; }}
    }
}
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Context
{
    public class SessionData : BindableBase
    {
        private int? _currentPlayers;
        private int? _serverMaxPlayers;
        private bool _seedingEnabled = true;
        private string _currentLoggedInUser;
        private GameInfo _currentGame = Constants.Games.Bf4;
        private bool _bfIsRunning;
        private int? _seedersOnCurrentServer;
        private bool _expansionEnabled;

        public int? CurrentPlayers
        {
            get { return this._currentPlayers; }
            set { SetField(ref _currentPlayers, value); }
        }

        public int? ServerMaxPlayers
        {
            get { return this._serverMaxPlayers; }
            set { SetField(ref _serverMaxPlayers, value); }
        }

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

        public int? SeedersOnCurrentServer
        {
            get { return this._seedersOnCurrentServer; }
            set { SetField(ref _seedersOnCurrentServer, value); }
        }

        public bool expansionEnabled
        {
            get { return this._expansionEnabled; }
            set { SetField(ref this._expansionEnabled, value); }
        }
    }
}
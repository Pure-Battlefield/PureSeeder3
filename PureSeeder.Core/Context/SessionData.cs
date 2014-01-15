﻿using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Context
{
    public class SessionData : BindableBase
    {
        private int? _currentPlayers;
        private int? _serverMaxPlayers;
        private bool _seedingEnabled = true;
        private string _currentLoggedInUser;
        private GameInfo _currentGame;
        private bool _bfIsRunning;
        private string _password;

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
            get { return this._currentGame; }
            set { SetField(ref _currentGame, value); }
        }

        public bool BfIsRunning
        {
            get { return this._bfIsRunning; }
            set { SetField(ref _bfIsRunning, value); }
        }

        public string Password
        {
            get { return this._password; }
            set { SetField(ref _password, value); }
        }

    }
}
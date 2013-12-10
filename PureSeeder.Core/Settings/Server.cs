using PureSeeder.Core.Context;

namespace PureSeeder.Core.Settings
{
    public class Server : BindableBase
    {
        private string _name;
        private string _address;
        private int _minPlayers;
        private int _maxPlayers;
        private bool _seedingEnabled;

        public string Name
        {
            get { return this._name; }
            set { SetField(ref _name, value); }
        }

        public string Address
        {
            get { return this._address; }
            set { SetField(ref _address, value); }
        }

        public int MinPlayers
        {
            get { return this._minPlayers; }
            set { SetField(ref _minPlayers, value); }
        }

        public int MaxPlayers
        {
            get { return this._maxPlayers; }
            set { SetField(ref _maxPlayers, value); }
        }

        public bool SeedingEnabled
        {
            get { return this._seedingEnabled; }
            set { SetField(ref _seedingEnabled, value); }
        }
    }
}
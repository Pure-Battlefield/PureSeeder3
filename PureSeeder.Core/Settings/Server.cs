using System.ComponentModel;
using PureSeeder.Core.Context;

namespace PureSeeder.Core.Settings
{
    public class Server : BindableBase
    {
        private string _name;
        private string _address;
        private int _minPlayers;
        private int _maxPlayers;
        private string _id;
        private bool _seedingEnabled;

        [Description("Name for the server")]
        public string Name
        {
            get { return this._name; }
            set { SetField(ref _name, value); }
        }

        [Description("Url for the server")]
        public string Address
        {
            get { return this._address; }
            set { SetField(ref _address, value); }
        }

        [Description("Unique ID of the server")]
        [Browsable(false)]
        public string Id
        {
            get { return this._id; }
            set { SetField(ref _id, value); }
        }
        
        [Description("Minimum player threshold. Seeding will happen if there are fewer than this many players.")]
        public int MinPlayers
        {
            get { return this._minPlayers; }
            set { SetField(ref _minPlayers, value); }
        }

        
        [Description("Maximum player threshold. Current seeding will stop if there are this many players or more.")]
        public int MaxPlayers
        {
            get { return this._maxPlayers; }
            set { SetField(ref _maxPlayers, value); }
        }

        [Description("Enables/disables seeding for this specific server.")]
        public bool SeedingEnabled
        {
            get { return this._seedingEnabled; }
            set { SetField(ref _seedingEnabled, value); }
        }

    }
}
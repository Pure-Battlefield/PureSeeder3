using System.ComponentModel;
using PureSeeder.Core.Context;

namespace PureSeeder.Core.Settings
{
    public class Server : BindableBase
    {
        protected string _name;
        protected string _address;
        protected int _minPlayers;
        protected int _maxPlayers;

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
    }
}
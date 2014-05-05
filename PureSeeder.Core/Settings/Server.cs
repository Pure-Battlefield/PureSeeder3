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
        private int _priority;

        [Description("Server priority")]
        [Browsable(false)]
        public int Priority
        {
            get { return this._priority; }
            set { SetField(ref _priority, value); }
        }

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

        public void SetPriority(int priority)
        {
            Priority = priority;
        }
    }
}
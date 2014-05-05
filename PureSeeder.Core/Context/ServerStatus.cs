using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    public class ServerStatus
    {
        private int? _curPlayers;
        private int? _maxPlayers;

        public int? CurPlayers
        {
            get { return _curPlayers; }
            set { _curPlayers = value; }
        }

        public int? MaxPlayers
        {
            get { return _maxPlayers; } 
            set { _maxPlayers = value; }
        }
    }
}

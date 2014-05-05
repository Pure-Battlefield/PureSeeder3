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
        private Server _server;
        private int _curPlayers;

        public Server Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public int CurPlayers
        {
            get { return _curPlayers; }
            set { _curPlayers = value; }
        }
    }
}

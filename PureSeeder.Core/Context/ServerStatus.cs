using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    public class ServerStatus : Server
    {
        private int? _curPlayers;
        private int? _serverMax;
        private string _serverGuid;

        private readonly Server _innerServer;

        public ServerStatus([NotNull] Server innerServer)
        {
            if (innerServer == null) throw new ArgumentNullException("innerServer");
            _innerServer = innerServer;
        }

        public int? CurPlayers
        {
            get { return _curPlayers; }
            set { SetField(ref _curPlayers, value); }
        }
        
        public int? ServerMax
        {
            get { return _serverMax; } 
            set { SetField(ref _serverMax, value); }
        }

        public new string Name
        {
            get { return _innerServer.Name; }
        }

        public new string Address
        {
            get { return _innerServer.Address; }
        }

        public new int MinPlayers
        {
            get { return _innerServer.MinPlayers; }
        }

        public new int MaxPlayers
        {
            get { return _innerServer.MaxPlayers; }
        }

        public new string Id
        {
            get { return _innerServer.Id; }
        }

        public new bool SeedingEnabled
        {
            get { return _innerServer.SeedingEnabled; }
            set { _innerServer.SeedingEnabled = value; }
        }
    }
}

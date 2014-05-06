using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{

    // Deprecated
//    public class ServerStatus : BindableBase
//    {
//        private int? _curPlayers;
//        private int? _serverMax;
//        private Server _server;
//
//        public ServerStatus([NotNull] Server innerServer)
//        {
//            if (innerServer == null) throw new ArgumentNullException("innerServer");
//            SetField(ref _server, innerServer);
//        }
//
//        public int? CurPlayers
//        {
//            get { return _curPlayers; }
//            set { SetField(ref _curPlayers, value); }
//        }
//
//        public int? ServerMax
//        {
//            get { return _serverMax; } 
//            set { SetField(ref _serverMax, value); }
//        }
//
//        public Server Server { get { return _server; } }
//    }

    public class ServerStatus : Server
    {
        private int? _curPlayers;
        private int? _serverMax;

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
            set { SetProperty(_innerServer.Name, value, s => s); }
        }

        public new string Address
        {
            get { return _innerServer.Address; }
            set { SetProperty(_innerServer.Address, value, s => s); }
        }
    }

    public class ServerStatusCollection : BindingList<ServerStatus>
    {
        private readonly Servers _serverCollection;
        
        public void SetInnerServerCollection([NotNull] Servers serverCollection)
        {
            if (serverCollection == null) throw new ArgumentNullException("serverCollection");

            Init(serverCollection);
        }

        private void Init(IEnumerable<Server> serverCollection)
        {
            foreach (var server in serverCollection)
            {
                this.Add(new ServerStatus(server));
            }
        }

        public void UpdateStatus(string address, int? curPlayers, int? serverMax)
        {
            if (!this.Any(x => x.Address == address))
                return;

            var server = this.First(x => x.Address == address);

            server.CurPlayers = curPlayers;
            server.ServerMax = serverMax;
        }
    }
}

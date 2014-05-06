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
            //set { SetProperty(_innerServer.Name, value, s => s); }
        }

        public new string Address
        {
            get { return _innerServer.Address; }
            //set { SetProperty(_innerServer.Address, value, s => s); }
        }

        public new int MinPlayers
        {
            get { return _innerServer.MinPlayers; }
        }

        public new int MaxPlayers
        {
            get { return _innerServer.MaxPlayers; }
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

        private void Init(Servers serverCollection)
        {
            serverCollection.ListChanged += ServerCollectionOnListChanged ;

            foreach (var server in serverCollection)
            {
                this.Add(new ServerStatus(server));
            }
        }

        private void ServerCollectionOnListChanged(object sender, ListChangedEventArgs listChangedEventArgs)
        {
            OnListChanged(listChangedEventArgs);
        }

        public void UpdateStatus(string address, int? curPlayers, int? serverMax)
        {
            if (!this.Any(x => x.Address == address))
                return;

            var server = this.First(x => x.Address == address);

            server.CurPlayers = curPlayers;
            server.ServerMax = serverMax;
        }

        public delegate void ServerChangedHandler(object sender, PropertyChangedEventArgs e);
        public event ServerChangedHandler ServerChanged;

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            if (e.NewObject != null)
                ((ServerStatus)e.NewObject).PropertyChanged += OnServerChanged;

            base.OnAddingNew(e);
        }

        protected override void RemoveItem(int index)
        {
            if (this[index] != null)
                this[index].PropertyChanged -= OnServerChanged;

            base.RemoveItem(index);
        }

        private void OnServerChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = ServerChanged;
            if (handler != null)
                handler(sender, e);
        }

        public void MoveUp(int index)
        {
            if (index <= 0) return;

            var item = this[index];
            var itemAbove = this[index - 1];

            this.RemoveAt(index);
            this.Insert(index - 1, item);
        }

        public void MoveDown(int index)
        {
            if (index + 1 >= this.Count) return;

            var item = this[index];
            var itemBelow = this[index + 1];

            this.RemoveAt(index);
            this.Insert(index + 1, item);
        }
    }
}

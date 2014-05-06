using System;
using System.Collections;
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
        private Servers _serverCollection;
        private Dictionary<string, Tuple<int?, int?>> _innerStatusCollection; 

        public ServerStatusCollection()
        {
            _serverCollection = new Servers();
            _innerStatusCollection = new Dictionary<string, Tuple<int?, int?>>();
        }
        
        public void SetInnerServerCollection([NotNull] Servers serverCollection)
        {
            if (serverCollection == null) throw new ArgumentNullException("serverCollection");
            _serverCollection = serverCollection;

            Init(serverCollection);

            _serverCollection.ListChanged += InnerListChanged;
        }

        private void Init(Servers serverCollection)
        {
            foreach (var server in serverCollection)
            {
                var newServerStatus = new ServerStatus(server);
                var innerStatus = _innerStatusCollection.FirstOrDefault(x => x.Key == server.Address).Value;
                if (innerStatus != null)
                {
                    newServerStatus.CurPlayers = innerStatus.Item1;
                    newServerStatus.ServerMax = innerStatus.Item2;
                }
                this.Add(newServerStatus);
            }
        }

        private void InnerListChanged(object sender, ListChangedEventArgs e)
        {
            this.Clear();

            Init((Servers) sender);
        }

        public new event ListChangedEventHandler ListChanged
        {
            add { _serverCollection.ListChanged += value; }
            remove { _serverCollection.ListChanged -= value; }
        }

        public new event AddingNewEventHandler AddingNew
        {
            add { _serverCollection.AddingNew += value; }
            remove { _serverCollection.AddingNew -= value; }
        }

        public new event Servers.ServerChangedHandler ServerChanged
        {
            add { _serverCollection.ServerChanged += value; }
            remove { _serverCollection.ServerChanged -= value; }
        }
      
        public void UpdateStatus(string address, int? curPlayers, int? serverMax)
        {
            _innerStatusCollection[address] = new Tuple<int?, int?>(curPlayers, serverMax);

            // Update if it exists in the collection
            if (!this.Any(x => x.Address == address))
                return;

            var server = this.First(x => x.Address == address);

            server.CurPlayers = curPlayers;
            server.ServerMax = serverMax;
        }

//        public delegate void ServerChangedHandler(object sender, PropertyChangedEventArgs e);
//        public event ServerChangedHandler ServerChanged;

//        protected override void OnAddingNew(AddingNewEventArgs e)
//        {
//            if (e.NewObject != null)
//                ((ServerStatus)e.NewObject).PropertyChanged += OnServerChanged;
//
//            base.OnAddingNew(e);
//        }
//
//        protected override void RemoveItem(int index)
//        {
//            if (this[index] != null)
//                this[index].PropertyChanged -= OnServerChanged;
//
//            base.RemoveItem(index);
//        }

//        private void OnServerChanged(object sender, PropertyChangedEventArgs e)
//        {
//            var handler = ServerChanged;
//            if (handler != null)
//                handler(sender, e);
//        }

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

    class cr : IList<object>
    {
        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(object item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public int IndexOf(object item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

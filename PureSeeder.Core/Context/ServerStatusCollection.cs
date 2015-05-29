using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    public class ServerStatusCollection : BindingList<ServerStatus>
    {
        private Servers _serverCollection;
        private readonly Dictionary<string, Tuple<int?, int?>> _innerStatusCollection; 

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

        public Servers Servers
        {
            get { return this._serverCollection; }
        }
    }
}
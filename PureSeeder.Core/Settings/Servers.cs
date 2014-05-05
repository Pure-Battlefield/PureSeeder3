using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PureSeeder.Core.Settings
{
    public class Servers : BindingList<Server>
    {
        public delegate void ServerChangedHandler(object sender, PropertyChangedEventArgs e);
        public event ServerChangedHandler ServerChanged;

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            if (e.NewObject != null)
                ((Server) e.NewObject).PropertyChanged += OnServerChanged;

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
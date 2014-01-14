using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

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

    }

//    public class Servers : ObservableCollection<Server>
//    {
//        public delegate void ServerChangedHandler(object sender, PropertyChangedEventArgs e);
//        public event ServerChangedHandler ServerChanged;
//
//        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//        {
//            if (e.NewItems != null)
//                foreach (Server item in e.NewItems)
//                    item.PropertyChanged += OnServerChanged;
//
//            if (e.OldItems != null)
//                foreach (Server oldItem in e.OldItems)
//                    oldItem.PropertyChanged -= OnServerChanged;
//
//            base.OnCollectionChanged(e);
//        }
//
//        private void OnServerChanged(object sender, PropertyChangedEventArgs e)
//        {
//            var handler = ServerChanged;
//            if (handler != null)
//                handler(sender, e);
//        }
//    }
}
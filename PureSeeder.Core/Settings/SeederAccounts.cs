using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace PureSeeder.Core.Settings
{
    public class SeederAccounts : BindingList<SeederAccount>
    {
        public delegate void SeederAccountChangedHandler(object sender, PropertyChangedEventArgs e);
        public event SeederAccountChangedHandler SeederAccountChanged;

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            if (e.NewObject != null)
                ((SeederAccount)e.NewObject).PropertyChanged += OnSeederAccountChanged;

            base.OnAddingNew(e);
        }

        protected override void RemoveItem(int index)
        {
            if (this[index] != null)
                this[index].PropertyChanged -= OnSeederAccountChanged;

            base.RemoveItem(index);
        }

        private void OnSeederAccountChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = SeederAccountChanged;
            if (handler != null)
                handler(sender, e);
        }

    }
}

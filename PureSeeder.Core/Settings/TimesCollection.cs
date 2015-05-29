using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PureSeeder.Core.Settings
{
    public class TimesCollection : BindingList<Times>
    {
        public delegate void TimesCollectionChangedHandler(object sender, PropertyChangedEventArgs e);
        public event TimesCollectionChangedHandler TimesCollectionChanged;

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            if (e.NewObject != null)
                ((Times) e.NewObject).PropertyChanged += OnTimesCollectionChanged;

            base.OnAddingNew(e);
        }

        protected override void RemoveItem(int index)
        {
            if (this[index] != null)
                this[index].PropertyChanged -= OnTimesCollectionChanged;

            base.RemoveItem(index);
        }

        private void OnTimesCollectionChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = TimesCollectionChanged;
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

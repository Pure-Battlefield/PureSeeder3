using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using PureSeeder.Core.Annotations;

namespace PureSeeder.Core.Context
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T1, T2>(T1 storage, T2 value, Expression<Func<T1, T2>> outExpr)
        {
            if (object.Equals(storage, value)) return false;

            var expr = (MemberExpression) outExpr.Body;
            var prop = (PropertyInfo) expr.Member;
            prop.SetValue(storage, value);
            this.OnPropertyChanged(prop.Name);
            return true;
        }
    }
}
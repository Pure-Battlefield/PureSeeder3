using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace PureSeeder.Forms.Extensions
{
    public static class BindingExtensions
    {
        public static Binding Add<T1, T2>(this ControlBindingsCollection bindingsColection, string propertyName, T1 dataSource,
                            Expression<Func<T1, T2>> dataMember)
        {
            return Add<T1, T2>(bindingsColection, propertyName, dataSource, dataMember, true,
                               DataSourceUpdateMode.OnPropertyChanged);
        }

        public static Binding Add<T1, T2>(this ControlBindingsCollection bindingsColection, string propertyName,
                                          T1 dataSource,
                                          Expression<Func<T1, T2>> dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode)
        {
            var expr = (MemberExpression)dataMember.Body;
            var prop = (PropertyInfo)expr.Member;
            return bindingsColection.Add(propertyName, dataSource, prop.Name, formattingEnabled, updateMode);
        }
    }
}
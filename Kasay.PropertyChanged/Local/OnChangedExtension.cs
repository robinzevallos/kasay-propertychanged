using Kasay.PropertyChanged.Infrastructure;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Kasay.PropertyChanged
{
    public static class OnChangedExtension
    {
        public static TObject OnChanged<TObject, TProperty>(
            this TObject @this,
            Expression<Func<TObject, TProperty>> propExpression,
            Action callback)
            where TObject : INotifyPropertyChanged
        {
            var propertyName = (propExpression.Body as MemberExpression).Member.Name;

            new OnChangedSubscriber(
                @this,
                callback,
                propertyName);

            return @this;
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViewModelSupport
{
    public static class Extensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static string StripLeft(this string value, int length)
        {
            return value.Substring(length, value.Length - length);
        }

        public static void Raise(this PropertyChangedEventHandler eventHandler, object source, string propertyName)
        {
            var handlers = eventHandler;
            if (handlers != null)
                handlers(source, new PropertyChangedEventArgs(propertyName));
        }

        public static void Raise(this EventHandler eventHandler, object source)
        {
            var handlers = eventHandler;
            if (handlers != null)
                handlers(source, EventArgs.Empty);
        }

        public static void Register(this INotifyPropertyChanged model, string propertyName, Action whenChanged)
        {
            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName)
                    whenChanged();
            };
        }
    }
}

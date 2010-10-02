using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using System.Diagnostics;

namespace System.Windows
{
    public class DependencyObject : DispatcherObject
    {
        public DependencyObject()
        {
            Initialize();
        }

        Dictionary<DependencyProperty, DependencyPropertyStorage> myProperties = new Dictionary<DependencyProperty, DependencyPropertyStorage>();

        public T GetValue<T>(DependencyProperty property)
        {
            return GetStorage<T>(property).Value;
        }

        public void SetValue<T>(DependencyProperty property, T value)
        {
            GetStorage<T>(property).Value = value;
        }

        public DependencyPropertyStorage GetStorage(DependencyProperty property)
        {
            DependencyPropertyStorage storage;
            if (myProperties.TryGetValue(property, out storage))
                return storage;

            Type storageType;
            Type type = property.Type;
            if (type.IsValueType)
                storageType = typeof(DependencyPropertyValueTypeStorage<>);
            else if (type.IsSubclassOf(typeof(DependencyObject)))
                storageType = typeof(DependencyPropertyDependencyObjectStorage<>);
            else
                storageType = typeof(DependencyPropertyClassStorage<>);

            storage = Activator.CreateInstance(storageType.MakeGenericType(type)) as DependencyPropertyStorage;
            storage.Initialize(this, property);
            myProperties.Add(property, storage);
            return storage;
        }

        public DependencyPropertyStorage<T> GetStorage<T>(DependencyProperty property)
        {
            return (DependencyPropertyStorage<T>)GetStorage(property);
        }

        internal void OnPropertyChangedInternal(DependencyPropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
            foreach (DependencyObjectWatcher watcher in myWatchers)
            {
                watcher.Object.OnPropertyChangedInternal(new DependencyPropertyChangedEventArgs(watcher.Property, this, this, watcher.Storage));
            }
        }

        protected virtual void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        protected virtual void Initialize()
        {
        }

        internal List<DependencyObjectWatcher> myWatchers = new List<DependencyObjectWatcher>();
    }

    struct DependencyObjectWatcher
    {
        public DependencyObjectWatcher(DependencyObject watcher, DependencyProperty property, object storage)
        {
            Object = watcher;
            Property = property;
            Storage = storage;
        }

        public DependencyObject Object;
        public DependencyProperty Property;
        public object Storage;
    }
}
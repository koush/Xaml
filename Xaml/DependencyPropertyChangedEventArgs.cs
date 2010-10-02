using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct DependencyPropertyChangedEventArgs
    {
        internal DependencyPropertyChangedEventArgs(DependencyProperty property, object oldValue, object newValue, object storage)
        {
            myOldValue = oldValue;
            myNewValue = newValue;
            myProperty = property;
            myStorage = storage;
        }

        public DependencyPropertyStorage<T> GetStorage<T>()
        {
            return myStorage as DependencyPropertyStorage<T>;
        }

        object myOldValue;

        public object OldValue
        {
            get { return myOldValue; }
        }
        object myNewValue;

        public object NewValue
        {
            get { return myNewValue; }
        }
        DependencyProperty myProperty;

        public DependencyProperty Property
        {
            get { return myProperty; }
        }

        object myStorage;
    }
}

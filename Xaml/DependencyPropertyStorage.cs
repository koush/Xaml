using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    enum EffectiveValueIndex : int
    {
        Animation,
        User,
        MaxPriority,
    }

    public abstract class DependencyPropertyStorage
    {
        DependencyObject myOwner;
        internal abstract DependencyProperty GetPropertyInternal();
        internal abstract object GetEffectiveValueInternal(EffectiveValueIndex index);
        internal abstract void SetEffectiveValueInternal(EffectiveValueIndex index, object value);
        internal abstract Type GetStorageTypeInternal();
        public abstract void ClearValue();

        internal void SetEffectiveValue(EffectiveValueIndex index, object value)
        {
            SetEffectiveValueInternal(index, value);
        }

        internal object GetEffectiveValue(EffectiveValueIndex index)
        {
            return GetEffectiveValueInternal(index);
        }

        public DependencyObject Owner
        {
            get
            {
                return myOwner;
            }
        }

        internal virtual void Initialize(DependencyObject owner, DependencyProperty property)
        {
            myOwner = owner;
        }

        public DependencyProperty Property
        {
            get
            {
                return GetPropertyInternal();
            }
        }

        public Type StorageType
        {
            get
            {
                return GetStorageTypeInternal();
            }
        }

        public object Value
        {
            get
            {
                return GetEffectiveValueInternal(EffectiveValueIndex.User);
            }
            set
            {
                SetEffectiveValueInternal(EffectiveValueIndex.User, value);
            }
        }
    }

    public abstract class DependencyPropertyStorage<T> : DependencyPropertyStorage
    {
        internal bool[] myIsValueSet = new bool[(int)EffectiveValueIndex.MaxPriority];
        internal T[] myEffectiveValues = new T[(int)EffectiveValueIndex.MaxPriority];
        internal int myEffectiveValueIndex = (int)EffectiveValueIndex.MaxPriority;
        DependencyPropertyTyped<T> myProperty;

        public DependencyProperty Property
        {
            get
            {
                return myProperty;
            }
        }

        internal DependencyPropertyStorage()
        {
        }

        internal override void Initialize(DependencyObject owner, DependencyProperty property)
        {
            base.Initialize(owner, property);
            myProperty = property as DependencyPropertyTyped<T>;
        }

        internal virtual void OnPropertyChanged(T oldValue, T newValue)
        {
            if (myProperty.myHandler != null)
                myProperty.myHandler(Owner, oldValue, newValue);
            Owner.OnPropertyChangedInternal(new DependencyPropertyChangedEventArgs(myProperty, oldValue, newValue, this));
        }

        public new T Value
        {
            get
            {
                if (myEffectiveValueIndex == (int)EffectiveValueIndex.MaxPriority)
                    return (T)myProperty.DefaultValue;
                return myEffectiveValues[myEffectiveValueIndex];
            }
            set
            {
                SetEffectiveValue(EffectiveValueIndex.User, value);
            }
        }

        public static implicit operator T(DependencyPropertyStorage<T> storage)
        {
            return storage.Value;
        }

        public override void ClearValue()
        {
            UnsetValue(EffectiveValueIndex.User);
        }

        internal void UnsetValue(EffectiveValueIndex index)
        {
            int ipri = (int)index;
            // it may already be unset, so check
            if (!myIsValueSet[ipri])
                return;
            // flag it as unset
            myIsValueSet[ipri] = false;
            // see if we're unsetting the current value
            if (ipri == myEffectiveValueIndex)
            {
                // we are unsetting the current value, so let's find the new value
                T oldValue = myEffectiveValues[ipri];
                for (myEffectiveValueIndex = myEffectiveValueIndex + 1; myEffectiveValueIndex < (int)EffectiveValueIndex.MaxPriority; myEffectiveValueIndex++)
                {
                    if (myIsValueSet[myEffectiveValueIndex])
                        break;
                }
                T newValue;
                if (myEffectiveValueIndex == (int)EffectiveValueIndex.MaxPriority)
                    newValue = (T)myProperty.DefaultValue;
                else
                    newValue = myEffectiveValues[myEffectiveValueIndex];

                myEffectiveValues[ipri] = GetUnsetValue();

                // see if the new value/default value is any different from the old value
                if (!Compare(newValue, oldValue))
                    OnPropertyChanged(oldValue, newValue);
            }
            else
            {
                // just unset it
                myEffectiveValues[ipri] = GetUnsetValue();
            }
        }

        internal T GetEffectiveValue(EffectiveValueIndex index)
        {
            return myEffectiveValues[(int)index];
        }

        internal void SetEffectiveValue(EffectiveValueIndex index, T value)
        {
            int ipri = (int)index;
            myIsValueSet[ipri] = true;

            // see if we are changing the current value
            if (ipri <= myEffectiveValueIndex)
            {
                // since the index is less than the current index, that implies that
                // the index is unset
                // get the old value
                T oldValue;
                if (myEffectiveValueIndex == (int)EffectiveValueIndex.MaxPriority)
                    oldValue = GetUnsetValue();
                else
                    oldValue = myEffectiveValues[myEffectiveValueIndex];
                myEffectiveValueIndex = ipri;
                myEffectiveValues[ipri] = value;
                // see if the current value changed
                if (!Compare(oldValue, value))
                    OnPropertyChanged(oldValue, value);
            }
            else
            {
                // just set it
                myEffectiveValues[ipri] = value;
            }
        }

        internal abstract bool Compare(T first, T second);
        internal abstract T GetUnsetValue();

        internal override DependencyProperty GetPropertyInternal()
        {
            return myProperty;
        }

        internal override object GetEffectiveValueInternal(EffectiveValueIndex index)
        {
            return GetEffectiveValue(index);
        }

        internal override void SetEffectiveValueInternal(EffectiveValueIndex index, object value)
        {
            SetEffectiveValue(index, (T)value);
        }

        internal override Type GetStorageTypeInternal()
        {
            return typeof(T);
        }
    }

    sealed class DependencyPropertyDependencyObjectStorage<T> : DependencyPropertyClassStorage<T> where T : DependencyObject
    {
        internal override void Initialize(DependencyObject owner, DependencyProperty property)
        {
            base.Initialize(owner, property);
            myWatcher = new DependencyObjectWatcher(owner, property, this);
        }

        DependencyObjectWatcher myWatcher;

        internal override void OnPropertyChanged(T oldValue, T newValue)
        {
            // unhook the watcher from the old object
            if (oldValue != null)
                oldValue.myWatchers.Remove(myWatcher);
            // add the watcher to the new object
            if (newValue != null)
                newValue.myWatchers.Add(myWatcher);
            // now fire the property changed
            base.OnPropertyChanged(oldValue, newValue);
        }
    }

    class DependencyPropertyClassStorage<T> : DependencyPropertyStorage<T> where T: class
    {
        // compare two classes
        internal override bool Compare(T first, T second)
        {
            if (first == null ^ second == null)
                return false;
            else if (first == null || second == null)
                return true;
            return first.Equals(second);
        }

        internal override T GetUnsetValue()
        {
            return null;
        }
    }

    sealed class DependencyPropertyValueTypeStorage<T> : DependencyPropertyStorage<T> where T : struct
    {
        public DependencyPropertyValueTypeStorage()
        {
        }

        // compare two structs
        internal override bool Compare(T first, T second)
        {
            return first.Equals(second);
        }

        internal override T GetUnsetValue()
        {
            return new T();
        }
    }
}

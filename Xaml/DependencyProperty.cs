using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    [Flags]
    public enum DependencyPropertyFlags : uint
    {
        AffectsNothing = 0x00000000,
        AffectsVisual = 0x00000001,
        AffectsMeasure = 0x00000002 | AffectsVisual,
    }

    public abstract class DependencyProperty
    {
        static int myDependencyPropertyCount = 0;

        internal int myGlobalIndex = myDependencyPropertyCount++;

        public int GlobalIndex
        {
            get { return myGlobalIndex; }
            set { myGlobalIndex = value; }
        }

        internal DependencyProperty()
        {
        }

        bool myIsAttached = true;
        public bool IsAttached
        {
            get { return myIsAttached; }
        }

        string myName;
        public string Name
        {
            get
            {
                return myName;
            }
        }



        Type myType;
        public Type Type
        {
            get
            {
                return myType;
            }
        }

        DependencyPropertyFlags myFlags;
        public DependencyPropertyFlags Flags
        {
            get
            {
                return myFlags;
            }
        }

        public static DependencyProperty Register<T>(string name, T defaultValue, bool isAttached, DependencyPropertyFlags flags, DependencyPropertyChangedHandler<T> handler)
        {
            DependencyPropertyTyped<T> property = new DependencyPropertyTyped<T>();
            property.myFlags = flags;
            property.myName = name;
            property.myDefaultValue = defaultValue;
            property.myIsAttached = isAttached;
            property.myType = typeof(T);
            property.myHandler = handler;
            return property;
        }

        public object DefaultValue
        {
            get
            {
                return GetDefaultValue();
            }
        }

        internal abstract object GetDefaultValue();
    }

    class DependencyPropertyTyped<T> : DependencyProperty
    {
        internal DependencyPropertyChangedHandler<T> myHandler;
        internal T myDefaultValue;

        public new T DefaultValue
        {
            get
            {
                return myDefaultValue;
            }
        }

        internal override object GetDefaultValue()
        {
            return myDefaultValue;
        }
    }

    public delegate void DependencyPropertyChangedHandler<T>(DependencyObject dependencyObject, T oldValue, T newValue);
}

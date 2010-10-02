using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace System.Windows.Controls
{
    public class ControlCollection : Collection<Control>
    {
        internal ControlCollection(Control owner)
        {
            myOwner = owner;
        }

        Control myOwner;
        protected override void ClearItems()
        {
            foreach (Control control in this)
            {
                control.myParent = null;
            }
            base.ClearItems();
            
            myOwner.InvalidateMeasure();
        }

        protected override void InsertItem(int index, Control item)
        {
            item.myParent = myOwner;
            base.InsertItem(index, item);

            myOwner.InvalidateMeasure();
        }

        protected override void RemoveItem(int index)
        {
            Control control = this[index];
            control.myParent = null;
            base.RemoveItem(index);

            myOwner.InvalidateMeasure();
        }

        protected override void SetItem(int index, Control item)
        {
            Control old = this[index];
            old.myParent = null;
            item.myParent = myOwner;
            base.SetItem(index, item);

            myOwner.InvalidateMeasure();
        }
    }
}

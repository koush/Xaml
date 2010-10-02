using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace System.Windows.Controls
{
    public abstract class Panel : Control
    {
        public Panel()
        {
            InitializeChildren();
        }

        public ControlCollection Children
        {
            get
            {
                return myChildren;
            }
        }
    }
}

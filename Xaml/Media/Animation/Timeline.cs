using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media.Animation
{
    public abstract class Timeline
    {
        public Duration Duration
        {
            get;
            set;
        }

        public bool AutoReverse
        {
            get;
            set;
        }

        public Clock CreateClock()
        {
            return AllocateClock();
        }

        protected internal virtual Clock AllocateClock()
        {
            return new Clock(this);
        }

        RepeatBehavior myRepeatBehavior = new RepeatBehavior(1);
        public RepeatBehavior RepeatBehavior
        {
            get
            {
                return myRepeatBehavior;
            }
            set
            {
                myRepeatBehavior = value;
            }
        }
    }
}

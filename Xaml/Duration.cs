using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    enum DurationType
    {
        TimeSpan,
        Automatic,
        Forever,
    }

    public struct Duration
    {
        DurationType myDurationType;
        TimeSpan myTimeSpan;

        public bool HasTimeSpan
        {
            get
            {
                return myDurationType == DurationType.TimeSpan;
            }
        }

        public TimeSpan TimeSpan
        {
            get
            {
                System.Diagnostics.Debug.Assert(HasTimeSpan);
                return myTimeSpan;
            }
        }

        public Duration(TimeSpan timeSpan)
        {
            myTimeSpan = timeSpan;
            myDurationType = DurationType.TimeSpan;
        }

        private Duration(DurationType duration)
        {
            myDurationType = duration;
            myTimeSpan = System.TimeSpan.Zero;
        }

        public static bool operator ==(Duration one, Duration two)
        {
            return one.myDurationType == two.myDurationType && one.myTimeSpan == two.myTimeSpan;
        }

        public static bool operator !=(Duration one, Duration two)
        {
            return one.myDurationType != two.myDurationType || one.myTimeSpan != two.myTimeSpan;
        }

        public static readonly Duration Automatic = new Duration(DurationType.Automatic);
        public static readonly Duration Forever = new Duration(DurationType.Forever);
    }
}

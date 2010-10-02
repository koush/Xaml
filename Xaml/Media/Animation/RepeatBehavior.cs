using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media.Animation
{
    public struct RepeatBehavior
    {
        float? myCount;
        Duration? myDuration;

        public RepeatBehavior(float count)
        {
            myDuration = null;
            myCount = count;
        }

        public RepeatBehavior(Duration duration)
        {
            myCount = null;
            myDuration = duration;
        }

        public float Count
        {
            get
            {
                return myCount.Value;
            }
        }

        public Duration Duration
        {
            get
            {
                return myDuration.Value;
            }
        }

        public bool HasDuration
        {
            get
            {
                return myDuration != null;
            }
        }

        public bool HasCount
        {
            get
            {
                return myCount != null;
            }
        }

        public static bool operator !=(RepeatBehavior one, RepeatBehavior two)
        {
            return one.myCount != two.myCount || one.myDuration == two.myDuration;
        }

        public static bool operator ==(RepeatBehavior one, RepeatBehavior two)
        {
            return !(one != two);
        }

        public static readonly RepeatBehavior Forever = new RepeatBehavior(float.PositiveInfinity);
    }
}

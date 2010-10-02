using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media.Animation
{
    public abstract class AnimationTimeline : Timeline
    {
    }

    public abstract class AnimationTimeline<T> : AnimationTimeline
    {
        public new AnimationClock<T> CreateClock()
        {
            return base.CreateClock() as AnimationClock<T>;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media.Animation
{
    public interface IAnimationTarget<T>
    {
        T Subtract(T other);
        T Add(T other);
        T Scale(float scalar);
    }

    public class AnimationBaseClock<T, V> : AnimationClock<T>
        where T : struct
        where V : IAnimationTarget<V>
    {
        AnimationBase<T, V> myTimeline;
        protected internal AnimationBaseClock(AnimationBase<T, V> timeline, T? to, T? from)
            : base(timeline)
        {
            myTo = to;
            myFrom = from;
            myTimeline = timeline;
        }

        T? myFrom;
        protected T? From
        {
            get
            {
                return myFrom;
            }
        }

        T? myTo;
        protected T? To
        {
            get
            {
                return myTo;
            }
        }

        public override T GetCurrentValue(T defaultOriginValue, T defaultDestinationValue)
        {
            V origin = myTimeline.Convert((myFrom == null) ? defaultOriginValue : myFrom.Value);
            V destination = myTimeline.Convert((myTo == null) ? defaultDestinationValue : myTo.Value);

            float timeLeft = this.CurrentProgress.Value;

            V diff = destination.Subtract(origin);
            diff = diff.Scale(timeLeft);

            V current = origin.Add(diff);
            return myTimeline.Convert(current);
        }
    }

    public abstract class AnimationBase<T, V> : AnimationTimeline<T>
        where T : struct
        where V : IAnimationTarget<V>
    {
        public T? From
        {
            get;
            set;
        }

        public T? To
        {
            get;
            set;
        }

        protected internal abstract V Convert(T source);
        protected internal abstract T Convert(V source);

        protected internal override Clock AllocateClock()
        {
            return new AnimationBaseClock<T, V>(this, To, From);
        }
    }
}

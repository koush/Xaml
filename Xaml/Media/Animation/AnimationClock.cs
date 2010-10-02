using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media.Animation
{
    public abstract class AnimationClock : Clock
    {
        AnimationTimeline myTimeline;
        List<DependencyPropertyStorage> myAnimatedProperties = new List<DependencyPropertyStorage>();

        protected internal AnimationClock(AnimationTimeline timeline)
            : base(timeline)
        {
            myTimeline = timeline;
        }

        public void Animate(DependencyObject target, DependencyProperty property)
        {
            Animate(target.GetStorage(property));
        }

        public void Animate(DependencyPropertyStorage storage)
        {
            myAnimatedProperties.Add(storage);
        }

        protected override void OnClockChanged()
        {
            foreach (var storage in myAnimatedProperties)
            {
                object defaultValue = storage.Property.DefaultValue;
                storage.SetEffectiveValue(EffectiveValueIndex.Animation, GetCurrentValueInternal(defaultValue, defaultValue));
            }
        }
        
        internal abstract object GetCurrentValueInternal(object defaultOriginValue, object defaultDestinationValue);
    }

    public abstract class AnimationClock<T> : AnimationClock
    {
        public abstract T GetCurrentValue(T defaultOriginValue, T defaultDestinationValue);
        AnimationTimeline<T> myTimeline;

        protected internal AnimationClock(AnimationTimeline<T> timeline)
            : base(timeline)
        {
            myTimeline = timeline;
        }

        internal override object GetCurrentValueInternal(object defaultOriginValue, object defaultDestinationValue)
        {
            return GetCurrentValue((T)defaultOriginValue, (T)defaultDestinationValue);
        }
    }
}

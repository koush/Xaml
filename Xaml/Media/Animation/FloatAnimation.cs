using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media.Animation
{
    public struct AnimatableFloat : IAnimationTarget<AnimatableFloat>
    {
        public float Value
        {
            get;
            set;
        }

        public static implicit operator float(AnimatableFloat source)
        {
            return source.Value;
        }

        public static implicit operator AnimatableFloat(float source)
        {
            AnimatableFloat ret = new AnimatableFloat();
            ret.Value = source;
            return ret;
        }

        #region IAnimationTarget<AnimatableFloat> Members

        public AnimatableFloat Subtract(AnimatableFloat other)
        {
            return Value - other.Value;
        }

        public AnimatableFloat Add(AnimatableFloat other)
        {
            return Value + other.Value;
        }

        public AnimatableFloat Scale(float scalar)
        {
            return scalar * Value;
        }

        #endregion
    }

    public class FloatAnimation : AnimationBase<float, AnimatableFloat>
    {
        protected internal override AnimatableFloat Convert(float source)
        {
            return source;
        }

        protected internal override float Convert(AnimatableFloat source)
        {
            return source;
        }
    }
}

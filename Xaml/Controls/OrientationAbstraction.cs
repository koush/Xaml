using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Controls
{
    public abstract class OrientationAbstraction
    {
        public Size GetSize(float primary, float secondary)
        {
            Size ret = SetPrimarySize(Size.Empty, primary);
            return SetSecondarySize(ret, secondary);
        }

        public abstract float GetPrimarySize(Size size);
        public abstract float GetSecondarySize(Size size);
        public abstract Size SetPrimarySize(Size size, float value);
        public abstract Size SetSecondarySize(Size size, float value);
        public abstract float GetPrimaryLocation(Rect rect);
        public abstract Rect SetPrimaryLocation(Rect rect, float value);
        public abstract Rect SetSecondaryLocation(Rect rect, float value);
    }

    public class VerticalAccessor : OrientationAbstraction
    {
        private VerticalAccessor()
        {
        }

        public static readonly VerticalAccessor Instance = new VerticalAccessor();

        public override float GetPrimarySize(Size size)
        {
            return size.Height;
        }

        public override float GetSecondarySize(Size size)
        {
            return size.Width;
        }

        public override Size SetPrimarySize(Size size, float value)
        {
            size.Height = value;
            return size;
        }

        public override float GetPrimaryLocation(Rect rect)
        {
            return rect.Y;
        }

        public override Rect SetPrimaryLocation(Rect rect, float value)
        {
            rect.Y = value;
            return rect;
        }

        public override Rect SetSecondaryLocation(Rect rect, float value)
        {
            rect.X = value;
            return rect;
        }

        public override Size SetSecondarySize(Size size, float value)
        {
            size.Width = value;
            return size;
        }
    }

    public class HorizontalAccessor : OrientationAbstraction
    {
        private HorizontalAccessor()
        {
        }

        public static readonly HorizontalAccessor Instance = new HorizontalAccessor();

        public override float GetPrimarySize(Size size)
        {
            return size.Width;
        }

        public override float GetSecondarySize(Size size)
        {
            return size.Height;
        }

        public override Size SetPrimarySize(Size size, float value)
        {
            size.Width = value;
            return size;
        }

        public override float GetPrimaryLocation(Rect rect)
        {
            return rect.X;
        }

        public override Rect SetPrimaryLocation(Rect rect, float value)
        {
            rect.X = value;
            return rect;
        }

        public override Rect SetSecondaryLocation(Rect rect, float value)
        {
            rect.Y = value;
            return rect;
        }

        public override Size SetSecondarySize(Size size, float value)
        {
            size.Height = value;
            return size;
        }
    }
}
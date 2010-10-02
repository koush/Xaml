using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct Rect
    {
        public static readonly Rect Empty = new Rect(0, 0, 0, 0);

        public Rect(float x, float y, float width, float height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public float Left
        {
            get
            {
                return X;
            }
        }

        public float Top
        {
            get
            {
                return Y;
            }
        }

        public float Right
        {
            get
            {
                return X + Width;
            }
        }

        public float Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public Point TopLeft
        {
            get
            {
                return new Point(X, Y);
            }
        }

        public Point TopRight
        {
            get
            {
                return new Point(Right, Y);
            }
        }

        public Point BottomLeft
        {
            get
            {
                return new Point(X, Bottom);
            }
        }

        public Point BottomRight
        {
            get
            {
                return new Point(Right, Bottom);
            }
        }

        public static Rect GetBoundingBox(Point[] points, int index, int count)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;
            for (int i = index; i < index + count; i++)
            {
                Point point = points[i];
                left = Math.Min(left, point.X);
                top = Math.Min(top, point.Y);
                right = Math.Max(right, point.X);
                bottom = Math.Max(bottom, point.Y);
            }

            return new Rect(left, top, right - left, bottom - top);
        }
    }
}

using System;

using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct Vector
    {
        public Vector(float x, float y)
            : this()
        {
            X = x;
            Y = y;
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

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(LengthSquare);
            }
        }

        public float LengthSquare
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        public Vector Normalize()
        {
            return Scale(1 / Length);
        }

        public Vector Scale(float scale)
        {
            Vector ret = this;
            ret.X *= scale;
            ret.Y *= scale;
            return ret;
        }

        public static Vector operator +(Vector one, Vector two)
        {
            one.X += two.X;
            one.Y += two.Y;
            return one;
        }

        public static Vector operator -(Vector one, Vector two)
        {
            one.X -= two.X;
            one.Y -= two.Y;
            return one;
        }

        public float DotProduct(Vector other)
        {
            return X * other.X + Y * other.Y;
        }

        public static readonly Vector Zero = new Vector(0, 0);
    }
}

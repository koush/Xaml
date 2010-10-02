using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct Point
    {
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

        public float Z
        {
            get;
            set;
        }

        public Point(float x, float y)
            : this()
        {
            X = x;
            Y = y;
        }

        public Point(float x, float y, float z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static readonly Point Empty = new Point(0, 0);
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct Thickness
    {
        public static readonly Thickness Empty = new Thickness();

        public float Left
        {
            get;
            set;
        }

        public float Right
        {
            get;
            set;
        }

        public float Top
        {
            get;
            set;
        }

        public float Bottom
        {
            get;
            set;
        }

        internal float Width
        {
            get
            {
                return Left + Right;
            }
        }

        internal float Height
        {
            get
            {
                return Top + Bottom;
            }
        }

        public Thickness(float left, float top, float right, float bottom)
            : this()
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}

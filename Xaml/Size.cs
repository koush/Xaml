using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct Size
    {
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

        public Size(float width, float height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public bool IsEmpty
        {
            get
            {
                return this == Empty;
            }
        }

        public static bool operator !=(Size first, Size second)
        {
            return first.Width != second.Width || first.Height != second.Height;
        }

        public static bool operator ==(Size first, Size second)
        {
            return first.Width == second.Width && first.Height == second.Height;
        }

        public static readonly Size Empty = new Size(0, 0);
    }
}

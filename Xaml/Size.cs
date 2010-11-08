using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public struct Size
    {
        float mWidth;
        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        float mHeight;
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
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
        
        public override string ToString ()
        {
            return string.Format ("[Size: Width={0}, Height={1}, IsEmpty={2}]", Width, Height, IsEmpty);
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

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{
    public class GlyphGeometry : Geometry
    {
        public override int VerticesPerPrimitive
        {
            get
            {
                return 4;
            }
        }

        public override GeometryType GeometryType
        {
            get
            {
                return GeometryType.Triangle;
            }
        }

        internal Rect myBoundingBox;
        public override Rect BoundingBox
        {
            get
            {
                return myBoundingBox;
            }
        }
    }
}

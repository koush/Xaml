using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{

    public class RectangleGeometry : Geometry
    {
        Rect myRect = Rect.Empty;

        public Rect Rect
        {
            get { return myRect; }
            set
            {
                myRect = value;
                SetupPoints(myRect);
            }
        }

        void SetupPoints(Rect rect)
        {
            Point[] points = Points;
            if (points == null)
                points = new Point[4];

            points[0] = rect.BottomLeft;
            points[1] = rect.TopLeft;
            points[2] = rect.TopRight;
            points[3] = rect.BottomRight;

            Points = points;
        }

        public RectangleGeometry()
        {
            SetupPoints(Rect.Empty);
        }

        public RectangleGeometry(Rect rectangle)
        {
            Rect = rectangle;
        }

        internal override int VerticesPerPrimitive
        {
            get
            {
                return 4;
            }
        }

        internal override GeometryType GeometryType
        {
            get
            {
                return GeometryType.TriangleFan;
            }
        }

        internal override Rect BoundingBox
        {
            get
            {
                return myRect;
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace System.Windows.Media
{
    public class GradientStop
    {
        public GradientStop()
        {
        }

        public GradientStop(Color color, float offset)
        {
            Color = color;
            Offset = offset;
        }

        public Color Color
        {
            get;
            set;
        }

        public float Offset
        {
            get;
            set;
        }
    }

    public class GradientBrush : Brush, IComparer<GradientStop>
    {
        public GradientBrush()
        {
            EndPoint = new Point(1, 1);
        }

        List<GradientStop> myStops = new List<GradientStop>();

        public List<GradientStop> GradientStops
        {
            get
            {
                return myStops;
            }
        }

        public Point StartPoint
        {
            get;
            set;
        }

        public Point EndPoint
        {
            get;
            set;
        }



        void VertexColorShader(BrushShader shader, Point[] points, int index, int count, Rect rect)
        {
            myStops.Sort(this);

            Vector diagonal = new Vector(rect.Right - rect.Left, rect.Bottom - rect.Top);
            Point startPoint = new Point(rect.Left + diagonal.X * StartPoint.X, rect.Top + diagonal.Y * StartPoint.Y);
            Point endPoint = new Point(rect.Left + diagonal.X * EndPoint.X, rect.Top + diagonal.Y * EndPoint.Y);

            Vector v = new Vector(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            Vector normalized = v.Normalize();
            float maxDist = v.Length;

            GradientStop search = new GradientStop();
            for (int i = index; i < index + count; i++)
            {
                Point point = points[i];

                Vector v1 = new Vector(point.X - startPoint.X, point.Y - startPoint.Y);
                float pointDist = v1.Length;
                float dp = v1.Normalize().DotProduct(normalized);
                if (dp <= 0)
                {
                    shader.Colors[i] = myStops[0].Color;
                    continue;
                }
                float dist = dp * pointDist;
                if (dist >= maxDist)
                {
                    shader.Colors[i] = myStops[myStops.Count - 1].Color;
                    continue;
                }

                float normalizedDist = dist / maxDist;
                search.Offset = normalizedDist;

                int found = myStops.BinarySearch(search, this);
                if (found < 0)
                    found = -found - 1;
                if (found == 0)
                {
                    shader.Colors[i] = myStops[0].Color;
                    continue;
                }
                if (found == myStops.Count)
                {
                    shader.Colors[i] = myStops[myStops.Count - 1].Color;
                    continue;
                }
                Color startColor = myStops[found - 1].Color;
                Color endColor = myStops[found].Color;
                float colorDist = myStops[found].Offset - myStops[found - 1].Offset;
                shader.Colors[i] = startColor.Blend(endColor, 1 - (normalizedDist - myStops[found - 1].Offset) / colorDist);
            }
        }

        protected override BrushShader GetShader(Geometry geometry)
        {
            BrushShader ret = new BrushShader();
            ret.Colors = new Color[geometry.Points.Length];
            VertexColorShader(ret, geometry.Points, 0, geometry.Points.Length, geometry.BoundingBox);
            return ret;
        }

        #region IComparer<GradientStop> Members

        public int Compare(GradientStop x, GradientStop y)
        {
             return x.Offset.CompareTo(y.Offset);
        }

        #endregion
    }
}

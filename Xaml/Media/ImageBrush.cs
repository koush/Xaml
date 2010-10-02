using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{
    public class ImageBrush : Brush
    {
        public BitmapSource ImageSource
        {
            get;
            set;
        }

        public Stretch Stretch
        {
            get;
            set;
        }

        void TextureCoordinateShader(BrushShader shader, Point[] points, int index, int count, Rect rect)
        {
            float uPerUnit;
            float vPerUnit;

            switch (Stretch)
            {
                case Stretch.None:
                    uPerUnit = 1f / ImageSource.Width;
                    vPerUnit = 1f / ImageSource.Height;
                    break;
                case Stretch.Fill:
                    uPerUnit = 1f / rect.Width;
                    vPerUnit = 1f / rect.Height;
                    break;
                default:
                    {
                        float ratio = (float)ImageSource.Width / (float)ImageSource.Height;
                        Vector vSize = new Vector(ImageSource.Width, ImageSource.Height);
                        Vector hSize = vSize;

                        vSize.X = rect.Width;
                        vSize.Y = rect.Width / ratio;

                        hSize.Y = rect.Height;
                        hSize.X = ratio * hSize.Y;

                        Vector max;
                        Vector min;
                        Vector interest;
                        if (vSize.LengthSquare > hSize.LengthSquare)
                        {
                            max = vSize;
                            min = hSize;
                        }
                        else
                        {
                            min = vSize;
                            max = hSize;
                        }

                        if (Stretch == Stretch.UniformToFill)
                            interest = max;
                        else
                            interest = min;

                        uPerUnit = 1f / interest.X;
                        vPerUnit = 1f / interest.Y;
                    }
                    break;
            }

            for (int i = index; i < index + count; i++)
            {
                Point point = points[i];
                shader.TextureCoordinates[i] = new TextureCoordinate(point.X * uPerUnit, point.Y * vPerUnit);
            }
        }

        protected override BrushShader GetShader(Geometry geometry)
        {
            BrushShader ret = new BrushShader();

            ret.ImageSource = ImageSource;
            ret.TextureCoordinates = new TextureCoordinate[geometry.Points.Length];

            if (Target == BrushTarget.Geometry)
            {
                TextureCoordinateShader(ret, geometry.Points, 0, geometry.Points.Length, geometry.BoundingBox);
            }
            else
            {
                for (int i = 0; i < geometry.Points.Length; i += geometry.VerticesPerPrimitive)
                {
                    TextureCoordinateShader(ret, geometry.Points, i, geometry.VerticesPerPrimitive, Rect.GetBoundingBox(geometry.Points, i, geometry.VerticesPerPrimitive));
                }
            }

            return ret;
        }
    }
}

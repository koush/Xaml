using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Image : Control
    {
        BitmapSource mySource;

        public BitmapSource ImageSource
        {
            get
            {
                return mySource;
            }
            set
            {
                mySource = value;
                InvalidateMeasure();
            }
        }

        Stretch myStretch = Stretch.None;
        public Stretch Stretch
        {
            get
            {
                return myStretch;
            }
            set
            {
                myStretch = value;
                InvalidateMeasure();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect rect;

            if (Stretch == Stretch.Fill || Stretch == Stretch.UniformToFill)
                rect = new Rect(0, 0, ActualWidth, ActualHeight);
            else if (Stretch == Stretch.None)
                rect = new Rect(0, 0, Math.Min(ActualWidth, mySource.Width), Math.Min(ActualHeight, mySource.Height));
            else
            {

                float ratio = (float)mySource.Width / (float)mySource.Height;
                Size vSize = new Size(mySource.Width, mySource.Height);
                Size hSize = vSize;

                vSize.Width = ActualWidth;
                vSize.Height = vSize.Width / ratio;

                hSize.Height = ActualHeight;
                hSize.Width = ratio * hSize.Height;

                Size min;
                if (vSize.Width > hSize.Width)
                    min = hSize;
                else
                    min = vSize;

                rect = new Rect(0, 0, min.Width, min.Height);
            }

            float xOffset = 0;
            float yOffset = 0;
            if (rect.Width < ActualWidth)
                xOffset = (ActualWidth - rect.Width) / 2;
            if (rect.Height < ActualHeight)
                yOffset = (ActualHeight - rect.Height) / 2;

            if (xOffset != 0 || yOffset != 0)
                drawingContext.PushTranslate(xOffset, yOffset);

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = ImageSource;
            brush.Stretch = Stretch;
            drawingContext.DrawGeometry(brush, null, new RectangleGeometry(rect));

            if (xOffset != 0 || yOffset != 0)
                drawingContext.Pop();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (constraint == Size.Empty || mySource == null)
                return Size.Empty;

            // don't bother with anything else if both dimensions are infinite
            if (float.IsPositiveInfinity(constraint.Width) && float.IsPositiveInfinity(constraint.Height))
                return new Size(mySource.Width, mySource.Height);
            
            if (myStretch == Stretch.None || myStretch == Stretch.Fill)
                return new Size(mySource.Width, mySource.Height);

            float ratio = (float)mySource.Width / (float)mySource.Height;
            Size vSize = new Size(mySource.Width, mySource.Height);
            Size hSize = vSize;

            if (!float.IsPositiveInfinity(constraint.Width))
            {
                vSize.Width = constraint.Width;
                vSize.Height = vSize.Width / ratio;
            }

            if (!float.IsPositiveInfinity(constraint.Height))
            {
                hSize.Height = constraint.Height;
                hSize.Width = ratio * hSize.Height;
            }

            Size max;
            Size min;
            if (vSize.Width > hSize.Width)
            {
                max = vSize;
                min = hSize;
            }
            else
            {
                min = vSize;
                max = hSize;
            }

            if (myStretch == Stretch.Uniform)
                return min;
            return max;
        }
    }
}

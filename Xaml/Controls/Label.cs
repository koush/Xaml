using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using SystemColors = System.Drawing.SystemColors;

namespace System.Windows.Controls
{
    public partial class Label : ContentControl
    {
        GlyphRun myRun;

        protected override Size MeasureOverride(Size constraint)
        {
            if (Content == null)
                return Size.Empty;

            if (myRun == null)
                myRun = new GlyphRun(Content.ToString(), myFont, myTextAlignment);

            return new Size(myRun.Width, myRun.Height);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (myRun == null)
                return;
            Point offset = Point.Empty;
            float slack = Width - myRun.Width;
            if (myTextAlignment == TextAlignment.Right)
                offset.X = slack;
            else if (myTextAlignment == TextAlignment.Center)
                offset.X = slack / 2;
            drawingContext.PushTranslate(offset.X, offset.Y);
            drawingContext.DrawGlyphRun(ForeBrush, myRun);
            drawingContext.Pop();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            myRun = null;
            InvalidateMeasure();
        }
    }
}

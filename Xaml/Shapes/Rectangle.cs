using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.Windows.Shapes
{
    public class Rectangle : Control
    {
        Brush myFill;
        public Brush Fill
        {
            get
            {
                return myFill;
            }
            set
            {
                myFill = value;
            }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            if (myFill != null)
                drawingContext.DrawRectangle(myFill, null, new Rect(0, 0, ActualWidth, ActualHeight));
        }
    }
}

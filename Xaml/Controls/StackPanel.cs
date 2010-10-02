using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Controls
{
    public partial class StackPanel : Panel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            // get an accessor depending on the orientation
            OrientationAbstraction orientationHandler;
            if (myOrientation == Orientation.Vertical)
                orientationHandler = VerticalAccessor.Instance;
            else
                orientationHandler = HorizontalAccessor.Instance;

            // adjust the contraint for the children given the orientation
            Size adjustedConstraint = orientationHandler.SetPrimarySize(constraint, float.PositiveInfinity);

            float curOffset = 0;
            float secondaryMax = 0;
            foreach (Control control in Children)
            {
                if (control.Visibility == Visibility.Collapsed)
                    continue;

                Size actualSize = control.Measure(adjustedConstraint);
                curOffset += orientationHandler.GetPrimarySize(actualSize);
                secondaryMax = Math.Max(orientationHandler.GetSecondarySize(actualSize), secondaryMax);
            }

            // if the stack extends beyond its constraint, limit it to the constraint
            curOffset = Math.Min(curOffset, orientationHandler.GetPrimarySize(constraint));
            Size retSize = orientationHandler.GetSize(curOffset, secondaryMax);

            float primarySize = orientationHandler.GetPrimarySize(retSize);
            float secondarySize = orientationHandler.GetSecondarySize(retSize);

            // now lets arrange the stuff!
            curOffset = 0;
            foreach (Control control in Children)
            {
                if (control.Visibility == Visibility.Collapsed)
                    continue;

                Size actualSize = new Size(control.ActualWidth, control.ActualHeight);
                Rect layoutRect = new Rect();
                layoutRect.Size = orientationHandler.SetSecondarySize(actualSize, secondarySize);
                layoutRect = orientationHandler.SetPrimaryLocation(layoutRect, curOffset);
                control.Arrange(layoutRect);

                curOffset += orientationHandler.GetPrimarySize(actualSize);
            }

            return retSize;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Controls
{
    public partial class WrapPanel : Panel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            // get an accessor depending on the orientation
            OrientationAbstraction orientationHandler;
            if (myOrientation == Orientation.Vertical)
                orientationHandler = VerticalAccessor.Instance;
            else
                orientationHandler = HorizontalAccessor.Instance;

            float itemWidth = ItemWidth;
            float itemHeight = ItemHeight;
            Size itemSize = new Size(itemWidth, itemHeight);
            float primItemLen = orientationHandler.GetPrimarySize(itemSize);
            float secItemLen = orientationHandler.GetSecondarySize(itemSize);

            float primLen = orientationHandler.GetPrimarySize(constraint);
            float primLeft = primLen;
            float secLeft = orientationHandler.GetSecondarySize(constraint);

            if (float.IsNaN(primItemLen))
                primItemLen = primLen;

            float maxPrimLen = 0;
            float maxSecLen = 0;
            float curSecOffset = 0;
            float curPrimOffset = 0;

            foreach (var control in Children)
            {
                if (control.Visibility == Visibility.Collapsed)
                    continue;

                Size cellConstraint = new Size();
                if (!float.IsNaN(secItemLen))
                    cellConstraint = orientationHandler.SetSecondarySize(cellConstraint, secItemLen);
                else
                    cellConstraint = orientationHandler.SetSecondarySize(cellConstraint, secLeft);
                cellConstraint = orientationHandler.SetPrimarySize(cellConstraint, primItemLen);

                Size actualSize = control.Measure(cellConstraint);
                float controlPrimLen = orientationHandler.GetPrimarySize(actualSize);
                float controlSecLen = orientationHandler.GetSecondarySize(actualSize);

                if (controlPrimLen > primLeft)
                {
                    // record this max for the final return
                    maxPrimLen = Math.Max(maxPrimLen, curPrimOffset);

                    // reset
                    primLeft = primLen;
                    secLeft -= maxSecLen;
                    curSecOffset += maxSecLen;
                    maxSecLen = 0;
                    curPrimOffset = 0;
                }

                Rect layoutRect = new Rect();
                layoutRect.Size = actualSize;
                if (!float.IsNaN(itemWidth))
                    layoutRect.Size = new Size(itemWidth, layoutRect.Size.Height);
                if (!float.IsNaN(itemHeight))
                    layoutRect.Size = new Size(layoutRect.Width, itemHeight);
                layoutRect = orientationHandler.SetPrimaryLocation(layoutRect, curPrimOffset);
                layoutRect = orientationHandler.SetSecondaryLocation(layoutRect, curSecOffset);
                control.Arrange(layoutRect);

                curPrimOffset += controlPrimLen;
                primLeft -= controlPrimLen;
                maxSecLen = Math.Max(maxSecLen, controlSecLen);
            }

            Size ret = new Size();
            ret = orientationHandler.SetPrimarySize(ret, maxPrimLen);
            ret = orientationHandler.SetSecondarySize(ret, curSecOffset + maxSecLen);
            return ret;
        }
    }
}


using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel;

namespace System.Windows.Controls
{
    public partial class Control : DependencyObject
    {
        internal ControlCollection myChildren;

        protected void InitializeChildren()
        {
            if (myChildren == null)
                myChildren = new ControlCollection(this);
        }

        internal Control myParent;
        public Control Parent
        {
            get
            {
                return myParent;
            }
        }

        static void VisibilityChanged(DependencyObject dependencyObject, Visibility oldValue, Visibility newValue)
        {
            Control control = dependencyObject as Control;
            if (oldValue == Visibility.Hidden && newValue == Visibility.Visible)
                control.InvalidateVisual();
            else if (newValue == Visibility.Hidden && oldValue == Visibility.Visible)
                control.InvalidateVisual();
            else
                control.InvalidateMeasure();
        }

        public void InvalidateVisual()
        {
            myIsVisualValid = false;
            Control control = this;
            // find the window to invalidate it
            while (control != null)
            {
                /*
                Window window = control as Window;
                if (window != null)
                {
                    window.myForm.Invalidate();
                    break;
                }
                */
                control = control.Parent;
            }
        }

        bool myIsVisualValid = false;
        public bool IsVisualValid
        {
            get
            {
                return myIsVisualValid;
            }
        }

        bool myIsMeasureValid = false;
        public bool IsMeasureValid
        {
            get
            {
                return myIsMeasureValid;
            }
        }

        Size myDesiredSize;
        /// <summary>
        /// Desired size is ActualWidth/Height + Margin. The handling of the margin
        /// is done completely internally. Controls need not worry about it in measurement
        /// or layout calculations.
        /// </summary>
        public Size DesiredSize
        {
            get
            {
                return myDesiredSize;
            }
        }

        float myLeft;
        float myTop;

        float myActualWidth = float.NaN;
        public float ActualWidth
        {
            get
            {
                return myActualWidth;
            }
        }
        float myActualHeight = float.NaN;
        public float ActualHeight
        {
            get
            {
                return myActualHeight;
            }
        }

        public void InvalidateMeasure()
        {
            Control last = null;
            Control control = this;
            // invalidate all measures in the tree until 
            while (control != null && control.IsMeasureValid)
            {
                control.myIsMeasureValid = false;
                last = control;
                control = control.Parent;
            }

            /*
            Window window = last as Window;
            if (window != null)
                window.myForm.Invalidate();
            */
        }

        Size myLastConstraint = Size.Empty;
        public Size Measure(Size availableSize)
        {
            // only measure if the measure is invalid or if measure constraint has changed
            // from the previously passed value. The measure constraint changing may be a result of another 
            // Control resizing, and possibly changing the available space left in a Panel.
            if (!IsMeasureValid || myLastConstraint != availableSize)
            {
                Size adjustedSize = availableSize;
                // adjust the constraint size to account for the space the margin would use
                // or restrict it to the user defined width/height
                float width = Width;
                float height = Height;
                bool widthIsNaN = float.IsNaN(width);
                bool heightIsNan = float.IsNaN(height);
                if (!widthIsNaN)
                    adjustedSize.Width = Math.Min(width, adjustedSize.Width);
                if (!heightIsNan)
                    adjustedSize.Height = Math.Min(height, adjustedSize.Height);
                // desired size may be an invalid value, but there's nothing we can do about that
                // we'll fix up the actual size so measurements are not effected
                myDesiredSize = MeasureOverride(adjustedSize);
                System.Diagnostics.Debug.Assert(myDesiredSize.Width <= adjustedSize.Width && myDesiredSize.Height <= adjustedSize.Height);
                Thickness margin = myMargin;
                myDesiredSize.Width += margin.Width;
                myDesiredSize.Height += margin.Height;

                myActualWidth = myDesiredSize.Width;
                myActualHeight = myDesiredSize.Height;
                // put the actual size into a valid range and stretch it if need be
                if (widthIsNaN && !float.IsPositiveInfinity(availableSize.Width) && myHorizontalAlignment == HorizontalAlignment.Stretch)
                    myActualWidth = availableSize.Width;
                else
                {
                    myActualWidth = Math.Max(myActualWidth, 0);
                    myActualWidth = Math.Min(myActualWidth, availableSize.Width);
                }
                if (heightIsNan && !float.IsPositiveInfinity(availableSize.Height) && myVerticalAlignment == VerticalAlignment.Stretch)
                    myActualHeight = availableSize.Height;
                else
                {
                    myActualHeight = Math.Min(myActualHeight, availableSize.Height);
                    myActualHeight = Math.Max(myActualHeight, 0);
                }
                myLastConstraint = availableSize;
            }

            myIsMeasureValid = true;
            return new Size(myActualWidth, myActualHeight);
        }

        protected virtual Size MeasureOverride(Size constraint)
        {
            Size ret = constraint;
            if (float.IsPositiveInfinity(ret.Width))
                ret.Width = 0;
            if (float.IsNegativeInfinity(ret.Height))
                ret.Height = 0;


            if (myChildren != null)
            {
                foreach (Control control in myChildren)
                {
                    Size childSize = control.Measure(constraint);
                    ret.Width = Math.Max(ret.Width, childSize.Width);
                    ret.Height = Math.Max(ret.Height, childSize.Height);
                }

                Rect rect = new Rect(0, 0, ret.Width, ret.Height);
                foreach (Control control in myChildren)
                {
                    control.Arrange(rect);
                }
            }

            return ret;
        }

        public void Arrange(Rect rect)
        {
            // determine the location of the outer edges given the alignments
            switch (myHorizontalAlignment.Value)
            {
                case HorizontalAlignment.Left:
                    myLeft = rect.Left;
                    break;
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    myLeft = (rect.Left + rect.Right - myActualWidth) / 2;
                    break;
                case HorizontalAlignment.Right:
                    myLeft = rect.Right - myActualWidth;
                    break;
            }
            switch (myVerticalAlignment.Value)
            {
                case VerticalAlignment.Top:
                    myTop = rect.Top;
                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    myTop = (rect.Top + rect.Bottom - myActualHeight) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    myTop = rect.Bottom - myActualHeight;
                    break;
            }

            // now adjust the left and the right with the padding
            Thickness margin = myMargin;
            myLeft += margin.Left;
            myTop += margin.Top;
        }

        internal void AddChild(Control child)
        {
            child.myParent = this;
            myChildren.Add(child);
            InvalidateMeasure();
        }

        internal void Render(Control control, DrawingContext context)
        {
            OnRender(context);
            myIsVisualValid = true;

            if (myChildren == null)
                return;
            foreach (Control child in myChildren)
            {
                context.PushTranslate(child.myLeft, child.myTop);
                child.Render(child, context);
                context.Pop();
            }
        }

        protected virtual void OnRender(DrawingContext drawingContext)
        {
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            DependencyPropertyFlags flags = e.Property.Flags;
            if ((flags & DependencyPropertyFlags.AffectsMeasure) == DependencyPropertyFlags.AffectsMeasure)
                InvalidateMeasure();
            else if ((flags & DependencyPropertyFlags.AffectsVisual) == DependencyPropertyFlags.AffectsVisual)
                InvalidateVisual();
            base.OnPropertyChanged(e);
        }
    }
}

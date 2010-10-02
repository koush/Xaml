using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

namespace System.Windows.Controls
{
    partial class Control
    {
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register<float>("Width", float.NaN, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<float> myWidth;
        public float Width
        {
            get
            {
                return myWidth;
            }
            set
            {
                myWidth.Value = value;
            }
        }
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register<float>("Height", float.NaN, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<float> myHeight;
        public float Height
        {
            get
            {
                return myHeight;
            }
            set
            {
                myHeight.Value = value;
            }
        }
        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register<Thickness>("Margin", Thickness.Empty, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<Thickness> myMargin;
        public Thickness Margin
        {
            get
            {
                return myMargin;
            }
            set
            {
                myMargin.Value = value;
            }
        }
        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register<VerticalAlignment>("VerticalAlignment", VerticalAlignment.Stretch, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<VerticalAlignment> myVerticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return myVerticalAlignment;
            }
            set
            {
                myVerticalAlignment.Value = value;
            }
        }
        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register<HorizontalAlignment>("HorizontalAlignment", HorizontalAlignment.Stretch, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<HorizontalAlignment> myHorizontalAlignment;
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return myHorizontalAlignment;
            }
            set
            {
                myHorizontalAlignment.Value = value;
            }
        }
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register<Visibility>("Visibility", Visibility.Visible, false, DependencyPropertyFlags.AffectsNothing, new DependencyPropertyChangedHandler<Visibility>(VisibilityChanged));
        DependencyPropertyStorage<Visibility> myVisibility;
        public Visibility Visibility
        {
            get
            {
                return myVisibility;
            }
            set
            {
                myVisibility.Value = value;
            }
        }
        protected override void Initialize()
        {
            base.Initialize();
            myWidth = GetStorage<float>(WidthProperty);
            myHeight = GetStorage<float>(HeightProperty);
            myMargin = GetStorage<Thickness>(MarginProperty);
            myVerticalAlignment = GetStorage<VerticalAlignment>(VerticalAlignmentProperty);
            myHorizontalAlignment = GetStorage<HorizontalAlignment>(HorizontalAlignmentProperty);
            myVisibility = GetStorage<Visibility>(VisibilityProperty);
        }
    }
    partial class StackPanel
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register<Orientation>("Orientation", Orientation.Vertical, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<Orientation> myOrientation;
        public Orientation Orientation
        {
            get
            {
                return myOrientation;
            }
            set
            {
                myOrientation.Value = value;
            }
        }
        protected override void Initialize()
        {
            base.Initialize();
            myOrientation = GetStorage<Orientation>(OrientationProperty);
        }
    }
    partial class Label
    {
        public static readonly DependencyProperty FontProperty = DependencyProperty.Register<Font>("Font", Font.Default, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<Font> myFont;
        public Font Font
        {
            get
            {
                return myFont;
            }
            set
            {
                myFont.Value = value;
            }
        }
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register<TextAlignment>("TextAlignment", TextAlignment.Left, false, DependencyPropertyFlags.AffectsMeasure, null);
        DependencyPropertyStorage<TextAlignment> myTextAlignment;
        public TextAlignment TextAlignment
        {
            get
            {
                return myTextAlignment;
            }
            set
            {
                myTextAlignment.Value = value;
            }
        }
        public static readonly DependencyProperty ForeBrushProperty = DependencyProperty.Register<Brush>("ForeBrush", new SolidColorBrush(new Color(System.Drawing.SystemColors.ControlText.R, System.Drawing.SystemColors.ControlText.G, System.Drawing.SystemColors.ControlText.B, System.Drawing.SystemColors.ControlText.A)), false, DependencyPropertyFlags.AffectsVisual, null);
        DependencyPropertyStorage<Brush> myForeBrush;
        public Brush ForeBrush
        {
            get
            {
                return myForeBrush;
            }
            set
            {
                myForeBrush.Value = value;
            }
        }
        protected override void Initialize()
        {
            base.Initialize();
            myFont = GetStorage<Font>(FontProperty);
            myTextAlignment = GetStorage<TextAlignment>(TextAlignmentProperty);
            myForeBrush = GetStorage<Brush>(ForeBrushProperty);
        }
    }
}

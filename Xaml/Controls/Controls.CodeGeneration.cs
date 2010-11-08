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
 partial class WrapPanel
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
     public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register<float>("ItemWidth", float.NaN, false, DependencyPropertyFlags.AffectsMeasure, null);
     DependencyPropertyStorage<float> myItemWidth;
     public float ItemWidth
     {
         get
         {
             return myItemWidth;
         }
         set
         {
             myItemWidth.Value = value;
         }
     }
     public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register<float>("ItemHeight", float.NaN, false, DependencyPropertyFlags.AffectsMeasure, null);
     DependencyPropertyStorage<float> myItemHeight;
     public float ItemHeight
     {
         get
         {
             return myItemHeight;
         }
         set
         {
             myItemHeight.Value = value;
         }
     }
     protected override void Initialize()
     {
         base.Initialize();
         myOrientation = GetStorage<Orientation>(OrientationProperty);
         myItemWidth = GetStorage<float>(ItemWidthProperty);
         myItemHeight = GetStorage<float>(ItemHeightProperty);
     }
 }
}



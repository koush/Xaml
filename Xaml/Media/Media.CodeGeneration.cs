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

namespace System.Windows.Media
{
    partial class Brush
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register<BrushTarget>("Target", BrushTarget.Geometry, false, DependencyPropertyFlags.AffectsVisual, null);
        DependencyPropertyStorage<BrushTarget> myTarget;
        public BrushTarget Target
        {
            get
            {
                return myTarget;
            }
            set
            {
                myTarget.Value = value;
            }
        }
        protected override void Initialize()
        {
            base.Initialize();
            myTarget = GetStorage<BrushTarget>(TargetProperty);
        }
    }
    partial class SolidColorBrush
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register<Color>("Color", new Color(255, 255, 255, 255), false, DependencyPropertyFlags.AffectsVisual, null);
        DependencyPropertyStorage<Color> myColor;
        public Color Color
        {
            get
            {
                return myColor;
            }
            set
            {
                myColor.Value = value;
            }
        }
        protected override void Initialize()
        {
            base.Initialize();
            myColor = GetStorage<Color>(ColorProperty);
        }
    }
}

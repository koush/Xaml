using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{
    public partial class SolidColorBrush : Brush
    {
        public SolidColorBrush()
        {
        }

        public SolidColorBrush(Color color)
        {
            myColor.Value = color;
        }

        protected override BrushShader GetShader(Geometry geometry)
        {
            BrushShader ret = new BrushShader();
            ret.Color = myColor;
            return ret;
        }
    }
}

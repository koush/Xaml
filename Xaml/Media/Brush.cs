using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{
    public class BrushShader
    {
        public Color[] Colors
        {
            get;
            set;
        }

        public TextureCoordinate[] TextureCoordinates
        {
            get;
            set;
        }

        public BitmapSource ImageSource
        {
            get;
            set;
        }

        public Color? Color
        {
            get;
            set;
        }
    }

    public struct TextureCoordinate
    {
        public TextureCoordinate(float u, float v)
            : this()
        {
            U = u;
            V = v;
        }

        public float U
        {
            get;
            set;
        }
        public float V
        {
            get;
            set;
        }
    }

    public enum BrushTarget
    {
        Geometry,
        Primitive,
    }

    public abstract partial class Brush : DependencyObject
    {
        internal BrushShader GetShaderInternal(Geometry geometry)
        {
            return GetShader(geometry);
        }
        protected abstract BrushShader GetShader(Geometry geometry);
    }
}

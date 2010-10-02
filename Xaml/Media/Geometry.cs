using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using OpenGLES;

namespace System.Windows.Media
{
    public enum GeometryType : uint
    {
        TriangleFan = gl.GL_TRIANGLE_FAN,
        TriangleStrip = gl.GL_TRIANGLE_STRIP,
        Triangle = gl.GL_TRIANGLES,
    }

    public abstract class Geometry
    {
        public Point[] Points
        {
            get;
            set;
        }

        public short[] Indices
        {
            get;
            set;
        }

        public abstract int VerticesPerPrimitive
        {
            get;
        }

        public abstract GeometryType GeometryType
        {
            get;
        }

        public abstract Rect BoundingBox
        {
            get;
        }
    }
}

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
        internal Point[] Points
        {
            get;
            set;
        }

        internal short[] Indices
        {
            get;
            set;
        }

        internal abstract int VerticesPerPrimitive
        {
            get;
        }

        internal abstract GeometryType GeometryType
        {
            get;
        }

        internal abstract Rect BoundingBox
        {
            get;
        }
    }
}

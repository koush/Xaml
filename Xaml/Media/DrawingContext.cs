using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenGLES;

namespace System.Windows.Media
{
    /// <summary>
    /// A DrawingContext builds up a collection of DrawingOperations that can be used to render
    /// the tree at a later time. (Retained mode)
    /// </summary>
    public class DrawingContext
    {
        unsafe public void DrawGlyphRun(Brush brush, GlyphRun run)
        {
            gl.PushMatrix();
            Geometry geo = run.GetGeometry();

            fixed (TextureCoordinate* texCoords = run.FontCoords)
            {
                gl.Enable(gl.GL_BLEND);
                gl.ActiveTexture(gl.GL_TEXTURE1);
                gl.ClientActiveTexture(gl.GL_TEXTURE1);
                gl.EnableClientState(gl.GL_TEXTURE_COORD_ARRAY);
                gl.Enable(gl.GL_TEXTURE_2D);
                gl.BindTexture(gl.GL_TEXTURE_2D, run.Font.mySource.myName);
                gl.TexCoordPointer(2, gl.GL_FLOAT, 0, (IntPtr)texCoords);
                gl.ActiveTexture(gl.GL_TEXTURE0);
                gl.ClientActiveTexture(gl.GL_TEXTURE0);

                DrawGeometry(brush, null, geo);

                gl.ActiveTexture(gl.GL_TEXTURE1);
                gl.Disable(gl.GL_TEXTURE_2D);
                gl.ActiveTexture(gl.GL_TEXTURE0);
                gl.Disable(gl.GL_BLEND);
            }

            gl.PopMatrix();
        }

        public void Pop()
        {
            gl.PopMatrix();
        }

        public void PushTranslate(float x, float y)
        {
            gl.PushMatrix();
            gl.Translatef(x, y, 0);
        }

        unsafe public void DrawGeometry(Brush brush, Pen pen, Geometry geometry)
        {
            BrushShader shader = brush.GetShaderInternal(geometry);

            gl.EnableClientState(gl.GL_VERTEX_ARRAY);

            fixed (Color* colors = shader.Colors)
            {
                fixed (TextureCoordinate* texCoords = shader.TextureCoordinates)
                {
                    if (colors != null)
                    {
                        gl.EnableClientState(gl.GL_COLOR_ARRAY);
                        gl.ColorPointer(4, gl.GL_FLOAT, 0, (IntPtr)colors);
                    }

                    if (texCoords != null && shader.ImageSource != null)
                    {
                        gl.EnableClientState(gl.GL_TEXTURE_COORD_ARRAY);
                        gl.Enable(gl.GL_TEXTURE_2D);
                        gl.BindTexture(gl.GL_TEXTURE_2D, shader.ImageSource.myName);
                        gl.TexCoordPointer(2, gl.GL_FLOAT, 0, (IntPtr)texCoords);
                    }

                    if (shader.Color != null)
                    {
                        Color color = shader.Color.Value;
                        gl.Color4f(color.ScR, color.ScG, color.ScB, color.ScA);
                    }

                    fixed (short* indices = geometry.Indices)
                    {
                        fixed (Point* points = geometry.Points)
                        {
                            gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)points);
                            if (geometry.Indices != null)
                                gl.DrawElements((uint)geometry.GeometryType, geometry.Indices.Length, gl.GL_UNSIGNED_SHORT, (IntPtr)indices);
                            else
                                gl.DrawArrays((uint)geometry.GeometryType, 0, geometry.Points.Length);
                        }
                    }

                    if (shader.Color != null)
                        gl.Color4f(1, 1, 1, 1);

                    if (texCoords != null && shader.ImageSource != null)
                        gl.Disable(gl.GL_TEXTURE_2D);

                    if (colors != null)
                        gl.DisableClientState(gl.GL_COLOR_ARRAY);
                }
            }


            gl.DisableClientState(gl.GL_VERTEX_ARRAY);
        }

        public void DrawRectangle(Brush brush, Pen pen, Rect rectangle)
        {
            DrawGeometry(brush, pen, new RectangleGeometry(rectangle));
        }

        public void DrawImage(BitmapSource imageSource, Rect rect)
        {
            RectangleGeometry geo = new RectangleGeometry(rect);
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = imageSource;
            brush.Stretch = Stretch.Fill;
            DrawGeometry(brush, null, geo);
        }
    }
}
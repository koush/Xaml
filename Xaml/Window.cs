using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using OpenGLES;
using System.Threading;
using System.Windows.Media;
using android.content;
using System.Windows;
using system.windows;

namespace System.Windows
{
    public partial class Window : ContentControl
    {
        static List<Window> myWindows = new List<Window>();
        
        public Window(WindowActivity activity)
        {
            myWindows.Add(this);
            WindowActivity = activity;

            WindowActivity.myWindow = this;
            SurfaceView = new android.opengl.GLSurfaceView(WindowActivity);
            SurfaceView.setRenderer(WindowActivity);
        }

        public void Show()
        {
            //myForm.Show();
        }

        public void Hide()
        {
            //myForm.Hide();
        }

        protected virtual void OnClosed(EventArgs e)
        {
            myWindows.Remove(this);

            if (Closed != null)
                Closed(this, e);
        }

        Color myBackBrush = new Color(255, 255, 255, 255);
            
        public Color BackBrush
        {
            get
            {
                return myBackBrush;
            }
            set
            {
                gl.ClearColor(BackBrush.ScR, BackBrush.ScG, BackBrush.ScB, BackBrush.ScA);
                myBackBrush = value;
            }
        }

        DrawingContext myDrawingContext = new DrawingContext();

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            gl.Clear(gl.GL_COLOR_BUFFER_BIT);
        }

        internal void OnPaint()
        {
            if (!IsMeasureValid)
            {
                //Control content = Content as Control;
                //if (content != null && !content.IsMeasureValid)
                //{
                //    content.Measure(new Size(myForm.ClientSize.Width, myForm.ClientSize.Height));
                //    content.Arrange(new Rect(0, 0, myForm.ClientSize.Width, myForm.ClientSize.Height));
                //}
                Measure(new Size(ClientWidth, ClientHeight));
            }

            gl.Clear(gl.GL_COLOR_BUFFER_BIT);

            gl.MatrixMode(gl.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Orthof(0, ClientWidth, ClientHeight, 0, -10, 10);

            gl.MatrixMode(gl.GL_MODELVIEW);
            gl.LoadIdentity();

            Render(this, myDrawingContext);
        }

        public event EventHandler Closed;
    }
}

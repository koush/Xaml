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
using System.Windows.Threading;

namespace System.Windows
{
    public partial class Window : ContentControl
    {
        static List<Window> myWindows = new List<Window>();
        internal static Dispatcher myGLDispatcher;
  
        public Window(WindowActivity activity)
        {
            WindowActivity = activity;
        }
        
        protected virtual void OnCreate()
        {
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
                 Measure(new Size(WindowActivity.ClientWidth, WindowActivity.ClientHeight));
            }

            gl.ClearColor(BackBrush.ScR, BackBrush.ScG, BackBrush.ScB, BackBrush.ScA);
            gl.ShadeModel(gl.GL_SMOOTH);
            gl.ClearDepthf(1.0f);
            gl.BlendFunc(gl.GL_SRC_ALPHA, gl.GL_ONE_MINUS_SRC_ALPHA);
            gl.DepthFunc(gl.GL_LEQUAL);
            gl.Hint(gl.GL_PERSPECTIVE_CORRECTION_HINT, gl.GL_NICEST);

            gl.Clear(gl.GL_COLOR_BUFFER_BIT);

            gl.MatrixMode(gl.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Orthof(0, WindowActivity.ClientWidth, WindowActivity.ClientHeight, 0, -10, 10);

            gl.MatrixMode(gl.GL_MODELVIEW);
            gl.LoadIdentity();

            Render(this, myDrawingContext);
        }

        public event EventHandler Closed;
    }
}

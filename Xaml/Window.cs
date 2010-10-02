using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using OpenGLES;
using System.Threading;
using System.Windows.Media;
using SystemColors = System.Drawing.SystemColors;

namespace System.Windows
{
    public partial class Window : ContentControl
    {
        static List<Window> myWindows = new List<Window>();
        internal WindowForm myForm = new WindowForm();
        EGLDisplay myDisplay;
        EGLSurface mySurface;
        EGLContext myContext;

        public Window()
        {
            myWindows.Add(this);
            myForm.myWindow = this;


            myDisplay = egl.GetDisplay(new EGLNativeDisplayType(myForm));

            int major, minor;
            egl.Initialize(myDisplay, out major, out minor);

            EGLConfig[] configs = new EGLConfig[10];
            int[] attribList = new int[] 
            { 
                egl.EGL_RED_SIZE, 5, 
                egl.EGL_GREEN_SIZE, 6, 
                egl.EGL_BLUE_SIZE, 5, 
                egl.EGL_DEPTH_SIZE, 16 , 
                egl.EGL_SURFACE_TYPE, egl.EGL_WINDOW_BIT,
                egl.EGL_STENCIL_SIZE, egl.EGL_DONT_CARE,
                egl.EGL_NONE, egl.EGL_NONE 
            };

            int numConfig;
            if (!egl.ChooseConfig(myDisplay, attribList, configs, configs.Length, out numConfig) || numConfig < 1)
                throw new InvalidOperationException("Unable to choose config.");

            EGLConfig config = configs[0];
            mySurface = egl.CreateWindowSurface(myDisplay, config, myForm.Handle, null);
            myContext = egl.CreateContext(myDisplay, config, EGLContext.None, null);

            egl.MakeCurrent(myDisplay, mySurface, mySurface, myContext);

            gl.ClearColor(BackBrush.ScR, BackBrush.ScG, BackBrush.ScB, BackBrush.ScA);
            gl.ShadeModel(gl.GL_SMOOTH);
            gl.ClearDepthf(1.0f);
            gl.BlendFunc(gl.GL_SRC_ALPHA, gl.GL_ONE_MINUS_SRC_ALPHA);
            gl.DepthFunc(gl.GL_LEQUAL);
            gl.Hint(gl.GL_PERSPECTIVE_CORRECTION_HINT, gl.GL_NICEST);
        }

        public void Show()
        {
            myForm.Show();
        }

        public void Hide()
        {
            myForm.Hide();
        }

        protected virtual void OnClosed(EventArgs e)
        {
            if (!egl.DestroySurface(myDisplay, mySurface))
                throw new Exception("Error while destroying surface.");
            if (!egl.DestroyContext(myDisplay, myContext))
                throw new Exception("Error while destroying context.");
            if (!egl.Terminate(myDisplay))
                throw new Exception("Error while terminating display.");

            myWindows.Remove(this);

            if (Closed != null)
                Closed(this, e);
        }

        Color myBackBrush = new Color(SystemColors.Window.R, SystemColors.Window.G, SystemColors.Window.B, SystemColors.Window.A);
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

        public event EventHandler Closed;
    }
}

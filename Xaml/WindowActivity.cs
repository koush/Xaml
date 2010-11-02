using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using OpenGLES;
using System.Threading;
using System.Windows.Media;
using android.opengl;
using System.Windows;
using system.windows;
using System.Windows.Threading;

namespace system.windows
{
    public class WindowActivity : android.app.Activity, GLSurfaceView.Renderer
    {
        internal Window myWindow;
        protected WindowActivity (MonoJavaBridge.JNIEnv env) : base(env)
        {
        }
        
        public void onSurfaceCreated (javax.microedition.khronos.opengles.GL10 arg0, javax.microedition.khronos.egl.EGLConfig arg1)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            Dispatcher.CreateDispatcherForGLThread(myWindow.SurfaceView);
            gl.ClearColor(myWindow.BackBrush.ScR, myWindow.BackBrush.ScG, myWindow.BackBrush.ScB, myWindow.BackBrush.ScA);
            gl.ShadeModel(gl.GL_SMOOTH);
            gl.ClearDepthf(1.0f);
            gl.BlendFunc(gl.GL_SRC_ALPHA, gl.GL_ONE_MINUS_SRC_ALPHA);
            gl.DepthFunc(gl.GL_LEQUAL);
            gl.Hint(gl.GL_PERSPECTIVE_CORRECTION_HINT, gl.GL_NICEST);
        }

        public void onSurfaceChanged (javax.microedition.khronos.opengles.GL10 arg0, int arg1, int arg2)
        {
            myWindow.ClientWidth = arg1;
            myWindow.ClientHeight = arg2;
        }

        public void onDrawFrame (javax.microedition.khronos.opengles.GL10 arg0)
        {
            myWindow.OnPaint();
        }
    }
}

namespace System.Windows
{
    public partial class Window
    {
        public GLSurfaceView SurfaceView
        {
            get;set;
        }
        
        internal int ClientWidth
        {
            get;set;
        }
        internal int ClientHeight
        {
            get;set;
        }
        
        public WindowActivity WindowActivity
        {
            get;set;
        }
    }
}


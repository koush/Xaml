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
        Dispatcher myDispatcher;
        public Type WindowType
        {
            get;set;
        }
        
        internal Window myWindow;
        protected WindowActivity (MonoJavaBridge.JNIEnv env) : base(env)
        {
        }
        
        GLSurfaceView mySurfaceView;
        protected override void onCreate (android.os.Bundle arg0)
        {
            base.onCreate (arg0);
            
            myDispatcher = Dispatcher.CurrentDispatcher;
            
            mySurfaceView = new GLSurfaceView(this);
            mySurfaceView.setRenderer(this);
            setContentView(mySurfaceView);
        }
        
        public void onSurfaceCreated (javax.microedition.khronos.opengles.GL10 arg0, javax.microedition.khronos.egl.EGLConfig arg1)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            var dispatcher = Dispatcher.CreateCustomDispatcher();
            System.Windows.Window.myGLDispatcher = dispatcher;
            dispatcher.myPostDelegate = d =>
            {
                mySurfaceView.queueEvent(() =>
                {
                    d.Invoke();
                });
            };

            myDispatcher.BeginInvoke(new Action(() =>
            {
                var constructor = WindowType.GetConstructor(new Type[]{ typeof(WindowActivity) });
                var window = constructor.Invoke(new object[] { this }) as Window;
                myWindow = window;
            }));
        }

        public void onSurfaceChanged (javax.microedition.khronos.opengles.GL10 arg0, int arg1, int arg2)
        {
            ClientWidth = arg1;
            ClientHeight = arg2;
        }

        public void onDrawFrame (javax.microedition.khronos.opengles.GL10 arg0)
        {
            if (myWindow == null)
                return;
            myWindow.OnPaint();
        }
        
        internal int ClientWidth
        {
            get;set;
        }
        internal int ClientHeight
        {
            get;set;
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
        
        public WindowActivity WindowActivity
        {
            get;set;
        }
    }
}


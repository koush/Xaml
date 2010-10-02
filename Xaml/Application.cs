using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using OpenGLES;

namespace System.Windows
{
    public class Application : DispatcherObject
    {
        static Application myApplication;

        public Application()
        {
            if (myApplication != null)
                throw new InvalidOperationException("An Application object already exists in this AppDomain.");
            myApplication = this;
        }

        public static Application Current
        {
            get
            {
                return myApplication;
            }
        }

        List<Window> myWindows = new List<Window>();
        DispatcherFrame myWindowDispatcherFrame = new DispatcherFrame();

        void HookWindow(Window window)
        {
            myWindows.Add(window);
            window.Closed += new EventHandler(window_Closed);
        }

        void window_Closed(object sender, EventArgs e)
        {
            myWindows.Remove(sender as Window);
            if (myWindows.Count == 0)
                Dispatcher.InvokeShutdown();
        }

        public void Run(Window window)
        {
            //InitializeOpenGL();

            HookWindow(window);
            window.Show();
            Dispatcher.PushFrame(myWindowDispatcherFrame);
        }

        [DllImport("coredll")]
        extern static IntPtr GetDC(IntPtr hwnd);

        EGLContext ourContext;
        EGLDisplay ourDisplay;
        EGLConfig ourConfig;

        void InitializeOpenGL()
        {
            ourDisplay = egl.GetDisplay(new EGLNativeDisplayType(IntPtr.Zero));

            int major, minor;
            bool ret = egl.Initialize(ourDisplay, out major, out minor);

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
            if (!egl.ChooseConfig(ourDisplay, attribList, configs, configs.Length, out numConfig) || numConfig < 1)
                throw new InvalidOperationException("Unable to choose config.");

            ourConfig = configs[0];
            ourContext = egl.CreateContext(ourDisplay, ourConfig, EGLContext.None, null);
        }
    }
}

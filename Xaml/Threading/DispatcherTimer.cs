using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
//using System.Threading;
using Timer = java.util.Timer;
using system.windows.threading;
using System.Windows.Threading;

namespace system.windows.threading
{
    class DispatcherTimerTask : java.util.TimerTask
    {
        internal static global::MonoJavaBridge.JniGlobalHandle staticClass;
        internal static global::MonoJavaBridge.MethodId constructor;
        internal DispatcherTimerTask(global::MonoJavaBridge.JNIEnv @__env) : base(@__env)
        {
        }
        static DispatcherTimerTask()
        {
            global::MonoJavaBridge.JNIEnv @__env = global::MonoJavaBridge.JNIEnv.ThreadEnv;
            staticClass = @__env.NewGlobalRef(@__env.FindClass("system/windows/threading/DispatcherTimerTask"));
            constructor = @__env.GetMethodIDNoThrow(staticClass, "<init>", "()V");
        }
        public DispatcherTimerTask() : base(global::MonoJavaBridge.JNIEnv.ThreadEnv)
        {
            global::MonoJavaBridge.JNIEnv @__env = global::MonoJavaBridge.JNIEnv.ThreadEnv;
            global::MonoJavaBridge.JniLocalHandle handle = @__env.NewObject(staticClass, constructor);
            Init(@__env, handle);
            MonoJavaBridge.JavaBridge.SetGCHandle(@__env, this);
        }

        DispatcherTimer myTimer;
        public DispatcherTimerTask(DispatcherTimer timer) : this()
        {
            myTimer = timer;
        }

        public override void run()
        {
            myTimer.FireTick();
        }
    }
}

namespace System.Windows.Threading
{
    public class DispatcherTimer
    {
        Timer myTimer = new Timer();
        DispatcherTimerTask myTimerTask;

        TimeSpan myTimeSpan;
        Dispatcher myDispatcher;
        bool myEnabled = true;
        public DispatcherTimer(TimeSpan timeSpan, EventHandler handler, Dispatcher dispatcher)
        {
            myTimeSpan = timeSpan;
            myDispatcher = dispatcher;
            Tick += handler;
            myTimerTask = new DispatcherTimerTask(this);
        }

        void Schedule()
        {
            myTimer.scheduleAtFixedRate(myTimerTask, (long)myTimeSpan.TotalMilliseconds, (long)myTimeSpan.TotalMilliseconds);
        }

        void myTimer_Tick(object sender, EventArgs e)
        {
            myDispatcher.BeginInvoke(new EmptyDelegate(FireTick));
        }
        
        public void Start()
        {
            myEnabled = true;
            Schedule();
        }

        public void Stop()
        {
            myEnabled = false;
            myTimer.cancel();
        }

        ~DispatcherTimer()
        {
            Stop();
            myTimer = null;
        }

        internal void FireTick()
        {
            if (Tick != null)
                Tick(Tag, null);
        }

        public event EventHandler Tick;

        public object Tag
        {
            get;
            set;
        }
    }
}

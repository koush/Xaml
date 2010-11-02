using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using Handler = android.os.Handler;
using Looper = android.os.Looper;
using Message = android.os.Message;
using android.opengl;

namespace System.Windows.Threading
{
    public class Dispatcher
    {
        internal int myDisableProcessingCount = 0;
        Thread myThread = Thread.CurrentThread;

        public Thread Thread
        {
            get { return myThread; }
        }

        internal Action<DispatcherOperation> mySendDelegate;
        internal Action<DispatcherOperation> myPostDelegate;
        internal Dispatcher()
        {
            Handler handler = new Handler();
            myPostDelegate = d => handler.post(() => d.Invoke());
            mySendDelegate = d =>
            {
                Message message = Message.obtain(handler, () =>
                {
                    d.Invoke();
                });
                handler.dispatchMessage(message);
            };
        }

        internal Dispatcher(Action<DispatcherOperation> postDelegate, Action<DispatcherOperation> sendDelegate)
        {
            mySendDelegate = sendDelegate;
            myPostDelegate = postDelegate;
        }

        public static Dispatcher FromThread(Thread thread)
        {
            return ThreadStatic<Dispatcher>.GetThreadStatic(thread);
        }

        public static Dispatcher CurrentDispatcher
        {
            get
            {
                return ThreadStatic<Dispatcher>.GetOrCreateThreadStatic(Create);
            }
        }

        internal static Dispatcher CreateDispatcherForGLThread(GLSurfaceView view)
        {
            var thread = Thread.CurrentThread;
            var ret = ThreadStatic<Dispatcher>.GetThreadStatic(thread);
            if (ret != null)
                throw new InvalidOperationException("Dispatcher has already been created for this thread.");
            ret = ThreadStatic<Dispatcher>.GetOrCreateThreadStatic(() => new Dispatcher(null, null));
            ret.myPostDelegate = d =>
            {
                view.queueEvent(() =>
                {
                    d.Invoke();
                });
            };
            return ret;
        }

        static Dispatcher Create()
        {
            return new Dispatcher();
        }

        public bool CheckAccess()
        {
            return this == FromThread(Thread.CurrentThread);
        }

        public void VerifyAccess()
        {
            if (myThread != Thread.CurrentThread)
                throw new InvalidOperationException("The calling thread does not have access to this Dispatcher.");
        }

        public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
        {
            DispatcherOperation ret = new DispatcherOperation(this, method, args);
            myPostDelegate(ret);
            return ret;
        }

        public object Invoke(Delegate method, params object[] args)
        {
            DispatcherOperation op = BeginInvoke(method, args);
            op.Wait();
            return op.Result;
        }

        public static void Run()
        {
            Dispatcher currentDispatcher = CurrentDispatcher;
            //currentDispatcher.PushFrameInternal(new DispatcherFrame());

        }
    }
}

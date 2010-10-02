using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace System.Windows.Threading
{
    public class Dispatcher
    {
        internal int myDisableProcessingCount = 0;
        Dictionary<DispatcherPriority, List<DispatcherOperation>> myOperations = new Dictionary<DispatcherPriority, List<DispatcherOperation>>();
        Thread myThread = Thread.CurrentThread;

#if PocketPC || Smartphone
        const string MessageDll = "coredll";
#endif

        enum DispatcherMessageParam : int
        {
            Invoke = 5000
        }

        const int DispatcherMessage = 34534;

        [DllImport(MessageDll)]
        extern static int GetMessage(IntPtr msg, IntPtr hwnd, uint filterMax, uint filterMin);

        [DllImport(MessageDll)]
        extern static bool TranslateMessage(IntPtr msg);

        [DllImport(MessageDll)]
        extern static uint DispatchMessage(IntPtr msg);

        [DllImport(MessageDll)]
        extern static bool PostThreadMessage(int threadId, int Msg, int wParam, int lParam);

        bool myHasShutdownStarted = false;

        public bool HasShutdownStarted
        {
            get { return myHasShutdownStarted; }
        }

        bool myHasShutdownFinished = false;

        public bool HasShutdownFinished
        {
            get { return myHasShutdownFinished; }
        }

        void ShutdownHandler()
        {
            myHasShutdownStarted = true;
            if (ShutdownStarted != null)
                ShutdownStarted(this, EventArgs.Empty);
        }

        DispatcherOperation BeginInvokeShutdownInternal(DispatcherPriority priority)
        {
            return BeginInvoke(priority, new EmptyDelegate(ShutdownHandler));
        }

        public void BeginInvokeShutdown(DispatcherPriority priority)
        {
            BeginInvokeShutdownInternal(priority);
        }

        public void InvokeShutdown()
        {
            BeginInvokeShutdownInternal((DispatcherPriority)((int)DispatcherPriority.Send + 1)).Wait();
        }

        void PostDispatcherMessage(DispatcherMessageParam message)
        {
            PostThreadMessage(myThread.ManagedThreadId, DispatcherMessage, (int)message, 0);
        }

        public Thread Thread
        {
            get { return myThread; }
        }

        private Dispatcher()
        {
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

        public DispatcherProcessingDisabled DisableProcessing()
        {
            if (this != FromThread(Thread.CurrentThread))
                throw new InvalidOperationException("The calling thread cannot access this object because a different thread owns it.");
            myDisableProcessingCount++;
            DispatcherProcessingDisabled ret = new DispatcherProcessingDisabled();
            ret.myDispatcher = this;
            return ret;
        }

        DispatcherOperation PopOperation()
        {
            lock (myOperations)
            {
                for (int i = (int)DispatcherPriority.Send + 1; i >= (int)DispatcherPriority.SystemIdle; i--)
                {
                    List<DispatcherOperation> operations;
                    if (!myOperations.TryGetValue((DispatcherPriority)i, out operations) || operations.Count == 0)
                        continue;
                    DispatcherOperation operation = operations[0];
                    operations.RemoveAt(0);
                    return operation;
                }
            }

            return null;
        }

        void PushOperation(DispatcherOperation operation, bool insert)
        {
            if (myHasShutdownStarted || myHasShutdownFinished)
                throw new InvalidOperationException("The Dispatcher has shutdown.");
            lock (myOperations)
            {
                List<DispatcherOperation> operations;
                if (!myOperations.TryGetValue(operation.Priority, out operations))
                {
                    operations = new List<DispatcherOperation>();
                    myOperations.Add(operation.Priority, operations);
                }
                if (insert)
                    operations.Insert(0, operation);
                else
                    operations.Add(operation);
            }
            PostDispatcherMessage(DispatcherMessageParam.Invoke);
        }

        internal bool Abort(DispatcherOperation operation)
        {
            lock (myOperations)
            {
                if (operation.myStatus != DispatcherOperationStatus.Pending)
                    return false;
                operation.myStatus = DispatcherOperationStatus.Aborted;
            }
            return true;
        }

        internal bool SetPriority(DispatcherOperation operation, DispatcherPriority priority)
        {
            lock (myOperations)
            {
                List<DispatcherOperation> operations = myOperations[operation.Priority];
                operations.Remove(operation);

                if (!myOperations.TryGetValue(operation.Priority, out operations))
                {
                    operations = new List<DispatcherOperation>();
                    myOperations.Add(operation.Priority, operations);
                }
                operations.Add(operation);
            }
            return true;
        }

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, params object[] args)
        {
            DispatcherOperation ret = new DispatcherOperation(this, method, priority, args);
            PushOperation(ret, false);
            return ret;
        }

        public object Invoke(DispatcherPriority priority, Delegate method, params object[] args)
        {
            DispatcherOperation op = BeginInvoke(priority, method, args);
            op.Wait();
            return op.Result;
        }

        public static void Run()
        {
            Dispatcher currentDispatcher = CurrentDispatcher;
            currentDispatcher.PushFrameInternal(new DispatcherFrame());
        }

        int myFrameCount = 0;
        internal void PushFrameInternal(DispatcherFrame frame)
        {
            System.Diagnostics.Debug.Assert(frame.Dispatcher == this);
            if (myHasShutdownFinished)
                throw new InvalidOperationException("The Dispatcher has finished shutting down."); 
            if (myDisableProcessingCount > 0)
                throw new InvalidOperationException("Dispatcher processing has been disabled.");

            // reset the exit count if we are starting a new frame stack
            if (myFrameCount == 0)
                myExitAllFrames = false;

            myFrameCount++;
            IntPtr message = Marshal.AllocHGlobal(1000);
            try
            {
                int ret;
                while (!myHasShutdownStarted && frame.Continue && (ret = GetMessage(message, IntPtr.Zero, 0, 0)) != 0)
                {
                    if (ret == -1)
                        break;

                    int wParam = Marshal.ReadInt32(message, 8);
                    if (wParam == (int)DispatcherMessageParam.Invoke)
                    {
                        DispatcherOperation operation = PopOperation();
                        operation.myStatus = DispatcherOperationStatus.Executing;
                        System.Diagnostics.Debug.Assert(operation != null);
                        // TODO: Use PeekMessage and handle background and lower priority items during idle
                        operation.Invoke();
                    }
                    else
                    {
                        TranslateMessage(message);
                        DispatchMessage(message);
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(message);
            }
            if (--myFrameCount == 0 && myHasShutdownStarted)
            {
                lock (myOperations)
                {
                    DispatcherOperation operation = PopOperation();
                    while (operation != null)
                    {  
                        operation.Abort();
                        operation = PopOperation();
                    }
                }
                myHasShutdownFinished = true;
                if (ShutdownFinished != null)
                    ShutdownFinished(this, EventArgs.Empty);
            }
        }

        internal bool myExitAllFrames = false;
        static void ExitAllFramesHandler()
        {
            Dispatcher.CurrentDispatcher.myExitAllFrames = true;
        }

        public static void ExitAllFrames()
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Send, new EmptyDelegate(ExitAllFramesHandler));
        }

        public static void PushFrame(DispatcherFrame frame)
        {
            Dispatcher currentDispatcher = CurrentDispatcher;
            if (frame.Dispatcher != currentDispatcher)
                throw new InvalidOperationException("frame is running on a different Dispatcher.");

            currentDispatcher.PushFrameInternal(frame);
        }

        public event EventHandler ShutdownFinished;
        public event EventHandler ShutdownStarted;
    }
    public delegate void EmptyDelegate();
}

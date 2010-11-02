using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;

namespace System.Windows.Threading
{
    public class DispatcherOperation
    {
        internal DispatcherOperation(Dispatcher dispatcher, Delegate del, object[] args)
        {
            myDispatcher = dispatcher;
            myDelegate = del;
            myArgs = args;
        }

        Dispatcher myDispatcher;
        public Dispatcher Dispatcher
        {
            get { return myDispatcher; }
        }

        Delegate myDelegate;
        object[] myArgs;
        ManualResetEvent myEvent;
        internal DispatcherOperationStatus myStatus = DispatcherOperationStatus.Pending;

        internal void Invoke()
        {
            lock (this)
            {
                // It can be complete if it ends up the handler and wait is later called.
                if (myStatus == DispatcherOperationStatus.Completed)
                    return;
                myStatus = DispatcherOperationStatus.Executing;
            }
            try
            {
                myResult = myDelegate.Method.Invoke(myDelegate.Target, myArgs);
            }
            finally
            {
                lock (this)
                {
                    myStatus = DispatcherOperationStatus.Completed;
                    NotifyInvokeComplete();
                }
            }
        }

        void NotifyInvokeComplete()
        {
            if (myEvent != null)
            {
                myEvent.Set();
                Thread.Sleep(0);
            }
        }

        public DispatcherOperationStatus Wait()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == myDispatcher)
            {
                lock (this)
                {
                    System.Diagnostics.Debug.Assert(myStatus != DispatcherOperationStatus.Executing);
                    if (myStatus == DispatcherOperationStatus.Completed)
                        return myStatus;
                }
                Invoke();
            }
            else
            {
                lock (this)
                {
                    if (myStatus != DispatcherOperationStatus.Pending)
                        return myStatus;
                    if (myEvent == null)
                        myEvent = new ManualResetEvent(false);
                }
                myEvent.WaitOne();
                Thread.Sleep(0);
            }
            return myStatus;
        }

        public DispatcherOperationStatus Status
        {
            get { return myStatus; }
        }

        object myResult;

        public object Result
        {
            get { return myResult; }
        }
    }

    public delegate void EmptyDelegate();
}

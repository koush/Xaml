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
        internal DispatcherOperation(Dispatcher dispatcher, Delegate del, DispatcherPriority priority, object[] args)
        {
            myDispatcher = dispatcher;
            myDelegate = del;
            myPriority = priority;
            myArgs = args;
        }

        Dispatcher myDispatcher;
        public Dispatcher Dispatcher
        {
            get { return myDispatcher; }
        }

        Delegate myDelegate;
        object[] myArgs;
        DispatcherPriority myPriority;
        ManualResetEvent myEvent;
        internal DispatcherOperationStatus myStatus = DispatcherOperationStatus.Pending;

        internal void Invoke()
        {
            System.Diagnostics.Debug.Assert(myStatus == DispatcherOperationStatus.Executing);
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
            if (myFrame != null)
                myFrame.Continue = false;
        }

        DispatcherFrame myFrame;
        public DispatcherOperationStatus Wait()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == myDispatcher)
            {
                myFrame = new DispatcherFrame();
                myDispatcher.PushFrameInternal(myFrame);
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

        public DispatcherPriority Priority
        {
            get { return myPriority; }
            set
            {
                lock (this)
                {
                    if (myPriority == value)
                        return;
                    if (myStatus != DispatcherOperationStatus.Pending)
                        throw new InvalidOperationException("This operation is already running.");
                    if (!myDispatcher.SetPriority(this, value))
                        throw new InvalidOperationException("This operation is already running.");
                }
            }
        }

        public bool Abort()
        {
            lock (this)
            {
                if (myDispatcher.Abort(this))
                {
                    NotifyInvokeComplete();
                    return true;
                }
            }
            return false;
        }
    }
}

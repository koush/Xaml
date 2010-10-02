using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Windows.Threading
{
    static class ThreadStatic<T> where T: class
    {
        static Dictionary<Thread, T> myTypeTable = new Dictionary<Thread, T>();
        static LocalDataStoreSlot mySlot = Thread.AllocateDataSlot();

        class ThreadWatcher
        {
            Thread myThread;
            public ThreadWatcher(Thread thread)
            {
                myThread = thread;
            }

            ~ThreadWatcher()
            {
                lock (myTypeTable)
                {
                    myTypeTable.Remove(myThread);
                }
            }
        }

        static void SetThreadStatic(Thread thread, T value)
        {
            lock (myTypeTable)
            {
                myTypeTable.Add(thread, value);
                Thread.SetData(mySlot, new ThreadWatcher(thread));
            }
        }

        public static T GetThreadStatic(Thread thread)
        {
            T ret = null;
            lock (myTypeTable)
            {
                myTypeTable.TryGetValue(thread, out ret);
            }
            return ret;
        }

        public static T GetOrCreateThreadStatic(GetOrCreateThreadStaticHandler createCallback)
        {
            Thread thread = Thread.CurrentThread;
            T ret = GetThreadStatic(thread);
            if (ret == null)
            {
                ret = createCallback();
                SetThreadStatic(thread, ret);
            }
            return ret;
        }
        public delegate T GetOrCreateThreadStaticHandler();
    }
}

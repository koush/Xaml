using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
//using System.Threading;

namespace System.Windows.Threading
{
    public class DispatcherTimer
    {
        //Timer myTimer = new Timer();

        TimeSpan myTimeSpan;
        Dispatcher myDispatcher;
        bool myEnabled = true;
        public DispatcherTimer(TimeSpan timeSpan, EventHandler handler, Dispatcher dispatcher)
        {
            myTimeSpan = timeSpan;
            myDispatcher = dispatcher;
            Tick += handler;
            //myTimer.Tick += new EventHandler(myTimer_Tick);
        }

        void myTimer_Tick(object sender, EventArgs e)
        {
            //myTimer.Enabled = false;
            myDispatcher.BeginInvoke(new EmptyDelegate(FireTick));
        }
        
        public void Start()
        {
            myEnabled = true;
            //myTimer.Enabled = true;
        }

        public void Stop()
        {
            myEnabled = false;
            //myTimer.Enabled = false;
        }

        ~DispatcherTimer()
        {
            Stop();
            //myTimer.Tick -= new EventHandler(myTimer_Tick);
            //myTimer = null;
        }

        void FireTick()
        {
            if (Tick != null)
                Tick(Tag, null);
            //if (myEnabled)
            //    myTimer.Enabled = true;
        }

        public event EventHandler Tick;

        public object Tag
        {
            get;
            set;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Windows.Threading
{
    public class DispatcherObject
    {
        Dispatcher myDispatcher = Dispatcher.CurrentDispatcher;
        public Dispatcher Dispatcher
        {
            get
            {
                return myDispatcher;
            }
        }

        public bool CheckAccess()
        {
            return myDispatcher.CheckAccess();
        }

        public void VerifyAccess()
        {
            myDispatcher.VerifyAccess();
        }
    }
}

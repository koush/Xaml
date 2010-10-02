using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Threading
{
    public struct DispatcherProcessingDisabled : IDisposable
    {
        internal Dispatcher myDispatcher;

        #region IDisposable Members

        public void Dispose()
        {
            if (myDispatcher != null)
            {
                myDispatcher.VerifyAccess();
                myDispatcher.myDisableProcessingCount--;
            }
        }

        #endregion
    }
}

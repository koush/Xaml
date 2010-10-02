using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Threading
{
    public class DispatcherFrame : DispatcherObject
    {
        public DispatcherFrame()
        {
        }
        public DispatcherFrame(bool exitWhenRequested)
        {
            myExitWhenRequested = exitWhenRequested;
        }

        bool myContinue = true;
        bool myExitWhenRequested = true;

        public bool Continue
        {
            get
            {
                return (!myExitWhenRequested || !Dispatcher.myExitAllFrames) && myContinue;
            }
            set
            {
                myContinue = value;
            }
        }
    }
}

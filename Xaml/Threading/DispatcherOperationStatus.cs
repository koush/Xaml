using System;

namespace System.Windows.Threading
{
    // Summary:
    //     Describes the possible values for the status of a System.Windows.Threading.DispatcherOperation.
    public enum DispatcherOperationStatus
    {
        // Summary:
        //     The operation is pending and is still in the System.Windows.Threading.Dispatcher
        //     queue.
        Pending = 0,
        //
        // Summary:
        //     The operation has aborted.
        Aborted = 1,
        //
        // Summary:
        //     The operation is completed.
        Completed = 2,
        //
        // Summary:
        //     The operation started executing, but has not completed.
        Executing = 3,
    }
}

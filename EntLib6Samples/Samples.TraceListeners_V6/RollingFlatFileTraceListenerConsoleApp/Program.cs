using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RollingFlatFileTraceListenerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Previously this would could throw an exception if permission issues or an invalid network drive occurred
            var logWriter = new LogWriterFactory().Create();
            
            // This will throw the exception encountered during initialization above but the exception will be 
            // attempted to be logged to the errors special source (in this project the Event Log under the 
            // source Enterprise Library Logging) and the exception swallowed
            logWriter.Write("To be...", "Failure Category");

            // This will create a logs directory and write to rolling.log file in the logs directory
            logWriter.Write("or not to be.", "Success Category");
        }
    }
}

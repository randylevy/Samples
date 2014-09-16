using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RollingXmlTraceListenerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();

            // log for 10 minutes...config is set to keep 7 files
            for (int i = 0; i < 10; i++)
            {
                Console.Write("\nWriting log");
                logger.Write("Testing!", "General");

                for (int j = 0; j < 60; j++)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(".");
                }
            }
        }
    }
}

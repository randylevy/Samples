//===============================================================================
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ExtendedPropertiesConsoleApp
{
    class Program
    {
        /// <summary>
        /// Used for just writing a log with extended properties
        /// </summary>
        static void Log()
        {
            using (var logger = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>())
            {
                var logEntry = new LogEntry()
                {
                    Message = "Log this message!"
                };

                logEntry.ExtendedProperties = new Dictionary<string, object>()
                {
                    { "hello", "world" }, 
                    { "hello, again", "hello" }, 
                    { "Goodbye", "Cruel world! & ' <> \" " } 
                };

                logEntry.Categories.Add("General");

                logger.Write(logEntry);
            }
        }

        static void HandleException()
        {
            try
            {
                var ex = new Exception("Bad!");
                ex.Data.Add("filename", @"C:\temp\file.txt");
                ex.Data.Add("Hello", "World");
                throw ex;
            }
            catch (Exception e)
            {
                bool shouldRethrow = ExceptionPolicy.HandleException(e, "Policy");

                if (shouldRethrow)
                {
                    throw;
                }
            }
        }

        static void Main(string[] args)
        {
            HandleException();
            Log();
        }
    }
}

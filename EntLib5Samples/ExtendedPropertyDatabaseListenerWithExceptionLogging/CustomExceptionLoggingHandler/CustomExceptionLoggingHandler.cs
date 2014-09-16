//===============================================================================
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CustomExceptionLoggingHandler
{
    [ConfigurationElementType(typeof(CustomExceptionLoggingHandlerData))]
    public class CustomExceptionLoggingHandler : IExceptionHandler
    {
        private readonly string logCategory;
        private readonly int eventId;
        private readonly TraceEventType severity;
        private readonly string defaultTitle;
        private readonly Type formatterType;
        private readonly int minimumPriority;
        private readonly LogWriter logWriter;

        public CustomExceptionLoggingHandler(
            string logCategory,
            int eventId,
            TraceEventType severity,
            string title,
            int priority,
            Type formatterType,
            LogWriter writer)
        {
            this.logCategory = logCategory;
            this.eventId = eventId;
            this.severity = severity;
            this.defaultTitle = title;
            this.minimumPriority = priority;
            this.formatterType = formatterType;
            this.logWriter = writer;
        }

        protected virtual void WriteToLog(string logMessage, IDictionary exceptionData)
        {
            LogEntry entry = new LogEntry(
                logMessage,
                logCategory,
                minimumPriority,
                eventId,
                severity,
                defaultTitle,
                null);

            foreach (DictionaryEntry dataEntry in exceptionData)
            {
                if (dataEntry.Key is string)
                {
                    entry.ExtendedProperties.Add(dataEntry.Key as string, dataEntry.Value);
                }
            }

            this.logWriter.Write(entry);
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            // Add custom data to exception Data which will be added to Extended Properties and logged.
            exception.Data.Add("handlingInstanceId", handlingInstanceId);
            exception.Data.Add("CallStack", exception.StackTrace);
            exception.Data.Add("ErrorMessage", exception.Message);
         
            WriteToLog(CreateMessage(exception, handlingInstanceId), exception.Data);
            return exception;
        }

        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            StringWriter writer = null;
            StringBuilder stringBuilder = null;
            try
            {
                writer = CreateStringWriter();
                Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter formatter = CreateFormatter(writer, exception, handlingInstanceID);
                formatter.Format();
                stringBuilder = writer.GetStringBuilder();

            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return stringBuilder.ToString();
        }

        protected virtual StringWriter CreateStringWriter()
        {
            return new StringWriter(CultureInfo.InvariantCulture);
        }

        private ConstructorInfo GetFormatterConstructor()
        {
            Type[] types = new Type[] { typeof(TextWriter), typeof(Exception), typeof(Guid) };
            ConstructorInfo constructor = formatterType.GetConstructor(types);
            if (constructor == null)
            {
                throw new ExceptionHandlingException("Unable to get the constructor.");
            }
            return constructor;
        }

        protected virtual Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter CreateFormatter(
            StringWriter writer,
            Exception exception,
            Guid handlingInstanceID)
        {
            ConstructorInfo constructor = GetFormatterConstructor();
            return (Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter)constructor.Invoke(
                new object[] { writer, exception, handlingInstanceID }
                );
        }
    }
}

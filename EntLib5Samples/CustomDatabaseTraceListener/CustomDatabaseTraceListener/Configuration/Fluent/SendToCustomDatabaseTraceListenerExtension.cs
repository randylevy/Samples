using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace CustomDatabaseTraceListener.Configuration.Fluent
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="CustomDatabaseTraceListener"/>.
    /// </summary>
    /// <seealso cref="CustomDatabaseTraceListener"/>
    /// <seealso cref="CustomDatabaseTraceListenerData"/>
    public static class SendToCustomDatabaseTraceListenerExtension
    {
        /// <summary>
        /// Adds a new <see cref="CustomDatabaseTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="CustomDatabaseTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomDatabaseTraceListenerData"/>. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        public static ILoggingConfigurationSendToCustomDatabaseTraceListener CustomDatabase(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToCustomDatabaseTraceListenerBuilder(context, listenerName);
        }

        private class SendToCustomDatabaseTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToCustomDatabaseTraceListener
        {
            CustomDatabaseTraceListenerData databaseTraceListener;
            public SendToCustomDatabaseTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                : base(context)
            {
                databaseTraceListener = new CustomDatabaseTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(databaseTraceListener);
            }


            public ILoggingConfigurationSendToCustomDatabaseTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                if (formatBuilder == null) throw new ArgumentNullException("formatBuilder");

                FormatterData formatter = formatBuilder.GetFormatterData();
                databaseTraceListener.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);

                return this;
            }

            public ILoggingConfigurationSendToCustomDatabaseTraceListener FormatWithSharedFormatter(string formatterName)
            {
                databaseTraceListener.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToCustomDatabaseTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                databaseTraceListener.TraceOutputOptions = traceOptions;

                return this;
            }

            public ILoggingConfigurationSendToCustomDatabaseTraceListener Filter(SourceLevels sourceLevel)
            {
                databaseTraceListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToCustomDatabaseTraceListener WithAddCategoryStoredProcedure(string addCategoryStoredProcedureName)
            {
                if (string.IsNullOrEmpty(addCategoryStoredProcedureName))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "addCategoryStoredProcedureName");

                databaseTraceListener.AddCategoryStoredProcName = addCategoryStoredProcedureName;

                return this;
            }

            public ILoggingConfigurationSendToCustomDatabaseTraceListener WithWriteLogStoredProcedure(string writeLogStoredProcedureName)
            {
                if (string.IsNullOrEmpty(writeLogStoredProcedureName))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "writeLogStoredProcedureName");

                databaseTraceListener.WriteLogStoredProcName = writeLogStoredProcedureName;

                return this;
            }

            public ILoggingConfigurationSendToCustomDatabaseTraceListener UseDatabase(string databaseInstanceName)
            {
                databaseTraceListener.DatabaseInstanceName = databaseInstanceName;

                return this;
            }
        }
    }

}

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace CustomDatabaseTraceListener.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="CustomDatabaseTraceListener"/> instance.
    /// </summary>
    /// <seealso cref="CustomDatabaseTraceListener"/>
    /// <seealso cref="CustomDatabaseTraceListenerData"/>
    public interface ILoggingConfigurationSendToCustomDatabaseTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {

        /// <summary>
        /// Specifies the formatter used to format database log messages send by this <see cref="CustomDatabaseTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener FormatWith(IFormatterBuilder formatBuilder);


        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="CustomDatabaseTraceListener"/>.<br/>
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="CustomDatabaseTraceListener"/>.<br/>
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener WithTraceOptions(TraceOptions traceOptions);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="CustomDatabaseTraceListener"/>.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies the name of the stored procedure that should be used to add a new log category.
        /// </summary>
        /// <param name="addCategoryStoredProcedureName">The name of the stored procedure that should be used to add a new log category.</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener WithAddCategoryStoredProcedure(string addCategoryStoredProcedureName);


        /// <summary>
        /// Specifies the name of the stored procedure that should be used when writing a log entry.
        /// </summary>
        /// <param name="writeLogStoredProcedureName">The name of the stored procedure that should be used when writing a log entry.</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener WithWriteLogStoredProcedure(string writeLogStoredProcedureName);

        /// <summary>
        /// Specifies which database instance, or connection string, should be used to send log messages to.
        /// </summary>
        /// <param name="databaseInstanceName">The name of the database instance, or connection string, should be used to send log messages to.</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="CustomDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="CustomDatabaseTraceListener"/>
        /// <seealso cref="CustomDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToCustomDatabaseTraceListener UseDatabase(string databaseInstanceName);
    }

}

using System.Diagnostics;
using CustomDatabaseTraceListener;
using CustomDatabaseTraceListener.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CustomDatabaseTraceListenerQuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomLogEntry logEntry = new CustomLogEntry()
            {
                Categories = new string[] { "General" },
                Message = "This is the message to log!",
                Severity = System.Diagnostics.TraceEventType.Information,
                CustomData = "My Custom Data"
            };

            LogWithConfigFile(logEntry);
            LogWithFluentInterface(logEntry);
        }

        static void LogWithConfigFile(CustomLogEntry logEntry)
        {
            var logger = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            logger.Write(logEntry);
            Logger.Write("Test", "General");
        }

        static void LogWithFluentInterface(CustomLogEntry logEntry)
        {
            var builder = new ConfigurationSourceBuilder();

            builder.ConfigureData()
                .ForDatabaseNamed("Logging")
                    .ThatIs.ASqlDatabase()
                    .WithConnectionString(@"data source=.\SQLEXPRESS;Integrated Security=SSPI;Database=Logging")
                .AsDefault();

            builder.ConfigureLogging()
                    .WithOptions
                        .DoNotRevertImpersonation()
                    .LogToCategoryNamed("General")
                        .WithOptions.SetAsDefaultCategory()
                        .SendTo.CustomDatabase("Custom Database Trace Listener")
                        .WithAddCategoryStoredProcedure("AddCategory")
                        .UseDatabase("Logging")
                        .Filter(System.Diagnostics.SourceLevels.All)
                        .WithWriteLogStoredProcedure("WriteLog")
                    .SpecialSources.LoggingErrorsAndWarningsCategory
                        .SendTo.EventLog("Event Log Listener")
                        .FormatWith(new FormatterBuilder()
                             .TextFormatterNamed("Text Formatter")
                               .UsingTemplate(@"Timestamp: {timestamp}{newline}
Message: {message}{newline}
Category: {category}{newline}
Priority: {priority}{newline}
EventId: {eventid}{newline}
Severity: {severity}{newline}
Title:{title}{newline}
Machine: {localMachine}{newline}
App Domain: {localAppDomain}{newline}
ProcessId: {localProcessId}{newline}
Process Name: {localProcessName}{newline}
Thread Name: {threadName}{newline}
Win32 ThreadId:{win32ThreadId}{newline}
Extended Properties: {dictionary({key} - {value}{newline})}"))
                            .ToLog("Application")
                            .ToMachine(".")
                            .UsingEventLogSource("Enterprise Library Logging")
                            .Filter(SourceLevels.All)
                            ;
                                                            

            var configSource = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(configSource);
            EnterpriseLibraryContainer.Current
                = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);

            var logger = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            logger.Write(logEntry);
            Logger.Write("Test", "General");
        }
    }
}

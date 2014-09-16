//===============================================================================
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace CustomDatabaseTraceListener.Configuration
{
    /// <summary>
    /// Configuration object for a <see cref="CustomDatabaseTraceListener"/>.
    /// </summary>
    [AddSateliteProviderCommand("connectionStrings", typeof(DatabaseSettings), "DefaultDatabase", "DatabaseInstanceName")]
    [ResourceDescription(typeof(DesignResources), "CustomDatabaseTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomDatabaseTraceListenerDataDisplayName")]
    public class CustomDatabaseTraceListenerData : TraceListenerData
    {
        private const string addCategoryStoredProcNameProperty = "addCategoryStoredProcName";
        private const string databaseInstanceNameProperty = "databaseInstanceName";
        private const string formatterNameProperty = "formatter";
        private const string writeLogStoredProcNameProperty = "writeLogStoredProcName";

        /// <summary>
        /// Initializes a <see cref="CustomDatabaseTraceListenerData"/>.
        /// </summary>
        public CustomDatabaseTraceListenerData()
            : base(typeof(CustomDatabaseTraceListener))
        {
            this.ListenerDataType = typeof(CustomDatabaseTraceListenerData);
        }

        /// <summary>
        /// Initializes a named instance of <see cref="CustomDatabaseTraceListenerData"/> with 
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="formatterName">The formatter name.</param>        
        public CustomDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName)
            : this(
                name,
                writeLogStoredProcName,
                addCategoryStoredProcName,
                databaseInstanceName,
                formatterName,
                TraceOptions.None,
                SourceLevels.All)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="CustomDatabaseTraceListenerData"/> with 
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to be applied</param>
        public CustomDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName,
                                                  TraceOptions traceOutputOptions,
                                                  SourceLevels filter)
            : base(name, typeof(CustomDatabaseTraceListener), traceOutputOptions, filter)
        {
            DatabaseInstanceName = databaseInstanceName;
            WriteLogStoredProcName = writeLogStoredProcName;
            AddCategoryStoredProcName = addCategoryStoredProcName;
            Formatter = formatterName;
        }

        /// <summary>
        /// Gets and sets the database instance name.
        /// </summary>
        [ConfigurationProperty(databaseInstanceNameProperty, IsRequired = true)]
        [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
        [ResourceDescription(typeof(DesignResources), "CustomDatabaseTraceListenerDataDatabaseInstanceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomDatabaseTraceListenerDataDatabaseInstanceNameDisplayName")]
        public string DatabaseInstanceName
        {
            get { return (string)base[databaseInstanceNameProperty]; }
            set { base[databaseInstanceNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for writing the log.
        /// </summary>
        [ConfigurationProperty(writeLogStoredProcNameProperty, IsRequired = true, DefaultValue = "WriteLog")]
        [ResourceDescription(typeof(DesignResources), "CustomDatabaseTraceListenerDataWriteLogStoredProcNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomDatabaseTraceListenerDataWriteLogStoredProcNameDisplayName")]
        public string WriteLogStoredProcName
        {
            get { return (string)base[writeLogStoredProcNameProperty]; }
            set { base[writeLogStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for adding a category for this log.
        /// </summary>
        [ConfigurationProperty(addCategoryStoredProcNameProperty, IsRequired = true, DefaultValue = "AddCategory")]
        [ResourceDescription(typeof(DesignResources), "CustomDatabaseTraceListenerDataAddCategoryStoredProcNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomDatabaseTraceListenerDataAddCategoryStoredProcNameDisplayName")]
        public string AddCategoryStoredProcName
        {
            get { return (string)base[addCategoryStoredProcNameProperty]; }
            set { base[addCategoryStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [ResourceDescription(typeof(DesignResources), "CustomDatabaseTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomDatabaseTraceListenerDataFormatterDisplayName")]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// A <see cref="FlatFileTraceListener"/>.
        /// </returns>
        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            var database = DatabaseFactory.CreateDatabase(this.DatabaseInstanceName);
            var formatter = this.BuildFormatterSafe(settings, this.Formatter);

            return new CustomDatabaseTraceListener(database, WriteLogStoredProcName, AddCategoryStoredProcName, formatter);
        }
    }
}

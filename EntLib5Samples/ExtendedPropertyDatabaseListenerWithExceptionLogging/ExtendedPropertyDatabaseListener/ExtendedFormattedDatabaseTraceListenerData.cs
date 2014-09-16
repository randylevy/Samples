//===============================================================================
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace ExtendedPropertyDatabaseListener
{
    /// <summary>
    /// Configuration object for a <see cref="ExtendedFormattedDatabaseTraceListener"/>.
    /// </summary>
    [AddSateliteProviderCommand("connectionStrings", typeof(DatabaseSettings), "DefaultDatabase", "DatabaseInstanceName")]
    public class ExtendedFormattedDatabaseTraceListenerData : TraceListenerData
    {
        private const string addCategoryStoredProcNameProperty = "addCategoryStoredProcName";
        private const string databaseInstanceNameProperty = "databaseInstanceName";
        private const string formatterNameProperty = "formatter";
        private const string writeLogStoredProcNameProperty = "writeLogStoredProcName";

        /// <summary>
        /// Initializes a <see cref="FormattedDatabaseTraceListenerData"/>.
        /// </summary>
        public ExtendedFormattedDatabaseTraceListenerData()
            : base(typeof(ExtendedFormattedDatabaseTraceListener))
        {
            this.ListenerDataType = typeof(ExtendedFormattedDatabaseTraceListenerData);
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FormattedDatabaseTraceListenerData"/> with 
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="formatterName">The formatter name.</param>        
        public ExtendedFormattedDatabaseTraceListenerData(string name,
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
        /// Initializes a named instance of <see cref="FormattedDatabaseTraceListenerData"/> with 
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to be applied</param>
        public ExtendedFormattedDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName,
                                                  TraceOptions traceOutputOptions,
                                                  SourceLevels filter)
            : base(name, typeof(ExtendedFormattedDatabaseTraceListener), traceOutputOptions, filter)
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
        public string DatabaseInstanceName
        {
            get { return (string)base[databaseInstanceNameProperty]; }
            set { base[databaseInstanceNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for writing the log.
        /// </summary>
        [ConfigurationProperty(writeLogStoredProcNameProperty, IsRequired = true, DefaultValue = "WriteLog")]
        public string WriteLogStoredProcName
        {
            get { return (string)base[writeLogStoredProcNameProperty]; }
            set { base[writeLogStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for adding a category for this log.
        /// </summary>
        [ConfigurationProperty(addCategoryStoredProcNameProperty, IsRequired = true, DefaultValue = "AddCategory")]
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
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                   new ExtendedFormattedDatabaseTraceListener(
                       Container.Resolved<Microsoft.Practices.EnterpriseLibrary.Data.Database>(DatabaseInstanceName),
                       WriteLogStoredProcName,
                       AddCategoryStoredProcName,
                       Container.ResolvedIfNotNull<ILogFormatter>(Formatter));
        }
    }
}

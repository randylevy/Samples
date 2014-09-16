using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CustomDatabaseTraceListener
{
    [XmlRoot("customLogEntry")]
    [Serializable]
    public class CustomLogEntry : LogEntry
    {
        private string customData;

        /// <summary>
        /// Create an empty <see cref="CustomLogEntry"/>.
        /// </summary>
        public CustomLogEntry()
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="LogEntry"/> with a full set of constructor parameters
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to record.</param>
        /// <param name="customData">Custom data not included in base LogEntry</param>        
        public CustomLogEntry(object message, string category, int priority, int eventId,
                        TraceEventType severity, string title, IDictionary<string, object> properties, string customData)
            : base(message, category, priority, eventId, severity, title, properties)
        {
            this.customData = customData;
        }

        /// <summary>
        /// Create a new instance of <see cref="LogEntry"/> with a full set of constructor parameters
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Collection of category names used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to record.</param>
        /// <param name="customData">Custom data not included in base LogEntry</param>        
        public CustomLogEntry(object message, ICollection<string> categories, int priority, int eventId,
                        TraceEventType severity, string title, IDictionary<string, object> properties, string customData)
            :base(message, categories, priority, eventId, severity, title, properties)
        {
            this.customData = customData;
        }

        public string CustomData
        {
            get { return this.customData; }
            set { this.customData = value; }
        }
    }
}

﻿using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Samples.TraceListeners
{
    /// <summary>
    /// Extends <see cref="TextWriterTraceListener"/> to add formatting capabilities.
    /// </summary>
    public class FormattedTextWriterTraceListener : TextWriterTraceListener
    {
        private ILogFormatter formatter;
        protected Exception initializationException;

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/>.
        /// </summary>
        public FormattedTextWriterTraceListener()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(ILogFormatter formatter)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(Stream stream, ILogFormatter formatter)
            : this(stream)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public FormattedTextWriterTraceListener(Stream stream)
            : base(stream)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(TextWriter writer, ILogFormatter formatter)
            : this(writer)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public FormattedTextWriterTraceListener(TextWriter writer)
            : base(writer)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a file name.
        /// </summary>
        /// <param name="fileName">The file name to write to.</param>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(string fileName, ILogFormatter formatter)
            : this(fileName)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FormattedTextWriterTraceListener"/> with a file name.
        /// </summary>
        /// <param name="fileName">The file name to write to.</param>
        public FormattedTextWriterTraceListener(string fileName)
            : base(RootFileName(fileName))
        {
            EnsureTargetFolderExists(fileName);
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(Stream stream, string name, ILogFormatter formatter)
            : this(stream, name)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FormattedTextWriterTraceListener"/> with a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="name">The name.</param>
        public FormattedTextWriterTraceListener(Stream stream, string name)
            : base(stream, name)
        { }

        /// <summary>
        /// Initializes a new named instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(TextWriter writer, string name, ILogFormatter formatter)
            : this(writer, name)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="name">The name.</param>
        public FormattedTextWriterTraceListener(TextWriter writer, string name)
            : base(writer, name)
        { }

        /// <summary>
        /// Initializes a new named instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a file name.
        /// </summary>
        /// <param name="fileName">The file name to write to.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter to format the messages.</param>
        public FormattedTextWriterTraceListener(string fileName, string name, ILogFormatter formatter)
            : this(fileName, name)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FormattedTextWriterTraceListener"/> with a 
        /// <see cref="ILogFormatter"/> and a file name.
        /// </summary>
        /// <param name="fileName">The file name to write to.</param>
        /// <param name="name">The name.</param>
        public FormattedTextWriterTraceListener(string fileName, string name)
            : base(RootFileName(fileName), name)
        {
            EnsureTargetFolderExists(fileName);
        }

        /// <summary>
        /// Intercepts the tracing request to format the object to trace.
        /// </summary>
        /// <remarks>
        /// Formatting is only performed if the object to trace is a <see cref="LogEntry"/> and the formatter is set.
        /// </remarks>
        /// <param name="eventCache">The context information.</param>
        /// <param name="source">The trace source.</param>
        /// <param name="eventType">The severity.</param>
        /// <param name="id">The event id.</param>
        /// <param name="data">The object to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    if (this.Formatter != null)
                    {
                        base.Write(this.Formatter.Format(data as LogEntry));
                    }
                    else
                    {
                        base.TraceData(eventCache, source, eventType, id, data);
                    }
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ILogFormatter"/> used to format the trace messages.
        /// </summary>
        public ILogFormatter Formatter
        {
            get
            {
                return this.formatter;
            }

            set
            {
                this.formatter = value;
            }
        }

        /// <summary>
        /// Declares "formatter" as a supported attribute name.
        /// </summary>
        /// <returns></returns>
        protected override string[] GetSupportedAttributes()
        {
            return new string[1] { "formatter" };
        }

        protected void ValidateInitializationWasSuccessful()
        {
            if (initializationException != null)
            {
                throw initializationException;
            }
        }

        private void EnsureTargetFolderExists(string fileName)
        {
            string rootedFileName = RootFileName(fileName);

            string directory = Path.GetDirectoryName(rootedFileName);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception e)
                {
                    // Preserve stack trace with inner exception
                    initializationException = Activator.CreateInstance(e.GetType(),
                        new object[] { e.Message, e }
                        ) as Exception;
                }
            }
        }

        private static string RootFileName(string fileName)
        {
            string rootedFileName = fileName;
            if (!Path.IsPathRooted(rootedFileName))
            {
                rootedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootedFileName);
            }

            return rootedFileName;
        }
    }
}

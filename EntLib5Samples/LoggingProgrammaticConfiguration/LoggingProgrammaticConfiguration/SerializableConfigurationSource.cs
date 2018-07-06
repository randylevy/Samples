using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace LoggingProgrammaticConfiguration
{
    public class SerializableConfigurationSource : IConfigurationSource
    {
        private readonly Dictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();

        public ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection configSection;
            if (!this.sections.TryGetValue(sectionName, out configSection))
            {
                return null;
            }

            var section = configSection as SerializableConfigurationSection;
            if (section == null)
            {
                return null;
            }

            using (var xml = new StringWriter())
            using (var xmlwriter = System.Xml.XmlWriter.Create(xml))
            {
                section.WriteXml(xmlwriter);
                xmlwriter.Flush();

                MethodInfo methodInfo = section.GetType().GetMethod("DeserializeSection", BindingFlags.NonPublic | BindingFlags.Instance);
                methodInfo.Invoke(section, new object[] { XDocument.Parse(xml.ToString()).CreateReader() });

                return configSection;
            }
        }

        public void Add(string sectionName, ConfigurationSection configurationSection)
        {
            this.sections[sectionName] = configurationSection;
        }

        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Remove(string sectionName)
        {
            this.sections.Remove(sectionName);
        }

        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        public void Dispose()
        {
        }
    }
}

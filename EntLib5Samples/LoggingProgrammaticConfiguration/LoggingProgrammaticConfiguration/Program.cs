using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace LoggingProgrammaticConfiguration
{
    internal class Program
    {
        private static void Main()
        {
            IConfigurationSource configSource = ConfigurationSourceFactory.Create();
            var logSettings = configSource.GetSection(LoggingSettings.SectionName) as LoggingSettings;

            SetNewFileName(logSettings);

            var loggingXmlConfigSource = new SerializableConfigurationSource();
            loggingXmlConfigSource.Add(LoggingSettings.SectionName, logSettings);

            IUnityContainer container = CreateContainer();

            InitializeLogging(container, loggingXmlConfigSource);

            SetContainer(container);

            Logger.Write("Test", "General");
        }


        private static void SetNewFileName(LoggingSettings logSettings)
        {
            var flatFileListeners = logSettings.TraceListeners
                .Where(t => t is FlatFileTraceListenerData)
                .Cast<FlatFileTraceListenerData>()
                .Select(oldData =>
                    new FlatFileTraceListenerData(oldData.Name,
                        oldData.FileName + ".new", // New FileName
                        oldData.Header,
                        oldData.Footer,
                        oldData.Formatter,
                        oldData.TraceOutputOptions));

            flatFileListeners.Cast<TraceListenerData>().ToList()
                .ForEach(data =>
                {
                    logSettings.TraceListeners.Remove(data.Name);
                    logSettings.TraceListeners.Add(data);
                });
        }

        private static IUnityContainer CreateContainer()
        {
            // Create the container
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<EnterpriseLibraryCoreExtension>();

            return container;
        }

        private static void InitializeLogging(IUnityContainer container, SerializableConfigurationSource loggingXmlConfigSource)
        {
            // Configurator will read Enterprise Library configuration
            // and set up the container
            var configurator = new UnityContainerConfigurator(container);

            // Configure the container with our own custom logging
            EnterpriseLibraryContainer.ConfigureContainer(configurator, loggingXmlConfigSource);
        }

        private static void SetContainer(IUnityContainer container)
        {
            // Wrap in ServiceLocator
            // And set Enterprise Library to use it
            EnterpriseLibraryContainer.Current = new UnityServiceLocator(container);
        }
    }
}

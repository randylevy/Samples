using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RemoteServiceTraceListenerSample
{
    public partial class MainPage : UserControl
    {
        private LogWriter logger;

        public MainPage()
        {
            InitializeComponent();

            InitializeEnterpriseLibrary();
            InitializeLogger();
        }

        private void InitializeEnterpriseLibrary()
        {
            string xaml;
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("RemoteServiceTraceListenerSample.LoggingConfig.xaml"))
            using (StreamReader sr = new StreamReader(s))
            {
                xaml = sr.ReadToEnd();
            }

            var configDictionary = (IDictionary)XamlReader.Load(xaml);
            var configSource = DictionaryConfigurationSource.FromDictionary(configDictionary);
            EnterpriseLibraryContainer.Current = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
        }

        private void InitializeLogger()
        {
            logger = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            logger.Write("If a log falls on the client does the server hear?", "default");                  
        }
    }
}

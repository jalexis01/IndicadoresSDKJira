using Microsoft.Extensions.Configuration;
using System.IO;

namespace MQTT.Subscriber
{
    public sealed class AppSettings
    {
        private static AppSettings instance = null;
        public IConfigurationRoot Configuration { get; set; }
        private AppSettings()
        {
            GetAppSettings();
        }
        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppSettings();
                }
                return instance;
            }
        }
        public void GetAppSettings()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("_appsettings.json".Trim(), optional: true, reloadOnChange: true);

            Configuration = configurationBuilder.Build();
        }
    }
}

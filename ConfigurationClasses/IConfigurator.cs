using System.Collections.Generic;
using System.Configuration;

namespace ConfigurationClasses
{
    public interface IConfigurator
    {
        KeyValueConfigurationCollection GetAppSettings();

        bool SetAppValue(string key, string value);

    }
}

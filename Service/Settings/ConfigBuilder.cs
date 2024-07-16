using Microsoft.Extensions.Configuration;

namespace Service.Settings;

public class ConfigBuilder
{
    public static Config? Build(string fileConfig)
    {
        return new ConfigurationBuilder()
            .AddJsonFile(fileConfig)
            .AddEnvironmentVariables()
            .Build()
            .Get<Config>();
    }
}
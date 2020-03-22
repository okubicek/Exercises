using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BootstrapInXunit
{
    public class BootstrapFixture
    {
        public IServiceProvider Provider { get; set; }

        public BootstrapFixture()
        {
            var config = GetConfiguration();
            var services = new ServiceCollection();

            services.AddOptions();
            services.Configure<CommonOptions>(config.GetSection("Common"));

            Provider = services.BuildServiceProvider();
        }

        public IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }   

    [CollectionDefinition("Bootstrap")]
    public class BootstrapFixtureDefinition
    {
    }
}

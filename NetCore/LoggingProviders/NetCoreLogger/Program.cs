using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetCoreLogger
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging((hostingContext, logBuilder) =>
				{
					logBuilder.ClearProviders(); // removes all providers from LoggerFactory
					logBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logBuilder.AddConsole(config => 
					{
						config.IncludeScopes = true;
					});
					logBuilder.AddDebug();
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}

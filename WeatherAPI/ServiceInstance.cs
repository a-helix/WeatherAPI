using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Threading;
using TaskController;
using DatabaseClient;

namespace WeatherAPI
{
    public class ServiceInstance :  IHostedService
    {
        private string[] _args;

        public ServiceInstance(string[] args)
        {
            _args = args;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string loggerConfigPath = Path.Join("Configs", "nlog.config.xml");
            string configPath = Path.Join("Configs", "ApiConfigs.json");
            var logger = NLogBuilder.ConfigureNLog(loggerConfigPath).GetCurrentClassLogger();
            MongoDatabaseClient databaseClient = new MongoDatabaseClient(configPath, "Service", "service");
            WeatherApiController controller = new WeatherApiController(configPath, databaseClient);
            try
            {
                Thread thread = new Thread(controller.Run);
                logger.Debug("init main");
                CreateHostBuilder(_args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                            Host.CreateDefaultBuilder(args)
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            }).ConfigureLogging(logging =>
                            {
                                logging.ClearProviders();
                                logging.SetMinimumLevel(LogLevel.Trace);
                            })
                                .UseNLog();

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => NLog.LogManager.Shutdown());
        }
    }
}

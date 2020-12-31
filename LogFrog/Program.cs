using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LogFrog.Core;
using LogFrog.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace LogFrog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => ConfigureTelegramWorker(services));

        private static IServiceCollection ConfigureTelegramWorker(IServiceCollection services)
        {
            return services
                .AddSingleton(GetTelegramSettings())
                .AddSingleton<ILogFrogService, LogFrogService>()
                .AddHostedService<TelegramWorker>();
        }

        private static TelegramWorkerSettings GetTelegramSettings()
        {
            var text = File.ReadAllText("telegram.settings.json");

            return JsonConvert.DeserializeObject<TelegramWorkerSettings>(text);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogFrog.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace LogFrog.Telegram
{
    public class TelegramWorker : BackgroundService
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ILogFrogService logFrogService;

        public TelegramWorker(string telegramToken, ILogFrogService logFrogService)
        {
            this.logFrogService = logFrogService;
            telegramBotClient = new TelegramBotClient(telegramToken);
            
            ConfigureTelegramClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            telegramBotClient.StartReceiving(cancellationToken: stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            telegramBotClient.StopReceiving();
        }

        private void ConfigureTelegramClient()
        {
            telegramBotClient.OnMessage += (e, args) => logFrogService.Log(new LogEvent
            {
                DateTime = DateTime.Now,
                Category = LogEventCategory.Info,
                Text = args.Message.Text,
                UserId = 1
            });
        }
    }
}
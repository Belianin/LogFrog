using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogFrog.Core;
using LogFrog.Core.Repositories;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogFrog.Telegram
{
    public class TelegramWorker : BackgroundService
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ILog log;

        public TelegramWorker(string telegramToken, ILog log)
        {
            this.log = log;
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
            telegramBotClient.OnMessage += (e, args) => LogMessage(args.Message);
        }

        private void LogMessage(Message message)
        {
            log.Log(new LogEvent
            {
                DateTime = DateTime.Now,
                Category = LogEventCategory.Info,
                Text = message.Text,
                Id = message.From.Id
            });
        }

    }
}
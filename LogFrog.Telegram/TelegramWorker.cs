using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogFrog.Core;
using LogFrog.Core.Repositories;
using LogFrog.Telegram.Dialogs;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LogFrog.Telegram
{
    public class TelegramWorker : BackgroundService
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ILog log;
        
        private readonly Dictionary<int, IDialogNode> userStates = new Dictionary<int, IDialogNode>();

        public TelegramWorker(TelegramWorkerSettings settings, ILog log)
        {
            this.log = log;
            telegramBotClient = new TelegramBotClient(settings.Token);
            
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
            telegramBotClient.OnMessage += async (e, args) => await OnMessageAsync(args.Message);
        }

        private async Task OnMessageAsync(Message message)
        {
            if (!userStates.TryGetValue(message.From.Id, out var node))
            {
                node = new StartNode();
                userStates[message.From.Id] = node;
            }

            if (node is StartNode)
            {
                log.Log(new LogEvent
                {
                    DateTime = DateTime.Now,
                    Category = LogEventCategory.Info,
                    Text = message.Text,
                    UserId = message.From.Id
                });
            }
            else
            {
                var reply = node.Reply(message.Text);
                userStates[message.From.Id] = reply;

                await telegramBotClient
                    .SendTextMessageAsync(message.Chat.Id, reply.Text, ParseMode.MarkdownV2, replyMarkup: reply.Markup)
                    .ConfigureAwait(false);

            }
        }
    }
}
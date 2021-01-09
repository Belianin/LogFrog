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
        private readonly IStartNodeProvider startNodeProvider;
        private readonly Dictionary<int, IDialogNode> userStates = new Dictionary<int, IDialogNode>();

        public TelegramWorker(TelegramWorkerSettings settings, IStartNodeProvider startNodeProvider)
        {
            this.startNodeProvider = startNodeProvider;
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
                node = startNodeProvider.GetStartNode(message.From.Id);
                userStates[message.From.Id] = node;
            }


            var reply = node.Reply(message) ?? startNodeProvider.GetStartNode(message.From.Id);

            userStates[message.From.Id] = reply;

            await telegramBotClient
                .SendTextMessageAsync(message.Chat.Id, reply.Text, ParseMode.MarkdownV2, replyMarkup: reply.Markup)
                .ConfigureAwait(false);
        }
    }
}
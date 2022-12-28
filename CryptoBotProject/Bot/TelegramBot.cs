using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot
{
    class TelegramBot
    {
        private static TelegramBot instance;
        public static TelegramBot Instance
        {
            get
            {
                instance ??= new TelegramBot();
                return instance;
            }
        }

        public TelegramBotClient BotClient { get; private set; }

        protected TelegramBot()
        {

        }

        public async static void Start(string token = "5901716200:AAHDRRygf7zl260udKhmczgkADVzqaLll6Y")//TODO: REMOVE TOKEN
        {
            Instance.BotClient = new TelegramBotClient(token);
            Instance.Start();
        }

        private async void Start()
        {
            using CancellationTokenSource cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            Instance.BotClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token);

            Console.WriteLine($"Start listening for @{(await BotClient.GetMeAsync()).Username}");
        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch(update.Type)
            {
                case UpdateType.Message:
                    ActiveChat.GetChat(update.Message.Chat.Id).SendUpdate(update);
                    break;
                case UpdateType.CallbackQuery:
                    ActiveChat.GetChat(update.CallbackQuery.From.Id).SendUpdate(update);
                    break;
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
            //commit??
        }
    }
}

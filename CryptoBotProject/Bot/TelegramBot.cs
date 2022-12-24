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

        private TelegramBotClient botClient;

        protected TelegramBot()
        {

        }

        public static void Start(string token = "1706882056:AAFfk4F_ZWZ3_h1Mx44SimSO_5JxQHAIKRM")//TODO: REMOVE TOKEN
        {
            Instance.botClient = new TelegramBotClient(token);
            Instance.Start();
        }

        private void Start()
        {
            using CancellationTokenSource cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            Instance.botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token);
        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch(update.Type)
            {
                case UpdateType.Message:
                    Seance.GetSeance(update.Message.Chat.Id).SendUpdate(update);
                    break;
                case UpdateType.CallbackQuery:
                    Seance.GetSeance(update.CallbackQuery.From.Id).SendUpdate(update);
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

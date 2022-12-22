using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Org.BouncyCastle.Asn1.Cms;
using System;

namespace CryptoBotProject.Bot
{
    internal class TelegramMessageReader
    {
        static TelegramMessageReader instance;
        public static TelegramMessageReader Instance
        {
            get
            {
                instance ??= new TelegramMessageReader();
                return instance;
            }
        }

        TelegramBotClient botClient;


        private TelegramMessageReader()
        {

        }

        public static void Start(string token = "1706882056:AAFfk4F_ZWZ3_h1Mx44SimSO_5JxQHAIKRM")//TODO: REMOVE TOKEN
        {
            Instance.botClient = new TelegramBotClient(token);
            Instance.Start();
        }

        private void Start()
        {

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            Instance.botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        public static void Stop()
        {
            Instance.botClient.CloseAsync().Wait();
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not Message message)
                return;

            // Only process text messages
            //if (message.Text is not string messageText)
            //    return;

            //Answer();

            //async void Answer()
            //{
            //    var chatId = message.Chat.Id;

            //    string sendingText = "ГОВОРИ НОРМАЛЬНО БЛЯДЪ, У ТЕБЯ НЕТ ВЫБОРА, ПИШИ КОММАНДУ /next";

            //    if (messageText == "/next")
            //    {
            //        sendingText = "Следующий великий философ:\n" + Philosopher[index++ % Philosopher.Length];
            //    }
            //    if (messageText == "/freaze")
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(5));
            //    }

            //    Console.WriteLine($"Received a '{message}' message in chat {chatId}, with user: {message.Chat.FirstName + " " + message.Chat.LastName}, userId {message.Chat.Username}. Sended {sendingText}");

            //    // Echo received message text
            //    Message sentMessage = await botClient.SendTextMessageAsync(
            //        chatId: chatId,
            //        text: sendingText,
            //        cancellationToken: cancellationToken);
            //}
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
        }
    }
}

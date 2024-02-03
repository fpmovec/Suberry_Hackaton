using System;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Syberry.Telegram
{
    public static class Bot
    {
        private static ITelegramBotClient _botClient;
        private static ReceiverOptions _receiverOptions;

        public static async Task StartBotAsync()
        {
            _botClient = new TelegramBotClient("6633951609:AAGBsXaBiH30xEGw7KjLWkOss4AqSgvagDI"); 
            _receiverOptions = new ReceiverOptions 
            {
                AllowedUpdates = new[] 
            {
                UpdateType.Message, 
            },

                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            // UpdateHander - обработчик приходящих Update`ов
            // ErrorHandler - обработчик ошибок, связанных с Bot API
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

            var me = await _botClient.GetMeAsync(); 
            Console.WriteLine($"{me.FirstName} запущен!");

            await Task.Delay(-1);
        }

        public   static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
            try
            {
                // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            // эта переменная будет содержать в себе все связанное с сообщениями
                            var message = update.Message;

                            // From - это от кого пришло сообщение (или любой другой Update)
                            var user = message.From;

                            // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                            // Chat - содержит всю информацию о чате
                            var chat = message.Chat;
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                message.Text, // отправляем то, что написал пользователь
                                replyToMessageId: message.MessageId // по желанию можем поставить этот параметр, отвечающий за "ответ" на сообщение
                                );

                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

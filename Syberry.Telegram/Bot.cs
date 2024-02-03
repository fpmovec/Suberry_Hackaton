using Syberry.Web.Services.Abstractions;
using Syberry.Web.Services.Implementations;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Syberry.Telegram
{
    public class Bot
    {
        private static ITelegramBotClient _botClient;

        private readonly IBelarusBankService _belarusBankService;

        public Bot(IBelarusBankService belarusBankService)
        {
            _belarusBankService = belarusBankService;
        }

        private static ReceiverOptions _receiverOptions;

        private const string _botToken = "6633951609:AAGBsXaBiH30xEGw7KjLWkOss4AqSgvagDI";

        private static string actualBank = string.Empty;

        private static string actualCurrency = string.Empty;

        public static async Task StartBotAsync()
        {
            _botClient = new TelegramBotClient(_botToken);
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                },
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} запущен!");

            await Task.Delay(-1);
        }

        public static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
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
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var message = update.Message;
                        var user = message.From;

                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        var chat = message.Chat;

                        if (message.Text.ToLower() == "/start")
                        {
                            var replyMarkup = new ReplyKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                new KeyboardButton("Национальный банк"),
                                new KeyboardButton("Альфабанк"),
                                new KeyboardButton("Беларусбанк")
                            }
                        });

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Привет! Чтобы воспользоваться функциями бота, сперва выбери банк из меню снизу.",
                                replyMarkup: replyMarkup
                            );
                        }
                        else if (message.Text.ToLower() == "национальный банк" ||
                                 message.Text.ToLower() == "альфабанк" ||
                                 message.Text.ToLower() == "беларусбанк")
                        {
                            actualBank = message.Text;

                            _belarusBankService.GetBelarusBankRatesAsync

                            var replyMarkup = new ReplyKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                new KeyboardButton("USD"),
                                new KeyboardButton("EUR"),
                                new KeyboardButton("GBP"),
                                new KeyboardButton("JPY")
                            }
                        });

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Выбран банк: {actualBank}.",
                                replyMarkup: replyMarkup
                            );
                        }
                        else if (message.Text.ToLower() == "usd" ||
                            message.Text.ToLower() == "eur" ||
                            message.Text.ToLower() == "gbp" ||
                            message.Text.ToLower() == "jpy")
                        {
                            actualCurrency = message.Text;

                            var replyMarkup = new ReplyKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                new KeyboardButton("USD"),
                                new KeyboardButton("EUR"),
                                new KeyboardButton("GBP"),
                                new KeyboardButton("JPY")
                            }
                        });

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Выбран банк: {actualBank}. Выбрана валюта: {actualCurrency}",
                                replyMarkup: replyMarkup
                            );
                        }

                        else
                        {
                            string[] parts = message.Text.Split(' ');

                            if (parts.Length == 1)
                            {
                                actualBank = parts[0];

                                Console.WriteLine($"Текущий банк: {actualBank}");
                            }

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                message.Text,
                                replyToMessageId: message.MessageId
                            );
                        }

                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    private static async Task SendCurrencySelectionKeyboardAsync(long chatId)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                                new[]
                                {
                                    new KeyboardButton("Национальный банк"),
                                    new KeyboardButton("Альфабанк"),
                                    new KeyboardButton("Беларусбанк")
                                }
                            });

            await _botClient.SendTextMessageAsync(chatId, "Выберите валюту:", replyMarkup: replyMarkup);
        }
    }
}

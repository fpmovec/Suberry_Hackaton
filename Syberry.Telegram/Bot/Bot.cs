using Microsoft.Extensions.DependencyInjection;
using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Syberry.Telegram.Bot
{
    public class Bot
    {
        private ITelegramBotClient _botClient;

        private ReceiverOptions _receiverOptions;

        private const string _botToken = "6633951609:AAGBsXaBiH30xEGw7KjLWkOss4AqSgvagDI";

        private string actualBank = string.Empty;

        private string actualCurrency = string.Empty;

        private readonly IAlpfaBankService _alpfaBankService;

        private readonly IBelarusBankService _belarusBankService;

        private readonly INationalBankService _nationalBankService;

        public Bot(IServiceProvider serviceProvider)
        {
            _belarusBankService = serviceProvider.GetRequiredService<IBelarusBankService>();
            _alpfaBankService = serviceProvider.GetRequiredService<IAlpfaBankService>();
            _nationalBankService = serviceProvider.GetRequiredService<INationalBankService>();
        }
        public async Task StartBotAsync()
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

        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                            await SendBankSelectionKeyboardAsync(botClient, chat.Id);
                        }

                        else if (message.Text.ToLower() == "национальный банк" ||
                                 message.Text.ToLower() == "альфабанк" ||
                                 message.Text.ToLower() == "беларусбанк")
                        {
                            switch (message.Text.ToLower())
                            {
                                case "национальный банк":
                                    actualBank = "National Bank";
                                    break;
                                case "альфабанк":
                                    actualBank = "Alfa Bank";
                                    break;
                                case "беларусбанк":
                                    actualBank = "Belarus Bank";
                                    break;
                                default:
                                    return;
                            }

                            await SendCurrencySelectionKeyboardAsync(botClient, chat.Id, actualBank);
                        }

                        else if (actualBank != string.Empty && (message.Text.ToLower() == "usd" ||
                            message.Text.ToLower() == "eur" ||
                            message.Text.ToLower() == "gbp" ||
                            message.Text.ToLower() == "jpy"))
                        {
                            actualCurrency = message.Text.ToUpper();

                            await SendActionSelectionKeyboardAsync(botClient, chat.Id, actualBank, actualCurrency);
                        }


                        else if (actualBank != string.Empty && actualCurrency != string.Empty &&
                            message.Text.ToLower() == "курс на текущий день")
                        {
                            Bank info = null;

                            switch (actualBank)
                            {
                                case "National Bank":
                                    info = await _nationalBankService.GetNationalBankAsync();
                                    break;
                                case "Alfa Bank":
                                    info = await _alpfaBankService.AlpfaBankRates();
                                    break;
                                case "Belarus Bank":
                                    info = await _belarusBankService.BelarusBankRates();
                                    break;
                                default:
                                    return;
                            }

                            if (info != null)
                            {
                                var sellInfo = info.Rates.FirstOrDefault(x => x.KursDateTime == DateTime.Today
                                    && x.Name == actualCurrency).SellRate;

                                var buyInfo = info.Rates.FirstOrDefault(x => x.KursDateTime == DateTime.Today
                                    && x.Name == actualCurrency).BuyRate;

                                if (sellInfo != null || buyInfo != null)
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        $"{actualBank} - {actualCurrency} на {DateTime.Today}"
                                    );

                                    await botClient.SendTextMessageAsync(
                                    chat.Id,
                                        $"Курс на покупку: {buyInfo}, курс на продажу: {sellInfo}."
                                    );
                                }
                                
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                    chat.Id,
                                        $"Курс не найден"
                                    );
                                }
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Курс не найден"
                                );
                            }

                        }

                        else if (actualBank != string.Empty && actualCurrency != string.Empty &&
                            message.Text.ToLower() == "курс на выбранный день")
                        {


                        }

                        else if (actualBank != string.Empty && actualCurrency != string.Empty &&
                            message.Text.ToLower() == "собрать статистику")
                        {
                            GenerateCurrencyChart();

                            using var stream = new FileStream("stat.png", FileMode.Open, FileAccess.Read);

                            await botClient.SendPhotoAsync(chat.Id, InputFile.FromStream(stream));
                        }

                        else if (actualBank != string.Empty && actualCurrency != string.Empty &&
                            message.Text.ToLower() == "выбрать другой банк")
                        {
                            await SendBankSelectionKeyboardAsync(botClient, chat.Id);
                        }

                        else if (actualBank != string.Empty && actualCurrency != string.Empty &&
                            message.Text.ToLower() == "выбрать другую валюту")
                        {
                            await SendCurrencySelectionKeyboardAsync(botClient, chat.Id, actualBank);
                        }

                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Ошибка: неизвестная команда. \n Используйте команду /start",
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

        private void GenerateCurrencyChart()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };

            ScottPlot.Plot myPlot = new();
            myPlot.Add.Scatter(dataX, dataY);

            myPlot.SavePng("stat.png", 400, 300);
        }

        public async Task SendBankSelectionKeyboardAsync(ITelegramBotClient bot, long chatId)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Национальный банк"),
                },
                 new[]
                {
                    new KeyboardButton("Альфабанк"),
                },
                 new[]
                {
                    new KeyboardButton("Беларусбанк")
                },
            })
            {
                ResizeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId,
                "Выбери банк из меню снизу.",
                replyMarkup: replyMarkup
            );
        }

        public async Task SendCurrencySelectionKeyboardAsync(ITelegramBotClient bot, long chatId, string bankName)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("USD"),
                    new KeyboardButton("EUR"),
                    new KeyboardButton("GBP"),
                    new KeyboardButton("JPY")
                }
           })
            {
                ResizeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId,
                $"Выбран банк: {bankName}.",
                replyMarkup: replyMarkup
            );
        }


        public async Task SendActionSelectionKeyboardAsync(ITelegramBotClient bot, long chatId, string bank, string currency)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Курс на текущий день"),
                },
                new[]
                {
                    new KeyboardButton("Собрать статистику"),
                },
                new[]
                {
                    new KeyboardButton("Выбрать другой банк"),
                },
                new[]
                {
                    new KeyboardButton("Выбрать другую валюту")
                },
            })
            {
                ResizeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId,
                $"Выбран банк: {bank}. Выбрана валюта: {currency}",
                replyMarkup: replyMarkup
            );
        }


        public Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
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

    }
}

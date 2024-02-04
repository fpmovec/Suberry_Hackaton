using Microsoft.Extensions.DependencyInjection;
using Syberry.Telegram.Bot;
using Syberry.Web.Services.Abstractions;
using Syberry.Web.Services.Implementations;

//Rate banksInfo =
////    await ApiService.GetBanksInfo("BelarusBank", "USD", DateTime.Today);

//Console.WriteLine($"{banksInfo.SellRate} {banksInfo.BuyRate} {DateTime.Today}");   

var services = new ServiceCollection();

services.AddScoped<IBelarusBankService, BelarusBankService>();
services.AddScoped<IAlpfaBankService, AlpfaBankService>();
services.AddScoped<INationalBankService, NationalBankService>();

services.AddHttpClient<BelarusBankService>();
services.AddHttpClient<AlpfaBankService>();
services.AddHttpClient<NationalBankService>();

var serviceProvider = services.BuildServiceProvider();

Bot test = new(serviceProvider);

test.StartBotAsync();

Console.WriteLine("Press ENTER to stop the bot");

Console.ReadLine();
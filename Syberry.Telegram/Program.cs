using Syberry.Telegram;
using System.Threading.Channels;

Bot test = new();

//test.StartBotAsync();

Console.WriteLine("Press ENTER to stop the bot");

Rate banksInfo =
    await ApiService.GetBanksInfo("BelarusBank", "USD", DateTime.Today);

Console.WriteLine($"{banksInfo.SellRate} {banksInfo.BuyRate} {DateTime.Today}");   

Console.ReadLine();
using Syberry.Telegram;
using System.Threading.Channels;

Bot test = new();

//test.StartBotAsync();

Console.WriteLine("Press ENTER to stop the bot");

Console.WriteLine(await ApiService.GetBanksInfo());   

Console.ReadLine();
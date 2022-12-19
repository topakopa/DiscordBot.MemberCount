using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBot.MemberCount;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Channels;

public class Program
{
    public static void Main()
    {
        var bot = new Bot();

        var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //if (!File.Exists(Path.Combine(appDir, guildInfoFile)))
        //{
        //    File.WriteAllText(Path.Combine(appDir, guildInfoFile), "");
        //}

        bot.Start().Wait(); //Запуск

        Task.Delay(-1).Wait(); //Блокиратор консоли
    }
}


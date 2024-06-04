using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.MemberCount
{
    public class Bot
    {
        private const string token = "bot-token";
        public static DiscordSocketClient _client;
        
        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
                AlwaysDownloadUsers = true,


            });

            //Инициализация
            _client.SetGameAsync("салки с калькулятором");
            //_client.Ready += Client_Ready;
            //_client.SlashCommandExecuted += SlashCommandHandler;
            _client.UserJoined += UserHandler.UserJoined;
            _client.UserLeft += UserHandler.UserLeft;
            //TODO: Установить защиту на канал

            //_client.Log += program.Log;
        }
        /// <summary>
        /// Запуск
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
        }



        [Obsolete]
        public async Task Client_Ready()
        {
            // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
            ulong guildId = 977995577419776000; //Сервер
            var guild = _client.GetGuild(guildId);

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var reboot = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            reboot.WithName("reboot");

            // Descriptions can have a max length of 100.
            reboot.WithDescription("Для первого запуска и/или перезагрузки");

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var rename = new SlashCommandBuilder()
                .WithName("rename")
                .WithDescription("Переименовать канал")
                .AddOption("name", ApplicationCommandOptionType.String, "Догадайтесь с трёх раз что делает этот параметр", true);


            try
            {
                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
                await guild.CreateApplicationCommandAsync(reboot.Build());
                await guild.CreateApplicationCommandAsync(rename.Build());
            }
            catch (ApplicationCommandException exception)
            {
                Console.WriteLine(exception);
            }


        }
        

    }
}

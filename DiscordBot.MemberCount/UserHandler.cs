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
    public static class UserHandler
    {
        static string guildInfoFile = "guildInfo.json";
        /// <summary>
        /// Обработчик при выходе
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public static async Task UserLeft(SocketGuild arg1, SocketUser arg2)
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


            if (File.Exists(Path.Combine(appDir, guildInfoFile)))
            {
                using (var sr = new StreamReader(Path.Combine(appDir, guildInfoFile)))
                {
                    var text = sr.ReadToEnd();
                    var guildInfos = JsonConvert.DeserializeObject<List<DiscordBot.MemberCount.GuildInfo>>(text);
                    var guild = guildInfos.First(x => x.GuildId == arg1.Id);
                    var chanell = arg1.Channels.FirstOrDefault(x => x.Id == guild.ChannelId);

                    var users = arg1.GetUsersAsync().ToArrayAsync().Result[0];
                    var botCount = users.Where(x => x.IsBot).Count();
                    var userCount = users.Count - botCount;

                    if (chanell != null)
                    {
                        await chanell.ModifyAsync(x =>
                        x.Name = guild.ChannelName.Replace("{count}", userCount.ToString()));
                    }

                }
            }
        }
        /// <summary>
        /// Обработчик при входе
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static async Task UserJoined(SocketGuildUser arg)
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (File.Exists(Path.Combine(appDir, guildInfoFile)))
            {
                using (var sr = new StreamReader(Path.Combine(appDir, guildInfoFile)))
                {
                    var text = sr.ReadToEnd();
                    var guildInfos = JsonConvert.DeserializeObject<List<DiscordBot.MemberCount.GuildInfo>>(text);
                    var guild = guildInfos.First(x => x.GuildId == arg.Guild.Id);
                    var chanell = arg.Guild.Channels.FirstOrDefault(x => x.Id == guild.ChannelId);

                    var users = arg.Guild.GetUsersAsync().ToArrayAsync().Result[0];
                    var botCount = users.Where(x => x.IsBot).Count();
                    var userCount = users.Count - botCount;

                    if (chanell != null)
                    {
                        await chanell.ModifyAsync(x =>
                        x.Name = guild.ChannelName.Replace("{count}", userCount.ToString()));
                    }

                }
            }
        }
    }
}

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
    public static class SlashCommandHandler
    {
        public static async Task Handle(SocketSlashCommand command)
        {

            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            List<DiscordBot.MemberCount.GuildInfo> guildInfos;
            using (var sr = new StreamReader(Path.Combine(appDir, GuildInfoStorage.fileName)))
            {
                var text = sr.ReadToEnd();
                guildInfos = JsonConvert.DeserializeObject<List<DiscordBot.MemberCount.GuildInfo>>(text);

            }


            var guildInfo = guildInfos.First(x => x.GuildId == command.GuildId);
            var chanell = command.Channel;
            var guild = Bot._client.GetGuild(command.GuildId.Value);
            switch (command.Data.Name)
            {
                case "reboot":


                    if (File.Exists(Path.Combine(appDir, "counters.txt")))
                    {
                        using (var sr = new StreamReader(Path.Combine(appDir, "counters.txt")))
                        {
                            var chID = sr.ReadLine();
                            var chanellBot = guild.Channels.FirstOrDefault(x => x.Id.ToString() == chID);
                            if (chanellBot != null) { await chanellBot.DeleteAsync(); }

                        }
                    }

                    string templateName = "Member Count: {count}";
                    if (File.Exists(Path.Combine(appDir, GuildInfoStorage.fileName)))
                    {
                        File.WriteAllText(Path.Combine(appDir, GuildInfoStorage.fileName), templateName);
                    }

                    var users = guild.GetUsersAsync().ToArrayAsync().Result[0];
                    var botCount = users.Where(x => x.IsBot).Count();
                    var userCount = users.Count - botCount;
                    var vChan = await guild.CreateVoiceChannelAsync($"Member Count: {userCount}"); // Вроде создание канала


                    using (var sw = new StreamWriter(Path.Combine(appDir, "counters.txt")))
                    {
                        sw.WriteLine(vChan.Id);
                    }

                    await command.RespondAsync($@"Бот был сброшен до стандартных настроек. Создан новый канал: ""{vChan.Name}""", ephemeral: true);
                    break;

                case "rename":
                    string name = (string)command.Data.Options.First().Value;
                    if (!name.Contains("{count}")) { await command.RespondAsync("Ты о счётчике забыл дурень! Пример: Кол-во участников {count}", ephemeral: true); break; }

                    users = guild.GetUsersAsync().ToArrayAsync().Result[0];
                    botCount = users.Where(x => x.IsBot).Count();
                    userCount = users.Count - botCount;

                    if (File.Exists(Path.Combine(appDir, "counters.txt")))
                    {
                        using (var sr = new StreamReader(Path.Combine(appDir, "counters.txt")))
                        {
                            var chID = sr.ReadLine();
                            var chanellBot = guild.Channels.FirstOrDefault(x => x.Id.ToString() == chID);
                            if (chanellBot != null)
                            {
                                await chanellBot.ModifyAsync(x =>
                                x.Name = name.Replace("{count}", userCount.ToString()));
                            }
                        }
                        if (File.Exists(Path.Combine(appDir, GuildInfoStorage.fileName)))
                        {
                            File.WriteAllText(Path.Combine(appDir, GuildInfoStorage.fileName), name);
                        }
                    }

                    await command.RespondAsync($@"Канал переименован в: ""{name}""", ephemeral: true);
                    break;
            }
        }
    }


}

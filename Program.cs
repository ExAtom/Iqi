using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Iqi.Commands;
using Iqi.Json;

namespace Iqi
{
    public class Recieved
    {
        public static SocketMessage Message;
        public static DateTime PingTime;
    }

    class Program
    {
        public static DiscordSocketClient _client;
        static void Main() => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += MessageHandler;
            _client.Log += Log;
            var token = BaseConfig.GetConfig().Token;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task MessageHandler(SocketMessage message)
        {
            string firstWord = message.Content.Split()[0];
            bool pong = message.Author.Id == BaseConfig.GetConfig().BotID && firstWord == "Pinging...";

            if (pong || !message.Author.IsBot)
                Recieved.Message = message;
            else
                return Task.CompletedTask;
            if (pong)
                Ping.DoCommand(true);
            if (!message.Content.StartsWith(BaseConfig.GetConfig().Prefix) || message.Author.IsBot)
                return Task.CompletedTask;

            string command = firstWord.Substring(1, firstWord.Length - 1).ToLower();

            // Commands
            if (Commands.Commands.Aliases.Contains(command) && BotChannel())
                Commands.Commands.DoCommand();
            if (Ping.Aliases.Contains(command) && BotChannel())
                Ping.DoCommand(false);
            if (Restart.Aliases.Contains(command) && BotChannel() && IsAdmin(true))
                Restart.DoCommand();
            if (Test.Aliases.Contains(command) && BotChannel() && IsAdmin(true))
                Test.DoCommand();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Parancs futtatásának logolása
        /// </summary>
        /// <returns></returns>
        public async static Task LogCommandRun()
        {
            var message = Recieved.Message;
            Console.Write(DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss") + " ");
            string output = $"Command run - {message.Author.Username}#{message.Author.Discriminator} in #{message.Channel}: {message.Content}";
            foreach (var id in BaseConfig.GetConfig().Terminals)
                try { await ((IMessageChannel)_client.GetChannel(id)).SendMessageAsync(output, allowedMentions: AllowedMentions.None); }
                catch (Exception) { }
            Console.WriteLine(output);
        }
        /// <summary>
        /// Ellenőrzi, hogy az üzenetküldő Admin-e.
        /// </summary>
        /// <param name="isOwner">Admin helyett tulajdonost ellenőriz.</param>
        /// <returns></returns>
        public static bool IsAdmin(bool isOwner)
        {
            foreach (var role in (Recieved.Message.Author as SocketGuildUser).Roles)
                if ((role.Permissions.Administrator && !isOwner) ||
                    BaseConfig.GetConfig().OwnerID == role.Id)
                    return true;
            return false;
        }
        /// <summary>
        /// Ellenőrzi, hogy az üzenet bot szobába volt-e küldve.
        /// </summary>
        /// <returns></returns>
        public static bool BotChannel()
        {
            var message = Recieved.Message;
            ulong serverId = ((SocketGuildChannel)message.Channel).Guild.Id;
            var servers = Servers.PullData();
            if (servers.Count(x => x.ID == serverId) == 0)
                return true;
            var server = servers[servers.IndexOf(servers.Find(x => x.ID == serverId))];
            if (server.BotChannels.Contains(message.Channel.Id) || server.BotChannels.Contains(0))
                return true;
            return false;
        }
    }
}

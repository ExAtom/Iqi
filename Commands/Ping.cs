using System;
using Discord;

namespace Iqi.Commands
{
    class Ping
    {
        public static string[] Aliases =
        {
            "ping",
            "latency"
        };
        public static string Description = "Calculates the bot's ping.";
        public static string[] Usages = { ".ping" };
        public static string Permission = "Anyone can use it.";
        public static string Trello = "https://trello.com/c/2wXQDMBX/31-ping";

        public static async void DoCommand(bool isResponse)
        {
            var message = Recieved.Message;
            if (message.Content.Split().Length > 1)
                return;

            if (isResponse)
            {
                var latency = DateTime.Now - Recieved.PingTime;
                await ((IUserMessage)message).ModifyAsync(m => m.Content = $"Pong! `{latency.TotalMilliseconds:f0}ms`");
            }
            else
            {
                await Program.LogCommandRun();
                Recieved.PingTime = DateTime.Now;
                await message.Channel.SendMessageAsync($"Pinging...");
            }
        }
    }
}

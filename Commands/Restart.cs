using System;
using System.Diagnostics;

namespace Iqi.Commands
{
    class Restart
    {
        public static string[] Aliases = { "restart" };
        public static string Description = "Restart or shutdown the bot. Updates from the Github repository.";
        public static string[] Usages = { ".restart [option]" };
        public static string Permission = "Only the owner can use it.";

        public async static void DoCommand()
        {
            await Program.LogCommandRun();

            var message = Recieved.Message;
            try
            {
                if (message.Content.Split()[1] == "exit")
                {
                    await message.Channel.SendMessageAsync("Shutting down...");
                    Environment.Exit(0);
                }
            }
            catch (Exception) { }
            try
            {
                await message.Channel.SendMessageAsync("Restarting bot... (This may take a few moments)");
                string commands =
                    "cd ..\n" +
                    "git pull\n" +
                    "dotnet build -o build\n" +
                    "cd build\n" +
                    "dotnet Iqi.dll";
                var process = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{commands}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                Process.Start(process);
                Environment.Exit(0);
            }
            catch (Exception) { await message.Channel.SendMessageAsync("❌ Can't find bash!"); }
        }
    }
}

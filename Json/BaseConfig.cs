using System.IO;
using System.Text.Json;

namespace Iqi.Json
{
    /// <summary>
    /// A Bot működéséhez elengedhetetlen configok.
    /// <para>Token, Prefix, Roles, Channels</para>
    /// </summary>
    class BaseConfig
    {
        public string Token { get; set; }
        public char Prefix { get; set; }
        public ulong BotID { get; set; }
        public ulong OwnerID { get; set; }
        public ulong[] Terminals { get; set; }

        /// <summary>
        /// BaseConfig adatainak a lekérése.
        /// </summary>
        /// <returns></returns>
        public static BaseConfig GetConfig()
        {
            return JsonSerializer.Deserialize<BaseConfig>(File.ReadAllText("BaseConfig.json"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace Iqi.Json
{
    /// <summary>
    /// Szerverek beállításai.
    /// </summary>
    class Servers
    {
        public ulong ID { get; set; }
        public string PartnerText { get; set; }
        public ulong PartnerChannel { get; set; }
        public List<ulong> BotChannels { get; set; }

        /// <summary>
        /// Szerverek adatainak lekérése a Json-ből.
        /// </summary>
        /// <returns></returns>
        public static List<Servers> PullData()
        {
            try { return JsonSerializer.Deserialize<List<Servers>>(File.ReadAllText("Servers.json")); }
            catch (Exception) { File.WriteAllText("Servers.json", "[]"); }
            return JsonSerializer.Deserialize<List<Servers>>(File.ReadAllText("Servers.json"));
        }
        /// <summary>
        /// Szerverek adatai feltöltése a Json-be.
        /// </summary>
        /// <param name="list"></param>
        public static void PushData(List<Servers> list)
        {
            File.WriteAllText("Servers.json", JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>
        /// Ez a sztupid Json deserialize miatt kell. Használd a másik konstruktort!
        /// </summary>
        public Servers() { }

        /// <summary>
        /// Alap konstruktor az összes, kötelező adattal.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerText"></param>
        /// <param name="botChannels"></param>
        public Servers(ulong id, string partnerText, ulong partnerChannel, List<ulong> botChannels)
        {
            ID = id;
            PartnerText = partnerText;
            PartnerChannel = partnerChannel;
            BotChannels = botChannels;
        }
    }
}

using GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities
{
    static public class Utility
    {
        static Random rand = new Random();

        public static int NextRandom()
        {
            return rand.Next();
        }

        public static int NextRandom(int min, int max)
        {
            return rand.Next(min,max);
        }

        public static List<PlayerData> CreatePlayerContext()
        {
            return File.ReadAllLines(@"ServerContent\random Names with scores.csv")
                                           .Select(v => FromCsv(v))
                                           .ToList();
        }

        public static PlayerData FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            PlayerData playerData = new PlayerData();
            playerData.playerID = Guid.NewGuid().ToString();
            playerData.PlayerName = values[0] + " " + values[1];
            playerData.GamerTag = values[2];
            playerData.XP= Int32.Parse(values[3]);
            playerData.Password = "password";
            return playerData;
        }
    }
}

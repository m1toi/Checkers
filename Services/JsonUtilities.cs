using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Checkers.Models;

namespace Checkers.Services
{
    public class JsonUtilities
    {
        public static void SerializeGameConfiguration(GameConfiguration gameConfiguration, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(gameConfiguration, options);
            File.WriteAllText(filePath, json);
        }
        public static GameConfiguration DeserializeGameConfiguration(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameConfiguration>(json);
        }
        
    }
}

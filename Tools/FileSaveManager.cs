using System.IO;
using System.Text.Json;

namespace ForgeBot.Tools
{
    public class FileSaveManager
    {
        public static void SaveFile(string address, object saveData)
        {
            string directory = Path.GetDirectoryName(address);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            FileStream file = File.Create(address);
            JsonSerializer.Serialize(file, saveData);
            file.Close();
        }
        public static T LoadFile<T>(string address)
        {
            if (!File.Exists(address))
            {
                throw new FileNotFoundException($"File not found at {address}");
            }

            FileStream file = File.OpenRead(address);
            var loadedResources = JsonSerializer.Deserialize<T>(file);
            file.Close();
            return loadedResources;
        }
    }
}

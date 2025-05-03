using System;
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
            string directoryPath = Path.GetDirectoryName(address);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!File.Exists(address))
                File.WriteAllText(address, "{}");
            else if (string.IsNullOrEmpty(File.ReadAllText(address)))
                File.WriteAllText(address, "{}");

            using (FileStream file = File.OpenRead(address))
            {
                var loadedResources = JsonSerializer.Deserialize<T>(file);
                return loadedResources ?? Activator.CreateInstance<T>();
            }
        }
    }
}

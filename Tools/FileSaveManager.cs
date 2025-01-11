using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace ForgeBot.Tools
{
    public class FileSaveManager
    {
        public static void SaveFile(string address, object saveData)
        {
            FileStream file = File.Create(address);
            JsonSerializer.Serialize(file, saveData);
            file.Close();
        }
    }
}

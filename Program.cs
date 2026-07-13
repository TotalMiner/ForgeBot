using ForgeBot.Tools;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace ForgeBot
{
    class Program
    {
        private readonly static string _propertiesVersionParam = "version=";
        private readonly static string _tokenParam = "token=";
        private readonly static string _prefixParam = "prefix=";
        private readonly static string _activityParam = "activity=";
        private readonly static string _imageLinkParam = "imageLink=";
        private readonly static string _itemDataLinkParam = "itemDataLink=";
        private readonly static string _blueprintDataLinkParam = "blueprintDataLink=";
        private readonly static string _scriptsDataLinkParam = "scriptsDataLink=";
        private readonly static string _debugServerParam = "debugServer=";
        private readonly static string _debugChannelParam = "debugChannel=";

        // Default values for properties
        private readonly static string _propertiesVersion = "1.0";
        private readonly static string _token = "REPLACEME";
        private readonly static string _prefix = "?";
        private readonly static string _activity = "Total Miner";
        private readonly static string _imageLink = "https://raw.githubusercontent.com/TotalMiner/ForgeBot-Resources/refs/heads/main/imgs";
        private readonly static string _itemDataLink = "https://raw.githubusercontent.com/TotalMiner/ForgeBot-Resources/refs/heads/main/data/ItemData.xml";
        private readonly static string _blueprintDataLink = "https://raw.githubusercontent.com/TotalMiner/ForgeBot-Resources/refs/heads/main/data/BlueprintData.xml";
        private readonly static string _scriptsDataLink = "https://raw.githubusercontent.com/TotalMiner/ForgeBot-Resources/refs/heads/main/data/ScriptCommands.xml";
        private readonly static string _debugServer = "0";
        private readonly static string _debugChannel = "0";

        static void Main(string[] args)
        {
            BotCore.WakeTime.Start();
            Console.WriteLine("### Starting Program");
            if (args.Length > 0)
            {
                string programCommands = "";
                foreach (string arg in args)
                {
                    programCommands += $" ( {arg} ) ";
                    if (arg == "-v")
                    {
                        BotCore.IsVerbose = true;
                    }
                }
                Console.WriteLine($"### Args: \"[ {programCommands} ]\"");
            }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            LoadBot();
        }

        public static void LoadProperties(out bool success, out string token, out string prefix, out string activity)
        {
            success = false;
            token = null;
            prefix = null;
            activity = null;

            if (!Directory.Exists("Resources"))
            {
                Console.WriteLine("!!! Resources folder not found. Creating a new one.");
                Directory.CreateDirectory("Resources");
            }

            if (!File.Exists("Resources/properties.txt"))
            {
                Console.WriteLine("!!! Properties file not found. Creating a new one. Please fill in the token and restart.");
                BuildPropFile(false);
                return;
            }

            string[] properties = File.ReadAllLines("Resources/properties.txt");

            if (!properties.Any(p => p.StartsWith(_propertiesVersionParam)))
            {
                Console.WriteLine("*** Properties file version not found. Creating a new one. Would you like to preserve existing settings? (Y/N)");
                bool preserveSettings = true;
                while (Environment.UserInteractive)
                {
                    char key = Console.ReadKey().KeyChar;
                    if (key is 'Y' or 'y')
                    {
                        preserveSettings = true;
                        Console.WriteLine();
                        break;
                    }
                    if (key is 'N' or 'n')
                    {
                        preserveSettings = false;
                        Console.WriteLine();
                        break;
                    }
                    Console.WriteLine("\n*** Invalid input. Please enter Yes( Y ) or No( N ).");
                }


                BuildPropFile(preserveSettings);

                properties = File.ReadAllLines("Resources/properties.txt");
            }
            else if (properties.First(p => p.StartsWith(_propertiesVersionParam)) != $"{_propertiesVersionParam}{_propertiesVersion}")
            {
                Console.WriteLine("*** Properties file version mismatch. Creating a new one. Would you like to preserve existing settings? (Y/N)");
                bool preserveSettings = true;
                while (Environment.UserInteractive)
                {
                    char key = Console.ReadKey().KeyChar;
                    if (key is 'Y' or 'y')
                    {
                        preserveSettings = true;
                        Console.WriteLine();
                        break;
                    }
                    if (key is 'N' or 'n')
                    {
                        preserveSettings = false;
                        Console.WriteLine();
                        break;
                    }
                    Console.WriteLine("\n*** Invalid input. Please enter Yes( Y ) or No( N ).");
                }
                BuildPropFile(preserveSettings);
                properties = File.ReadAllLines("Resources/properties.txt");
            }


            foreach (string prop in properties)
            {
                if (prop.StartsWith(_tokenParam))
                {
                    token = prop[_tokenParam.Length..];
                    if (token == _token)
                    {
                        Console.WriteLine("!!! Token not set in properties file. Please set the token in Resources/properties.txt");
                        return;
                    }
                }
                else if (prop.StartsWith(_prefixParam))
                {
                    prefix = prop[_prefixParam.Length..];
                }
                else if (prop.StartsWith(_activityParam))
                {
                    activity = prop[_activityParam.Length..];
                }
                else if (prop.StartsWith(_imageLinkParam))
                {
                    BotCore.ImagePath = prop[_imageLinkParam.Length..];
                }
                else if (prop.StartsWith(_itemDataLinkParam))
                {
                    BotCore.ItemDataPath = prop[_itemDataLinkParam.Length..];
                }
                else if (prop.StartsWith(_blueprintDataLinkParam))
                {
                    BotCore.BlueprintDataPath = prop[_blueprintDataLinkParam.Length..];
                }
                else if (prop.StartsWith(_scriptsDataLinkParam))
                {
                    BotCore.ScriptsDataPath = prop[_scriptsDataLinkParam.Length..];
                }
                else if (prop.StartsWith(_debugServerParam))
                {
                    BotCore.DebugGuildId = ulong.Parse(prop[_debugServerParam.Length..]);
                }
                else if (prop.StartsWith(_debugChannelParam))
                {
                    BotCore.DebugChannelId = ulong.Parse(prop[_debugChannelParam.Length..]);
                }
            }
            success = true;
        }

        public static void DownloadDataFiles()
        {
            Console.WriteLine("### Downloading Data Files...");

            string dataDir = "Resources";
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "ForgeBot-DataFetcher");

            DownloadSingleFile(client, BotCore.ItemDataPath, Path.Combine(dataDir, "ItemData.xml"));
            DownloadSingleFile(client, BotCore.BlueprintDataPath, Path.Combine(dataDir, "BlueprintData.xml"));
            DownloadSingleFile(client, BotCore.ScriptsDataPath, Path.Combine(dataDir, "ScriptCommands.xml"));
        }
        private static void DownloadSingleFile(HttpClient client, string url, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            try
            {
                Console.WriteLine($"### Checking: {Path.GetFileName(destinationPath)}");

                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                if (File.Exists(destinationPath))
                {
                    DateTime lastModifiedLocal = File.GetLastWriteTimeUtc(destinationPath);
                    request.Headers.IfModifiedSince = lastModifiedLocal;
                }

                HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    Console.WriteLine($"--- \"{Path.GetFileName(destinationPath)}\" File is up to date. Skipping download.");
                    return;
                }

                response.EnsureSuccessStatusCode();

                byte[] content = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                File.WriteAllBytes(destinationPath, content);

                if (response.Content.Headers.LastModified.HasValue)
                {
                    File.SetLastWriteTimeUtc(destinationPath, response.Content.Headers.LastModified.Value.UtcDateTime);
                }

                Console.WriteLine($"--- \"{Path.GetFileName(destinationPath)}\" Downloaded newer version successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! Failed to download {Path.GetFileName(destinationPath)}. Falling back to local file if available. Error: {ex.Message}");
            }
        }

        private static void BuildPropFile(bool preserveSettings)
        {
            if (!preserveSettings)
            {
                File.WriteAllText("Resources/properties.txt", $"version={_propertiesVersion}\n" +
                    $"token={_token}\n" +
                    $"prefix={_prefix}\n" +
                    $"activity={_activity}\n" +
                    $"imageLink={_imageLink}\n" +
                    $"itemDataLink={_itemDataLink}\n" +
                    $"blueprintDataLink={_blueprintDataLink}\n" +
                    $"scriptsDataLink={_scriptsDataLink} \n" +
                    $"debugServer={_debugServer}\n" +
                    $"debugChannel={_debugChannel}");
            }
            else
            {
                string propertiesRaw = File.ReadAllText("Resources/properties.txt");
                if (!propertiesRaw.Contains(_propertiesVersionParam))
                {
                    propertiesRaw += $"\nversion={_propertiesVersion}";
                }
                else if (propertiesRaw.Contains(_propertiesVersionParam))
                {
                    string[] lines = propertiesRaw.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    string oldVersionLine = lines.First(p => p.StartsWith(_propertiesVersionParam));

                    propertiesRaw = propertiesRaw.Replace(oldVersionLine, $"{_propertiesVersionParam}{_propertiesVersion}");
                }
                if (!propertiesRaw.Contains(_tokenParam))
                {
                    propertiesRaw += $"\n{_tokenParam}{_token}";
                }
                if (!propertiesRaw.Contains(_prefixParam))
                {
                    propertiesRaw += $"\n{_prefixParam}{_prefix}";
                }
                if (!propertiesRaw.Contains(_activityParam))
                {
                    propertiesRaw += $"\n{_activityParam}{_activity}";
                }
                if (!propertiesRaw.Contains(_imageLinkParam))
                {
                    propertiesRaw += $"\n{_imageLinkParam}{_imageLink}";
                }
                if (!propertiesRaw.Contains(_itemDataLinkParam))
                {
                    propertiesRaw += $"\n{_itemDataLinkParam}{_itemDataLink}";
                }
                if (!propertiesRaw.Contains(_blueprintDataLinkParam))
                {
                    propertiesRaw += $"\n{_blueprintDataLinkParam}{_blueprintDataLink}";
                }
                if (!propertiesRaw.Contains(_scriptsDataLinkParam))
                {
                    propertiesRaw += $"\n{_scriptsDataLinkParam}{_scriptsDataLink}";
                }
                if (!propertiesRaw.Contains(_debugServerParam))
                {
                    propertiesRaw += $"\n{_debugServerParam}{_debugServer}";
                }
                if (!propertiesRaw.Contains(_debugChannelParam))
                {
                    propertiesRaw += $"\n{_debugChannelParam}{_debugChannel}";
                }

                File.WriteAllText("Resources/properties.txt", propertiesRaw);
            }

        }

        private static void LoadBot()
        {
            Console.WriteLine("### Loading Properties");

            LoadProperties(out bool success, out string token, out string prefix, out string activity);

            if (!success)
            {
                Console.WriteLine("!!! Failed to load properties.");
                return;
            }

            DownloadDataFiles();

            Console.WriteLine("### Loading Item Data");
            GameItem.DeserializeItemData();
            Console.WriteLine("### Loading Script Data");
            ScriptCommand.DeserializeScriptCommands();
            Console.WriteLine("### Loading Permissions");
            _ = new Permissions();
            BotCore.MainAsync(token, prefix, activity).GetAwaiter().GetResult();
        }

        static void OnProcessExit(object sender, EventArgs e) => BotCore.ClosingClient().GetAwaiter().GetResult();
    }
}

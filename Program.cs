using ForgeBot.Tools;
using System;
using System.IO;

namespace ForgeBot
{
    class Program
    {
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
                        BotCore.IsVerbose = true;
                }
                Console.WriteLine($"### Args: \"[ {programCommands} ]\"");
            }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Console.WriteLine("### Loading Properties");
            string[] properties = File.ReadAllLines("Resources/properties.txt");
            string token = "";
            string prefix = "?";
            string activity = "";
            foreach (string prop in properties)
            {
                if (prop.StartsWith("token="))
                    token = prop.Remove(0, 6);
                else if (prop.StartsWith("prefix="))
                    prefix = prop.Remove(0, 7);
                else if (prop.StartsWith("activity="))
                    activity = prop.Remove(0, 9);
            }

            Console.WriteLine("### Loading Item Data");
            GameItem.DeserializeItemData();
            Console.WriteLine("### Loading Script Data");
            ScriptCommand.DeserializeScriptCommands();
            BotCore.MainAsync(token, prefix, activity).GetAwaiter().GetResult();
        }
        static void OnProcessExit(object sender, EventArgs e)
        {
            BotCore.ClosingClient();
        }
    }
}

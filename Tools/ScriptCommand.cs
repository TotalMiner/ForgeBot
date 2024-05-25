using StudioForge.TotalMiner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ForgeBot.Tools
{
    public static class ScriptCommand
    {
        public class Script
        {
            public string Name;
            public string Description;
        }

        public static List<Script> Scripts = new();
        public static List<string> ScriptNames = new();
        public static void DeserializeScriptCommands()
        {
                        XmlRootAttribute scriptRoot = new XmlRootAttribute
            {
                ElementName = "ArrayOfScript",
                IsNullable = true
            };
            XmlSerializer scriptXmlSerializer = new XmlSerializer(typeof(List<Script>), scriptRoot);
            TextReader scriptTextReader = new StreamReader("Resources/ScriptCommands.xml");
            Scripts = (List<Script>)scriptXmlSerializer.Deserialize(scriptTextReader);
            scriptTextReader.Close();
            foreach (Script script in Scripts)
            {
                script.Name = script.Name.Replace("\n", "").Replace(" ", "");
                ScriptNames.Add(script.Name);

                if (BotCore.IsVerbose)
                    Console.WriteLine($"*** Script : [{script.Name}]");
            }
        }
        public static Script GetScript(string name)
        {
            Script script;
            try
            {
                script = Scripts.Where(s => s.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }
            catch
            {
                script = null;
            }
            return script;
        }
    }
}

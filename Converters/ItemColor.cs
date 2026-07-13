using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ForgeBot.Converters
{
    public static class ItemColor
    {
        private static readonly Dictionary<string, DiscordColor> ColorKeywords;

        static ItemColor()
        {
            ColorKeywords = new Dictionary<string, DiscordColor>
            {
                { "greenstone", DiscordColor.PhthaloGreen },
                { "green", DiscordColor.PhthaloGreen },
                { "tan", DiscordColor.Wheat },
                { "creme", DiscordColor.Wheat },
                { "sledge", DiscordColor.Wheat },
                { "flour", DiscordColor.Wheat },
                { "dough", DiscordColor.Wheat },
                { "butter", DiscordColor.Wheat },
                { "wheat", DiscordColor.Wheat },
                { "seed", DiscordColor.Wheat },
                { "lime", DiscordColor.SpringGreen },
                { "iron", DiscordColor.VeryDarkGray },
                { "gray", DiscordColor.VeryDarkGray },
                { "wood", DiscordColor.Brown },
                { "bronze", DiscordColor.Brown },
                { "leather", DiscordColor.Brown },
                { "brown", DiscordColor.Brown },
                { "steel", DiscordColor.White },
                { "snow", DiscordColor.White },
                { "cloud", DiscordColor.White },
                { "troll", DiscordColor.White },
                { "tile", DiscordColor.White },
                { "white", DiscordColor.White },
                { "milk", DiscordColor.White },
                { "egg", DiscordColor.White },
                { "salt", DiscordColor.White },
                { "sugar", DiscordColor.White },
                { "ruby", DiscordColor.Sienna },
                { "red", DiscordColor.Sienna },
                { "tomato", DiscordColor.Sienna },
                { "apple", DiscordColor.Sienna },
                { "boom", DiscordColor.DarkButNotBlack },
                { "cherries", DiscordColor.Sienna },
                { "strawberries", DiscordColor.Sienna },
                { "titanium", DiscordColor.DarkButNotBlack },
                { "black", DiscordColor.DarkButNotBlack },
                { "spider", DiscordColor.DarkButNotBlack },
                { "blue", DiscordColor.Blue },
                { "rawfish", DiscordColor.Blue },
                { "pink", DiscordColor.HotPink },
                { "cherry", DiscordColor.HotPink },
                { "raw", DiscordColor.HotPink },
                { "cyan", DiscordColor.Cyan },
                { "diamantium", DiscordColor.Cyan },
                { "water", DiscordColor.Cyan },
                { "orange", DiscordColor.Orange },
                { "cooked", DiscordColor.Orange },
                { "potion", DiscordColor.MidnightBlue },
                { "obsidian", DiscordColor.Purple },
                { "purple", DiscordColor.Purple },
                { "staff", DiscordColor.Purple },
                { "gold", DiscordColor.Gold },
                { "key", DiscordColor.Gold },
                { "yellow", DiscordColor.Gold },
                { "lemon", DiscordColor.Gold },
                { "corn", DiscordColor.Gold },
                { "banana", DiscordColor.Gold },
                { "grapefruit", DiscordColor.Gold }
            };
        }

        static public DiscordColor GetItemColor(string itemID)
        {
            KeyValuePair<string, DiscordColor> match = ColorKeywords.FirstOrDefault(pair => itemID.Contains(pair.Key));

            return match.Key != null ? match.Value : DiscordColor.Azure;
        }
    }
}

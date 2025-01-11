using DSharpPlus.Entities;

namespace ForgeBot.Converters
{
    public static class ItemColor
    {
        // WARNING THIS IS REALLY UGLY
        static public DiscordColor GetItemColor(string itemID)
        {
            if (itemID.Contains("greenstone") || itemID.Contains("green"))
                return DiscordColor.PhthaloGreen;
            if (itemID.Contains("tan") || itemID.Contains("creme") || itemID.Contains("sledge") || itemID.Contains("flour") || itemID.Contains("dough") || itemID.Contains("butter") || itemID.Contains("wheat") || itemID.Contains("seed"))
                return DiscordColor.Wheat;
            if (itemID.Contains("lime"))
                return DiscordColor.SpringGreen;
            if (itemID.Contains("iron") || itemID.Contains("gray"))
                return DiscordColor.VeryDarkGray;
            if (itemID.Contains("wood") || itemID.Contains("bronze") || itemID.Contains("leather") || itemID.Contains("brown"))
                return DiscordColor.Brown;
            if (itemID.Contains("steel") || itemID.Contains("snow") || itemID.Contains("cloud") || itemID.Contains("troll") || itemID.Contains("tile") || itemID.Contains("white") || itemID.Contains("milk") || itemID.Contains("egg") || itemID.Contains("salt") || itemID.Contains("sugar"))
                return DiscordColor.White;
            if (itemID.Contains("ruby") || itemID.Contains("red") || itemID.Contains("tomato") || itemID.Contains("apple") || itemID.Contains("boom") || itemID.Contains("cherries") || itemID.Contains("strawberries"))
                return DiscordColor.Sienna;
            if (itemID.Contains("titanium") || itemID.Contains("black") || itemID.Contains("boom") || itemID.Contains("spider"))
                return DiscordColor.DarkButNotBlack;
            if (itemID.Contains("blue") || itemID.Contains("rawfish"))
                return DiscordColor.Blue;
            if (itemID.Contains("pink") || itemID.Contains("cherry") || itemID.Contains("raw"))
                return DiscordColor.HotPink;
            if (itemID.Contains("cyan") || itemID.Contains("diamantium") || itemID.Contains("water"))
                return DiscordColor.Cyan;
            if (itemID.Contains("orange") || itemID.Contains("cooked"))
                return DiscordColor.Orange;
            if (itemID.Contains("potion"))
                return DiscordColor.MidnightBlue;
            if (itemID.Contains("obsidian") || itemID.Contains("purple") || itemID.Contains("staff"))
                return DiscordColor.Purple;
            if (itemID.Contains("gold") || itemID.Contains("key") || itemID.Contains("yellow") || itemID.Contains("lemon") || itemID.Contains("corn") || itemID.Contains("banana") || itemID.Contains("grapefruit"))
                return DiscordColor.Gold;
            return DiscordColor.Azure;
        }
    }
}

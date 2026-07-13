namespace ForgeBot.Tools
{
    public static class StringBuilders
    {
        public static string GetItemIconLink(string itemID)
        {
            string imagelink = $"{BotCore.ImagePath}/{itemID}";
            switch (itemID)
            {
                default:
                    imagelink += ".png";
                    return imagelink;
                case "Furnace": case "StainedGlassPane": case "Stairs": case "Button": case "Switch": case "TrapDoor":
                    imagelink += ".gif";
                    return imagelink;
            }
        }
    }
}

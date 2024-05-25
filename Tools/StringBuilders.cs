using System;
using System.Collections.Generic;
using System.Text;

namespace ForgeBot.Tools
{
    public static class StringBuilders
    {
        public static string GetItemIconLink(string itemID)
        {
            string imagelink = $"https://synhayden.com/download/totalminer/imgs/{itemID}128x";
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

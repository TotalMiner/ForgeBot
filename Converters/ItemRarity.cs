using System;
using System.Collections.Generic;
using System.Text;

namespace ForgeBot.Converters
{
    public static class ItemRarity
    {
        public static string GetRarity(string itemID)
        {
            return itemID.ToLower() switch
            {
                "battleaxe" => ":small_blue_diamond:Rare",
                "elvenbow" => ":small_blue_diamond:Rare",
                "amuletofflight" => ":trident: Legendary",
                "watertalisman" => ":small_orange_diamond:Epic",
                "shieldbadge" => ":small_blue_diamond:Rare",
                "tenleagueboots" => ":small_orange_diamond:Epic",
                _ => ":white_small_square:Common",
            };
            }
    }
}

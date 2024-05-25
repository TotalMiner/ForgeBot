using StudioForge.TotalMiner;

namespace ForgeBot.Converters
{
    public static class ItemIDConverter
    {
        public static string ConvertBlockIDToItemID(string itemID)
        {
            return itemID.ToLower() switch
            {
                "fence" => "FenceIcon",
                "table" => "TableIcon",
                "halfblock" => "HalfBlockIcon",
                "halfblock2" => "HalfBlock2Icon",
                "stairs" => "StairsIcon",
                "stairs2" => "Stairs2Icon",
                "ramp" => "RampIcon",
                "ramp2" => "Ramp2Icon",
                "cylinder" => "CylinderIcon",
                "rope" => "RopeIcon",
                "sign" => "SignIcon",
                "switch" => "SwitchIcon",
                "button" => "ButtonIcon",
                "black" => "ColorBlack",
                "bloodorange" => "ColorBloodOrange",
                "blue" => "ColorBlue",
                "darkbrown" => "ColorDarkBrown",
                "darkgray" => "ColorDarkGray",
                "darkgreen" => "ColorDarkGreen",
                "darkred" => "ColorDarkRed",
                "gray" => "ColorGray",
                "green" => "ColorGreen",
                "lightblue" => "ColorLightBlue",
                "lightbrown" => "ColorLightBrown",
                "lightgreen" => "ColorLightGreen",
                "lightorange" => "ColorLightOrange",
                "lighttan" => "ColorLightTan",
                "lightyellow" => "ColorLightYellow",
                "limegreen" => "ColorLimeGreen",
                "pink" => "ColorPink",
                "purple" => "ColorPurple",
                "red" => "ColorRed",
                "smoothgray" => "ColorSmoothGray",
                "tan" => "ColorTan",
                "white" => "ColorWhite",
                "yellow" => "ColorYellow",
                "brown" => "ColorBrown",
                "creme" => "ColorCreme",
                "cyan" => "ColorCyan",
                "darkblue" => "DarkBlue",
                _ => itemID,
            };
        }
    }
}

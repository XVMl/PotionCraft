using System.Collections.Generic;
using static PotionCraft.Content.System.AutoLoaderSystem.JsonLoader;
namespace PotionCraft.Content.System.ColorfulText
{
    public class OperatorColorText
    {
        public static Dictionary<string, string> PurifyColor => ColorfulTexts["Purify"];

        public static Dictionary<string, string> MashUpColor =>ColorfulTexts["MashUp"];
        
        public static Dictionary<string, string> PotionColor => ColorfulTexts["BuffName"];

    }
}
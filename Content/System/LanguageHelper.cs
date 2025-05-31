using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PotionCraft.Content.System
{
    public class LanguageHelper
    {
        public static string TryGetLanguagValue(string path)
        {
            string lang = null;
            string key = "Mods.PotionCraft." + path.Replace(" ","");
            lang += Language.GetTextValue(key);
            if (lang.Equals(key))
            {
                return null;
            }
            return lang;
        }

        public static string ColorfulBuffName(string path)=>TryGetLanguagValue("BuffColors.TModLoader." + path);

        public static string ColorfulBuffName(int buffid) => TryGetLanguagValue("BuffColors.TModLoader." + Lang.GetBuffName(buffid));
    }
}

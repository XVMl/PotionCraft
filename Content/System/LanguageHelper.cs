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
        /// <summary>
        /// 尝试从.hjson获取文本，没有则返回空
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string TryGetLanguagValue(string path)
        {
            string lang = null;
            string key = "Mods.PotionCraft.Colorfulfont." + path.Replace(" ", "");
            lang += Language.GetTextValue(key);
            Main.NewText(lang);
            if (lang.Equals(key))
            {
                return null;
            }
            return lang;
        }

        public static string ColorfulFont(string path) => TryGetLanguagValue("Craftfont." + path);

        public static string ColorfulBuffName(string path)=>TryGetLanguagValue("BuffColors.TModLoader." + path);

        public static string ColorfulBuffName(int buffid) => TryGetLanguagValue("BuffColors.TModLoader." + Lang.GetBuffName(buffid));

    }
}

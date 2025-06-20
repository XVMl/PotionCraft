﻿using System;
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
            return lang.Equals(key) ? null : lang;
        }

        public static string ColorfulFont(string path) => TryGetLanguagValue("Craftfont." + path);

        public static string ColorfulBuffName(string path)=>TryGetLanguagValue("BuffColors.TModLoader." + path);

        public static string ColorfulBuffName(int buffid) => TryGetLanguagValue("BuffColors.TModLoader." + Lang.GetBuffName(buffid));

        public static List<string> GenerateRandomPostfix()
        {
            Random random = new Random();
            int numOperands = random.Next(3, 5);
            List<string> experssion = new() { "34" };

            for (int i = 0; i < numOperands; i++)
            {
                experssion.Add(random.Next(1, 100).ToString());
                experssion.Add("and");
            }

            int operrands = random.Next(2 * numOperands, 3 * numOperands);

            for (int i = 0; i < operrands; i++)
            {
                string oper = random.Next(0, 2) == 1 ? "purifying" : "boling";
                experssion.Insert(random.Next(1, experssion.Count + 1), oper);
            }
            return experssion;
        }

        public static string ConverToInfix(List<string> postlist)
        {
            Stack<string> stack = new();
            foreach (string token in postlist)
            {
                switch (token)
                {
                    case "purifying":
                    case "bowling":
                    {
                        string operand = stack.Pop();

                        bool needParens = operand.Contains("and") || operand.Contains("purifying") || operand.Contains("boling");

                        string expr = needParens ? $"({operand})" : operand;

                        stack.Push(token == "purifying" ? $"purifying({expr})" : $"{expr}bowling");
                        break;
                    }
                    case "and":
                        stack.Push($"{stack.Pop()} and {stack.Pop()}");
                        break;
                    default:
                        stack.Push(token);
                        break;
                }
            }
            return stack.Pop();
        }


    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static PotionCraft.Content.System.ColorfulText.OperatorColorText;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
using static PotionCraft.Content.System.AutoLoaderSystem.JsonLoader;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using System.Reflection;

namespace PotionCraft.Content.System
{
    public class LanguageHelper
    {
        public static readonly Color Deafult = new(215, 193, 147);

        public static readonly string Deafult_Hex = "[c/D7C193:]";

        public static float Scale_location
        {
            get
            {
                return LanguageManager.Instance.ActiveCulture.Name switch
                {
                    "zhn-Hans" => 1f,
                    "en-US" => .85f,
                    _ => .8f
                };
            }
        }
        /// <summary>
        /// 尝试从.hjson获取文本，没有则返回空
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string TryGetLanguagValue(string path)
        {
            string lang = null;
            string key = "Mods.PotionCraft." + path.Replace(" ", "");
            lang += Language.GetTextValue(key);
            return lang.Equals(key) ? null : lang;
        }

        public static string TryGetLanguagValue(string path, object arg0)
        {
            string lang = null;
            string key = "Mods.PotionCraft." + path.Replace(" ", "");
            lang += Language.GetTextValue(key, arg0);
            return lang.Equals(key) ? null : lang;
        }

        public static string TryGetPurifyText(int count) => PurifyColor.GetValueOrDefault(count.ToString(), Deafult_Hex)?
            .Insert(10, TryGetLanguagValue($"Craft.Purified"));
        public static string TryGetMashUpText(int count) => MashUpColor.GetValueOrDefault(count.ToString(), Deafult_Hex)?
            .Insert(10, TryGetLanguagValue($"Craft.MashUp"));

        public static string TryGetAndText(int count) => MashUpColor.GetValueOrDefault(count.ToString(), Deafult_Hex)?
            .Insert(10, TryGetLanguagValue($"Craft.And"));

        public static string TryGetBuffName(string name, bool space = false){

            var lname = TryGetBuffNameText(name);
            return ColorfulTexts["BuffName"]
                .GetValueOrDefault(name, Deafult_Hex).Insert(10, $"{lname}{(space ? " " : "")}");
        }

        public static string TryGetBuffNameText(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            if (PotionList.TryGetValue(name, out var value) && value.Item1)
                return Lang.GetBuffName(value.Item2);
            if (PotionList.ContainsKey(name)&&!value.Item1)
                return "NONE";
            var buff = typeof(BuffID).GetField(name, BindingFlags.Static|BindingFlags.Public|BindingFlags.FlattenHierarchy);
            if(buff is null)
            {
                PotionList.TryAdd(name,(false,0));
                return "NONE";
            }
            var buffid = (int)buff.GetValue(null);
            PotionList.TryAdd(name, (true, buffid));
            return Lang.GetBuffName(buffid);
        }

        public static string LocationPotionText(string text) => TryGetLanguagValue($"Craft.{text}");

        public static string RGBToHex(Color color) => color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

        public static string TryGetBracketText(int count, bool right = false)
        {
            string bracket = !right ? "(" : ")";
            return MashUpColor.GetValueOrDefault(count.ToString(), Deafult_Hex)?
            .Insert(10, bracket);
        }

        // 判断是否为中文字符
        private static bool IsChineseCharacter(char c)
        {
            return c >= 0x4E00 && c <= 0x9FA5;
        }
        // 解析字符串并计算每个部分的宽度
        private static List<(string colorCode, string text)> ParseText(string text)
        {
            var parts = new List<(string colorCode, string text)>();
            var regex = new Regex(@"\[c/(\w{6}):([^]]+)\]");
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                string colorCode = match.Groups[1].Value;
                string partText = match.Groups[2].Value;
                parts.Add((colorCode, partText));
            }
            return parts;
        }
        private static int CalculateWidth(string text)
        {
            int width = 0;
            foreach (var c in text)
            {
                width += IsChineseCharacter(c) ? 2 : 1;
            }
            return width;
        }
        /// <summary>
        /// 智能换行
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns>Item1:包含颜色的字符串；Item2：总行数</returns>
        public static (string, int) WrapTextWithColors(string text, int length)
        {
            var parsedParts = ParseText(text);
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            int currentWidth = 0;
            int linecount = 1;
            foreach (var (colorCode, partText) in parsedParts)
            {
                int partWidth = CalculateWidth(partText);
                if (currentWidth + partWidth > length)
                {
                    lines.Add(currentLine.ToString().Trim());
                    currentLine.Clear();
                    currentWidth = 0;
                    linecount++;
                }
                currentLine.Append($"[c/{colorCode}:{partText}]");
                currentWidth += partWidth;
            }
            if (currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString().Trim());
            }
            return (string.Join("\n", lines), linecount);
        }

        public static string DeleteTextColor(string msg)
        {
            string pattern = @"\[c/[^:]+:([^]]+)\]";
            MatchCollection matches = Regex.Matches(msg, pattern);
            string result = "";
            foreach (Match match in matches)
            {
                result += match.Groups[1].Value + " ";
            }
            return result;
        }

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
                string oper = random.Next(0, 2) == 1 ? "purified" : "boling";
                experssion.Insert(random.Next(1, experssion.Count + 1), oper);
            }
            return experssion;
        }

        public static (string,int) WrapText(string text)
        {
            string[] parts = text.Split(' ');
            return (text.Replace(" ", "\n"), parts.Length);
        }

        public static string LocationTranslate(string _name, bool bracket = true)
        {
            string[] parts = _name.Split(' ');
            Stack<string> stack = new();
            var mashupconunt = 1;
            var purifycount = 1;
            foreach (string token in parts)
            {
                if (string.IsNullOrEmpty(token))
                    continue;
                switch (token)
                {
                    case "+":
                        var potion2 = stack.Pop();
                        var potion1 = stack.Pop();
                        stack.Push(bracket
                        ? $"{TryGetBracketText(mashupconunt)}{potion1}{TryGetAndText(mashupconunt)}{potion2}{TryGetBracketText(mashupconunt, true)}{TryGetMashUpText(mashupconunt++)}"
                        : $"{potion1}{TryGetAndText(mashupconunt)}{potion2}{TryGetMashUpText(mashupconunt++)}");
                        break;
                    case "@":
                        stack.Push($"{TryGetPurifyText(purifycount++)} {stack.Pop()}");
                        break;
                    default:
                        stack.Push(TryGetBuffName(token));
                        break;
                }
            }
            return string.Join(" ", stack.ToList());
        }


    }
}

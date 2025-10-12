using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static PotionCraft.Content.System.ColorfulText.OperatorColorText;
using static PotionCraft.Content.System.ColorfulText.PotionColorText;

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

        public static string TryGetPurifyText(int count) => PurifyColor.GetValueOrDefault(count, null)?.Insert(10, "Purified ");

        public static string TryGetMashUpText(int count) => MashUpColor.GetValueOrDefault(count, null)?.Insert(10, "MashUp ");

        public static string TryGetAndText(int count) => MashUpColor.GetValueOrDefault(count, null)?.Insert(10, " And ");

        public static void LocationPotionText(ref string name)
        {
            name = LanguageManager.Instance.ActiveCulture.Name switch
            {
                "zh-Hans" => name.Replace("Purified ", TryGetLanguagValue("Craft.Purified"))
                                 .Replace(" And ", TryGetLanguagValue("Craft.And"))
                                 .Replace("MashUp ", TryGetLanguagValue("Craft.MashUp")),
                "en-US" => name.Replace("纯化", TryGetLanguagValue("Craft.Purified"))
                               .Replace("和", TryGetLanguagValue("Craft.And"))
                               .Replace("组合", TryGetLanguagValue("Craft.MashUp")),
                _ => name,
            };
        }

        public static string GetBracketText(int count, bool mashup = false, bool right = false)
        {
            string bracket = right ? "(" : ")";
            return MashUpColor.GetValueOrDefault(count, null)?.Insert(10, bracket);
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

        public static string TryGetPotionText(int buffid)
        {
            var text = TModLoader.PotionColor.GetValueOrDefault(buffid, null);
            return text?.Insert(10, Lang.GetBuffName(buffid));
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

        public static string ConverToInfix(List<string> postlist)
        {
            Stack<string> stack = new();
            foreach (string token in postlist)
            {
                switch (token)
                {
                    case "purified":
                    case "bowling":
                        {
                            string operand = stack.Pop();

                            bool needParens = operand.Contains("and") || operand.Contains("purified") || operand.Contains("boling");

                            string expr = needParens ? $"({operand})" : operand;

                            stack.Push(token == "purified" ? $"purified({expr})" : $"{expr}bowling");
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

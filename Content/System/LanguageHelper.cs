using System;
using System.Collections;
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
using ReLogic.Graphics;
using static ReLogic.Graphics.DynamicSpriteFont;
using ReLogic.Graphics;

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
            if (BuffsList.TryGetValue(name, out var value) && value.Item1)
                return Lang.GetBuffName(value.Item2);
            if (BuffsList.ContainsKey(name)&&!value.Item1)
                return "NONE";
            var buff = typeof(BuffID).GetField(name, BindingFlags.Static|BindingFlags.Public|BindingFlags.FlattenHierarchy);
            if(buff is null)
            {
                BuffsList.TryAdd(name,(false,0));
                return "NONE";
            }
            var buffid = (int)buff.GetValue(null);
            BuffsList.TryAdd(name, (true, buffid));
            return Lang.GetBuffName(buffid);
        }

        public static string LocationPotionText(string text) => TryGetLanguagValue($"Craft.{text}");

        public static string RGBToHex(Color color) =>color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

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
        public static List<(string colorCode, string text)> ParseText(string text)
        {
            var parts = new List<(string colorCode, string text)>();
            if(string.IsNullOrEmpty(text))
                return parts;
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
        public static Stack<(string colorCode, string text)> ParseText_Stack(string text)
        {
            var parts = new Stack<(string colorCode, string text)>();
            if(string.IsNullOrEmpty(text))
                return parts;
            var regex = new Regex(@"\[c/(\w{6}):([^]]+)\]");
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                var colorCode = match.Groups[1].Value;
                var partText = match.Groups[2].Value;
                parts.Push((colorCode, partText));
            }
            return parts;
        }
        
        private static float CalculateWidth(string text) => FontAssets.MouseText.Value.MeasureString(DeleteTextColor_SaveString(text)).X;

        /// <summary>
        /// 智能换行
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <param name="font"></param>
        /// <returns>Item1:包含颜色的字符串；Item2：总行数</returns>
        public static (string, int) WrapTextWithColors(string text, float length,DynamicSpriteFont font = null)
        {
            var parsedParts = ParseText(text);
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            var lineNumber = 1;
            font ??= FontAssets.MouseText.Value;
            var zero = 0f;
            var num2 = 0f;
            var flag = true;
            var characterData = font.DefaultCharacterData;
            var kerning = characterData.Kerning;

            foreach (var (colorCode, partText) in parsedParts)
            {
                for (var index = 0; index < partText.Length; index++)
                {
                    start:
                    if (flag)
                        kerning.X = Math.Max(kerning.X, 0f);
                    else
                        zero += font.CharacterSpacing + num2;

                    zero += kerning.X + kerning.Y;
                    num2 = kerning.Z;
                    flag = false;
                
                    if (zero <= length) 
                        continue;
                
                    lines.Add(currentLine.ToString().Trim());
                    currentLine.Clear();
                    lineNumber++;
                    num2 = 0f;
                    zero = 0f;
                    flag = true;
                
                    goto start;
                }
                currentLine.Append($"[c/{colorCode}:{partText}]");
            }
            if (currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString().Trim());
            }
            return (string.Join("\n", lines), lineNumber);
        }

        /// <summary>
        /// 智能换行
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <param name="font"></param>
        /// <returns>Item1:包含颜色的字符串；Item2：总行数</returns>
        public static (string, int) WrapTextWithColors_ComPact(string text, float length,DynamicSpriteFont font = null)
        {
            var parsedParts = ParseText(text);
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            var lineNumber = 1;
            font ??= FontAssets.MouseText.Value;
            var zero = 0f;
            var num2 = 0f;
            var flag = true;
            var characterData = font.DefaultCharacterData;
            var kerning = characterData.Kerning;

            foreach (var (colorCode, partText) in parsedParts)
            {
                var substring = partText;
                var pos = 0;
                for (var index = 0; index < partText.Length; index++,pos++)
                {
                    start:
                    if (flag)
                        kerning.X = Math.Max(kerning.X, 0f);
                    else
                        zero += font.CharacterSpacing + num2;

                    zero += kerning.X + kerning.Y;
                    num2 = kerning.Z;
                    flag = false;
                
                    if (zero <= length) 
                        continue;
                
                    currentLine.Append($"[c/{colorCode}:{substring[..pos]}]");
                    lines.Add(currentLine.ToString().Trim());
                    substring=substring[pos..];
                    currentLine.Clear();
                    lineNumber++;
                    num2 = 0f;
                    zero = 0f;
                    pos = 0;
                    flag = true;
                
                    goto start;
                }
                currentLine.Append($"[c/{colorCode}:{substring}]");
            }
            if (currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString().Trim());
            }
            return (string.Join("\n", lines), lineNumber);
        }

        public static Vector2 MeasureString_Cursor(string text,DynamicSpriteFont font =null)
        {
            if (text.Length == 0)
                return Vector2.Zero;

            var zero = Vector2.Zero;
            font ??= FontAssets.MouseText.Value;
            zero.Y = font.LineSpacing;
            var num = 0;
            var num2 = 0f;
            var flag = true;
            foreach (var c in text) {
                switch (c) {
                    case '\n':
                        num2 = 0f;
                        zero = Vector2.Zero;
                        zero.Y = font.LineSpacing;
                        flag = true;
                        num++;
                        continue;
                    case '\r':
                        continue;
                }

                var characterData =font.DefaultCharacterData;
                var kerning = characterData.Kerning;
                if (flag)
                    kerning.X = Math.Max(kerning.X, 0f);
                else
                    zero.X += font.CharacterSpacing + num2;

                zero.X += kerning.X + kerning.Y;
                num2 = kerning.Z;
                zero.Y = Math.Max(zero.Y, characterData.Padding.Height);
                flag = false;
            }

            zero.X += Math.Max(num2, 0f);
            zero.Y = num * font.LineSpacing;
            return zero;
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
        
        public static string DeleteTextColor_SaveString(string msg) => Regex.Replace(msg, @"\[c/\w+:(.*?)\]", "$1");

        public static Color HexToColor(string hex)
        {
            hex = hex.Substring(3, hex.Length - 5);
            // 添加#号以符合HTML颜色格式
            if (hex.StartsWith('#'))
                hex = hex[1..];
            return hex.Length switch
            {
                // 如果包含透明度
                8 => new Color(Convert.ToInt32(hex[..2], 16), Convert.ToInt32(hex.Substring(2, 2), 16),
                    Convert.ToInt32(hex.Substring(4, 2), 16), Convert.ToInt32(hex.Substring(6, 2), 16)),
                6 => new Color(Convert.ToInt32(hex[..2], 16), Convert.ToInt32(hex.Substring(2, 2), 16),
                    Convert.ToInt32(hex.Substring(4, 2), 16), 255),
                _ => throw new ArgumentException("Invalid hex color string.")
            };
        }
        
        public static string CreatQuestion(int level)
        {
            List<string> experssion = [];
            var random = new Random();
            var numOperands = level * random.Next(3, 5);
            for (var  i = 0; i < numOperands; i++)
            {
                experssion.Add($"{Terrariabuffs[random.Next(0, Terrariabuffs.Count + 1)]} ");
                experssion.Add("+ ");
            }
            var operrands = numOperands/level *(random.Next(2,3)+level);
            for (var i = 0; i < operrands; i++)
                experssion.Insert(random.Next(1, experssion.Count + 1), "@ ");
            
            return string.Join("", experssion);
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

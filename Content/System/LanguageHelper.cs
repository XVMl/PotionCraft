using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using static PotionCraft.Content.System.ColorfulText.OperatorColorText;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
using static PotionCraft.Content.System.AutoLoaderSystem.JsonLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using System.Reflection;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using static ReLogic.Graphics.DynamicSpriteFont;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;

namespace PotionCraft.Content.System
{
    public class LanguageHelper
    {
        public static readonly Color Deafult = new(215, 193, 147);

        public static readonly string Deafult_Hex = "[c/D7C193:]";

        public static readonly MethodInfo GetCharacterData = typeof(DynamicSpriteFont).GetMethod("GetCharacterData", BindingFlags.Instance | BindingFlags.NonPublic); 
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

        public static string TryGetPurifyText(int count) => PurifyColor.GetValueOrDefault(count.ToString(), "[c/FC1488:]")?
            .Insert(10, TryGetLanguagValue($"Craft.Purified"));
        public static string TryGetMashUpText(int count) => MashUpColor.GetValueOrDefault(count.ToString(), "[c/02000f:]")?
            .Insert(10, TryGetLanguagValue($"Craft.MashUp"));

        public static string TryGetAndText(int count) => MashUpColor.GetValueOrDefault(count.ToString(), "[c/02000f:]")?
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
        /// <param name="text">文本</param>
        /// <param name="length">最大长度</param>
        /// <param name="left">已有长度</param>
        /// <param name="font">字体</param>
        /// <returns>Item1:包含颜色的字符串；Item2：总行数</returns>
        public static (string, int) WrapTextWithColors_ComPact(string text, float length,float left=0, DynamicSpriteFont font = null)
        {
            var parsedParts = ParseText(text);
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            var lineNumber = 1;
            font ??= FontAssets.MouseText.Value;
            var zero = 0f + left;
            var num2 = 0f;
            var flag = true; 
            
            foreach (var (colorCode, partText) in parsedParts)
            {
                var substring = partText;
                var pos = 0;
                for (var index = 0; index < partText.Length; index++,pos++)
                {
                    start:
                    var characterData = (SpriteCharacterData)GetCharacterData.Invoke(font, [partText[index]]);
                    var kerning = characterData.Kerning;

                    if(flag)
                        kerning.X = Math.Max(kerning.X, 0f);
                    else
                        zero += font.CharacterSpacing + num2;

                    zero += kerning.X + kerning.Y;
                    num2 = kerning.Z;
                    flag = false;

                    if (zero <= length) 
                        continue;

                        
                    if (!string.IsNullOrEmpty(substring[..pos]))
                        currentLine.Append($"[c/{colorCode}:{substring[..pos]}]");
                    
                    //if (LanguageManager.Instance.ActiveCulture.Name != "zh-Hans" && (char.IsLetter(partText[index]) && 
                    //    index-1>=0 && char.IsLetter(partText[index-1]) || index+1 < partText.Length && char.IsLetter(partText[index+1])))
                    //    currentLine.Append($"[c/{colorCode}:-]");
                    
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
            var res = Vector2.Zero;
            res += DrawColorCodedStringWithShadow(FontAssets.MouseText.Value, text, res,Color.White, Vector2.One);
            return res;
        }
        
        private static Vector2 DrawColorCodedString(DynamicSpriteFont font,
            TextSnippet[] snippets, Vector2 position, Vector2 baseScale)
        {
            var vector = position;
            var result = vector;
            var x = font.MeasureString(" ").X;
            var num2 = 1f;
            var num3 = 0f;
            for (var i = 0; i < snippets.Length; i++)
            {
                var textSnippet = snippets[i];
                textSnippet.Update();

                textSnippet.GetVisibleColor();

                num2 = textSnippet.Scale;

                var array = Regex.Split(textSnippet.Text, "(\n)");
                var flag = true;
                foreach (var text in array)
                {
                    var array2 = text.Split(' ');
                    if (text == "\n")
                    {
                        vector.Y += font.LineSpacing * num3 * baseScale.Y;
                        vector.X = position.X;
                        result.X = position.X;
                        result.Y = Math.Max(result.Y, vector.Y);
                        num3 = 0f;
                        flag = false;
                        continue;
                    }

                    for (var k = 0; k < array2.Length; k++)
                    {
                        if (k != 0)
                            vector.X += x * baseScale.X * num2;

                        if (num3 < num2)
                            num3 = num2;

                        var vector2 = font.MeasureString(array2[k]);

                        vector.X += vector2.X * baseScale.X * num2;
                        result.X = Math.Max(result.X, vector.X);
                    }

                    if (array.Length > 1 && flag)
                    {
                        vector.Y += font.LineSpacing * num3 * baseScale.Y;
                        vector.X = position.X;
                        result.Y = Math.Max(result.Y, vector.Y);
                        num3 = 0f;
                    }

                    flag = true;
                }
            }

            return result;
        }

        private static Vector2 DrawColorCodedStringWithShadow(DynamicSpriteFont font, string text, Vector2 position, Color baseColor, Vector2 baseScale)
        {
            var snippets =ChatManager.ParseMessage(text, baseColor).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);
            return DrawColorCodedString(font, snippets, position, baseScale);
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
            experssion.Add($"{Terrariabuffs[random.Next(0, Terrariabuffs.Count)]} ");
            for (var  i = 0; i < numOperands; i++)
            {
                experssion.Add($"{Terrariabuffs[random.Next(0, Terrariabuffs.Count)]} ");
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
                        stack.Push(TryGetBuffName(token.Replace(":", "")));
                        break;
                }
            }
            return string.Join(" ", stack.ToList());
        }
        
    }
    
}

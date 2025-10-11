using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.LoaderSystem
{
    internal class XMLLoader : ModSystem
    {
        public static Dictionary<int, string> PotionColor = [];

        public static Dictionary<int, string> PurifyColor = [];

        public static Dictionary<int, string> MashUpColor = [];

        public static Dictionary<string, string> CustomMaterials = [];
        public static Dictionary<int, string> PotionColorLoader()
        {
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "BuffColorfulText.xml");
            XDocument doc = XDocument.Load("BuffColorfulText.xml");

            Dictionary<int, string> buffColors = new();

            var buffs = doc.Root?.Descendants("tModLoader").Elements();
            foreach (var (id, color) in
            from buff in buffs
            let id = int.Parse(buff.Element("ID").Value)
            let color = buff.Element("Color").Value
            select (id, color))
            {
                buffColors[id] = color;
            }

            return buffColors;

        }

        public static Dictionary<string, string> CustomMaterialsLoader()
        {
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "CustomMaterials.xml");
            XDocument doc = XDocument.Load("CustomMaterials.xml");

            Dictionary<string, string> custommaterials = new();

            var materialNodes = doc.Descendants("Material");
            foreach (var (name, path) in
                from materialNode in materialNodes
                let name = materialNode.Element("name")?.Value
                let path = materialNode.Element("path")?.Value
                where name != null && path != null
                select (name, path))
            {
                // 将名称和路径添加到字典中
                custommaterials[name] = path;
            }
            return custommaterials;

        }

        public override void Load()
        {
            //PotionColor = PotionColorLoader();
            //CustomMaterials = CustomMaterialsLoader();
        }

    }
}

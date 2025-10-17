using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PotionCraft.Content.System.AutoLoaderSystem;

public class LoaderPotionOrMaterial:ModSystem
{
    
    public static readonly List<int>PotionList = [];
    
    public static bool IsFood(Item item) => item.buffType is 26 or 206 or 207;
    
    public static bool CanEditor(Item item) => PotionList.Contains(item.buffType);

    
}
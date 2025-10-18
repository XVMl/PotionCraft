using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PotionCraft.Content.System.AutoLoaderSystem;

public class LoaderPotionOrMaterial:ModSystem
{
    
    public static readonly Dictionary<string,(bool,int)> PotionList = [];
    
    public static bool IsFood(Item item) => item.buffType is 26 or 206 or 207;
    
    public static bool CanEditor(Item item) => PotionList.ContainsKey(item.Name.Replace(" ",""));

    public override void PostAddRecipes()
    {
        
    }

}
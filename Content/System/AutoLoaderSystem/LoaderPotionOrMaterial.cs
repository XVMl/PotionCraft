using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.AutoLoaderSystem;

public class LoaderPotionOrMaterial:ModSystem
{
    
    public static readonly List<int>PotionList = new List<int>();
    
    public static bool IsFood(Item item) => item.buffType is 26 or 206 or 207;
    
    public static bool CanEditor(Item item) => PotionList.Contains(item.buffType);
    
    public override void Load()
    {
    //     for (int i = 0; i < ItemLoader.ItemCount; i++)
    //     {
    //         var item = new Item(); 
    //         item.SetDefaults(i);
    //         if (item.buffType!=0)
    //             PotionList.Add(i);
    //     }
    }
}
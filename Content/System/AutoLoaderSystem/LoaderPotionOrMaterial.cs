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
    public static readonly Dictionary<string, int> PotionList = [];

    public static readonly Dictionary<int, int> PotionWithBuffList = [];

    public static List<string> Terrariabuffs = [];
    
    public static bool IsFood(Item item) => ItemID.Sets.IsFood[item.type];
    
    public override void PostAddRecipes()
    {

        for (var i = 0; i <ItemLoader.ItemCount; i++)
        {
            Item item = new();
            item.SetDefaults(i);
            if (!IsFood(item) && item.consumable && item.buffType is not 0)
            {
                PotionList.TryAdd(item.Name,item.buffType);
                PotionWithBuffList.TryAdd(item.buffType, item.type);
                if (item.buffType < 175)
                Terrariabuffs.Add(BuffID.Search.GetName(item.buffType));
            }
        }
        var list =new List<string>();
        for(int i=1;i<125;i++)
        {
            list.Add(BuffID.Search.GetName(i));
        }
        Mod.Logger.Debug(PotionWithBuffList);
    }



}
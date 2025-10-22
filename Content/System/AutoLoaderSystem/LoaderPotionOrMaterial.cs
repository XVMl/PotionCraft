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

    public static readonly Dictionary<Item, int> ModPotionList = [];
    
    public static bool IsFood(Item item) => item.buffType is 26 or 206 or 207;
    
    public static bool CanEditor(Item item) => PotionList.ContainsKey(item.Name.Replace(" ",""));
    
    public override void PostAddRecipes()
    {
        var Modbuffs = typeof(BuffLoader).GetField("buffs", BindingFlags.Static|BindingFlags.NonPublic)?.GetValue(null) as IList<ModBuff>;
        foreach (var buff in Modbuffs)
        {
            PotionList.TryAdd(buff.Name, (false, Modbuffs.IndexOf(buff)));
        }
        
        for (var i = 0; i <ItemLoader.ItemCount; i++)
        {
            Item item = new();
            item.SetDefaults(i);
            if (!IsFood(item) && item.consumable && item.buffType is not 0)
            {
                ModPotionList.TryAdd(item,item.buffType);
            }
        }
    }

}
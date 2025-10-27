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
    
    public static readonly Dictionary<string,(bool,int)> BuffsList = [];

    public static readonly Dictionary<Item, int> PotionList = [];
    
    public static List<string> Terrariabuffs = [];
    
    public static bool IsFood(Item item) => item.buffType is 26 or 206 or 207;
    
    public static bool CanEditor(Item item) => BuffsList.ContainsKey(item.Name.Replace(" ",""));
    
    public override void PostAddRecipes()
    {
        var Modbuffs = typeof(BuffID).GetField("buffs", BindingFlags.Static|BindingFlags.Public|BindingFlags.FlattenHierarchy)?.GetValue(null) as IList<ModBuff>;
        var modfields = typeof(BuffID).GetFields();
        foreach (var modfield in modfields)
        {
            if (modfield.FieldType != typeof(int)) continue;
            BuffsList.TryAdd(modfield.Name, (true, (int)modfield.GetValue(null)! ));
            Terrariabuffs.Add(modfield.Name);
        }
        foreach (var buff in Modbuffs)
            BuffsList.TryAdd(buff.Name, (false, Modbuffs.IndexOf(buff)));
        
        for (var i = 0; i <ItemLoader.ItemCount; i++)
        {
            Item item = new();
            item.SetDefaults(i);
            if (!IsFood(item) && item.consumable && item.buffType is not 0)
            {
                PotionList.TryAdd(item,item.buffType);
            }
        }
    }

}
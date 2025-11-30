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

    public static readonly Dictionary<string, int> PotionList = [];

    public static List<string> Terrariabuffs = [];

    public static List<string> Foods = new();
    
    public static bool IsFood(Item item) => ItemID.Sets.IsFood[item.type];
    
    public override void PostAddRecipes()
    {
        var modfields = typeof(BuffID).GetFields();
        foreach (var modfield in modfields)
        {
            if (modfield.FieldType != typeof(int)) continue;
            BuffsList.TryAdd(modfield.Name, (true, (int)modfield.GetValue(null)! ));
            if (((int)modfield.GetValue(null)>= 1 && (int)modfield.GetValue(null) <= 18) ||
                ((int)modfield.GetValue(null) >= 104 && (int)modfield.GetValue(null) <= 124))
            Terrariabuffs.Add(modfield.Name);
        }

        for (var i = 0; i <ItemLoader.ItemCount; i++)
        {
            Item item = new();
            item.SetDefaults(i);
            if (IsFood(item))
            {
                Foods.Add(item.Name);
            }
            if (!IsFood(item) && item.consumable && item.buffType is not 0)
            {
                PotionList.TryAdd(item.Name,item.buffType);
            }
        }
        var buffs = typeof(BuffLoader).GetField("buffs", BindingFlags.Static | BindingFlags.NonPublic)
            .GetValue(null) as IList<ModBuff>;

        for (int i = BuffID.Count; i < buffs.Count; i++)
        {
            ModBuff buff = buffs[i];
            BuffsList.TryAdd(buff.Name, (true, buff.Type));
        }

    }

}
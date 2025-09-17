using PotionCraft.Content.Items;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.System;

public class LoaderItemsSystem:ModSystem
{
    public static readonly Dictionary<string, string> CustomMaterials = new() 
    {
        {"Wine1", "BasePotion"},
        {"Wine2","BasePotion" }
    };
    
    public override void Load()
    {

        foreach (var item in CustomMaterials)
        {
            Mod.AddContent(new BaseCustomMaterials(item.Key, item.Value));
        }
    }
}
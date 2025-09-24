using System.Collections.Generic;
using PotionCraft.Content.Items;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.AutoLoaderSystem;

public class LoaderItemsSystem:ModSystem
{
    
    private readonly List<MaterialData> Materials = [
        new("Pudding","",Base.Water),
        new("Jelly","",Base.Water),
    ];
    
    public override void Load()
    {
        foreach (var item in Materials)
        {
            Mod.AddContent(new BaseCustomMaterials(item));
        }
    }
}
using System.Collections.Generic;
using PotionCraft.Content.Items;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.AutoLoaderSystem;

public class LoaderItemsSystem:ModSystem
{
    
    private readonly List<MaterialData> Materials = [
        new("Pudding","",Base.Water,2f),
        new("Jelly","",Base.Water,1.7f),
        new("Lily","",Base.Magic,1.65f),
        new("Style2","",Base.None,1.5f),
        new("Style3","",Base.None,1.5f),
        new("Style4","",Base.None,1.5f),
    ];
    
    public override void Load()
    {
        foreach (var item in Materials)
        {
            Mod.AddContent(new BaseCustomMaterials(item));
        }
    }
}
using PotionCraft.Content.Items;
using Terraria.ModLoader;

namespace PotionCraft.Content.System;

public class LoaderItemsSystem:ModSystem
{
    
    public override void Load()
    {
        this.Mod.AddContent(new BaseCustomMaterials());
    }
}
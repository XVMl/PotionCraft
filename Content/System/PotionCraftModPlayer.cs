using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PotionCraft.Content.System;

public class PotionCraftModPlayer:ModPlayer
{
    public bool CanNOBasePotion = true;


    public override void SaveData(TagCompound tag)
    {
        tag["CanNOBasePotion"] =  CanNOBasePotion;  
    }

    public override void LoadData(TagCompound tag)
    {
        CanNOBasePotion = tag.GetBool("CanNOBasePotion");
    }
}
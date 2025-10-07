using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PotionCraft.Content.System;

public class PotionCraftModPlayer:ModPlayer
{
    public bool CanNOBasePotion = true;

    public static ModKeybind PotionCraftKeybind;

    public override void Load()
    {
        PotionCraftKeybind = KeybindLoader.RegisterKeybind(this.Mod, "ExtraList", Keys.LeftShift);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["CanNOBasePotion"] =  CanNOBasePotion;  
    }

    public override void LoadData(TagCompound tag)
    {
        CanNOBasePotion = tag.GetBool("CanNOBasePotion");
    }
}
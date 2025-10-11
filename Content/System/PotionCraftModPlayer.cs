using Microsoft.Xna.Framework.Input;
using PotionCraft.Content.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static PotionCraft.Content.System.AutoUIState;

namespace PotionCraft.Content.System;

public class PotionCraftModPlayer:ModPlayer
{
    public bool CanNOBasePotion = true;

    public static ModKeybind PotionCraftKeybind;

    public override void Load()
    {
        PotionCraftKeybind = KeybindLoader.RegisterKeybind(this.Mod, "ExtraList", Keys.LeftShift);
    }

    public override void UpdateDead()
    {
        foreach (var activeItem in Main.ActiveItems)
        {
            if (activeItem.ModItem is not BasePotion)       
                continue;
            var potion = AsPotion(activeItem);
            if (!potion.AutoUse)
                continue;
            potion.UseItem(Main.LocalPlayer);
            potion.Item.stack -= 1;
            return;
        }
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
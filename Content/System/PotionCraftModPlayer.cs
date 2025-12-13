using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PotionCraft.Content.Items;
using PotionCraft.Content.UI.CraftUI;
using PotionCraft.Content.UI.PotionTooltip;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static PotionCraft.Content.System.AutoUIState;

namespace PotionCraft.Content.System;

public class PotionCraftModPlayer:ModPlayer
{
    public bool CanNOBasePotion = true;

    public string PotionName="";

    public int remainingTime = 0;

    public static ModKeybind PotionCraftKeybind;

    public static MethodInfo Helditem = typeof(Player).GetMethod(nameof(Main.LocalPlayer.HeldItem), BindingFlags.Public | BindingFlags.Instance);

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

    public override void PostUpdate()
    {
        if (Main.dedServ)
        {
            return;
        }
        PotionCraftUI.UIstate.TryGetValue(nameof(TooltipUI), out var state);
        state.Active = Main.HoverItem.type.Equals(ModContent.ItemType<BasePotion>());

        if (remainingTime > 0)
            remainingTime--;

        if (remainingTime == 0)
            PotionName = "";

        if (!ActiveState)
            return;
        ManagedScreenFilter distortion = ShaderManager.GetFilter("PotionCraft.GaussBlur");
        if (!distortion.IsActive)
        {
            distortion.TrySetParameter("screenscalerevise", new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom);
            distortion.Activate();
        }
        
        if(remainingTime > 0)
            remainingTime--;
    }

    public override void SaveData(TagCompound tag)
    {
        tag["CanNOBasePotion"] =  CanNOBasePotion;
        tag["PotionName"] = PotionName;
        tag["remainingTime"] = remainingTime;
    }

    public override void LoadData(TagCompound tag)
    {
        CanNOBasePotion = tag.GetBool("CanNOBasePotion");
        PotionName = tag.GetString("PotionName");
        remainingTime = tag.GetInt("remainingTime");
    }
}
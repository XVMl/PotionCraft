using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using PotionCraft.Content.UI.DialogueUI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static PotionCraft.Content.System.AutoUIState;
namespace PotionCraft.Content.Items;
public class Invitation:ModItem
{
    public override string Texture => Assets.Path.Items + Name;

    public override void SetStaticDefaults()
    {

    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 1;
        Item.rare = 0;
        Item.useAnimation = 45;
        Item.useTime = 45;
        Item.useStyle = ItemUseStyleID.HiddenAnimation;
        Item.UseSound = SoundID.Item3;
    }
    public override bool? UseItem(Player player)
    {
        player.itemAnimation = Item.useAnimation;
        PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
        state.Active = !state.Active; 
        ActiveState = !ActiveState;
        if(!state.Active)
        {
            ((BrewPotionState)state).InitPotion();
            PotionCraftUI.UIstate.TryGetValue(nameof(ColorSelector), out var colorselect);
            colorselect.Active = false;
            PotionCraftUI.UIstate.TryGetValue(nameof(DialogueState), out var dialogue);
            dialogue.Active = false;
            PotionCraftUI.UIstate.TryGetValue(nameof(PotionBaseSelect), out var potionbase);
            potionbase.Active = false;
        }
        return true;
    }
}


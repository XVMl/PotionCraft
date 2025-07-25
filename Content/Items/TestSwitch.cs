﻿using Luminance.Assets;
using Luminance.Common.Utilities;
using PotionCraft.Content.UI.CraftUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PotionCraft.Content.System.AutoUIState;

namespace PotionCraft.Content.Items
{
    internal class TestSwitch:ModItem
    {

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = 0;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 2;
            Item.UseSound = SoundID.Item3;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            player.itemAnimation = Item.useAnimation;
            if (player.altFunctionUse==2)
            {
                PotionCraftState.CraftState = (CraftUiState)((int)(PotionCraftState.CraftState + 1) % 4);
                Main.NewText(PotionCraftState.CraftState);
            }
            else
            {
                PotionCraftState.ActiveState = !PotionCraftState.ActiveState;
            }
            return true;
        }
    }
}

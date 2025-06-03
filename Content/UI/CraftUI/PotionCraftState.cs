using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionCraftState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState;

        public override string Layers_FindIndex => "Vanilla: Interface Logic 3";

        public UIElement area;

        public Item item = new();

        public override void OnInitialize()
        {
            area = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };
            area.Width.Set(400f, 0);
            area.Height.Set(300f, 0);
            Append(area);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.BackGround1, area.GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class PotionSlot<T> : PotionElement<T> where T : AutoUIState
    {
        public PotionSlot(T potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!Main.mouseItem.IsAir && PotionCraftState.Potion.IsAir)
            {
                PotionCraftState.Potion = Main.mouseItem.Clone();
                Main.LocalPlayer.HeldItem.TurnToAir();
                Main.mouseItem.TurnToAir();
                return;
            }
            if (Main.mouseItem.IsAir && !PotionCraftState.Potion.IsAir)
            {
                Main.mouseItem = PotionCraftState.Potion.Clone();
                PotionCraftState.Potion.TurnToAir();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Slot, GetDimensions().ToRectangle(), Color.White);
            if (!PotionCraftState.Potion.IsAir)
            {
                Main.inventoryScale = 2.30f;
                ItemSlot.Draw(spriteBatch, ref PotionCraftState.Potion, 21, GetDimensions().Position());
                if (IsMouseHovering)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Main.HoverItem = PotionCraftState.Potion.Clone();
                    Main.hoverItemName = "a";
                    return;
                }
            }
        }

    }

    public class MaterialSlot<T> : PotionElement<T> where T : AutoUIState
    {

        public MaterialSlot(T potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!Main.mouseItem.IsAir && PotionCraftState.Material.IsAir)
            {
                if (!IsMaterial(Main.mouseItem))
                {
                    return;
                }
                PotionCraftState.Material = Main.mouseItem.Clone();
                Main.LocalPlayer.HeldItem.TurnToAir();
                Main.mouseItem.TurnToAir();
                return;
            }
            if (Main.mouseItem.IsAir && !PotionCraftState.Material.IsAir)
            {
                Main.mouseItem = PotionCraftState.Material.Clone();
                PotionCraftState.Material.TurnToAir();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Slot, GetDimensions().ToRectangle(), Color.White);
            if (!PotionCraftState.Material.IsAir)
            {
                Main.inventoryScale = 2.3f;
                ItemSlot.Draw(spriteBatch, ref PotionCraftState.Material, 21, GetDimensions().Position());
                if (IsMouseHovering)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Main.HoverItem = PotionCraftState.Material.Clone();
                    Main.hoverItemName = "a";
                    return;
                }
            }
        }

    }

    public class CreatedPotionSlot<T> : PotionElement<T> where T : AutoUIState
    {

        public CreatedPotionSlot(T potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (Main.mouseItem.IsAir && !PotionCraftState.CreatedPotion.IsAir)
            {
                Main.mouseItem = PotionCraftState.CreatedPotion.Clone();
                PotionCraftState.CreatedPotion.TurnToAir();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Panel, GetDimensions().ToRectangle(), Color.White);
            if (!PotionCraftState.CreatedPotion.IsAir)
            {
                Main.inventoryScale = 2.3f;
                ItemSlot.Draw(spriteBatch, ref PotionCraftState.CreatedPotion, 21, GetDimensions().Position());
                if (IsMouseHovering)
                {

                    Main.LocalPlayer.mouseInterface = true;
                    Main.HoverItem = PotionCraftState.CreatedPotion.Clone();
                    Main.hoverItemName = "a";
                    return;
                }
            }
        }

    }

}

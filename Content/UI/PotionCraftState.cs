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
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PotionCraft.Content.UI
{
    public class PotionCraftState : AutoUIState
    {
        public override string Layers_FindIndex => "Vanilla: Interface Logic 2";

        private UIGrid iGrid;

        public Item Potion = new();

        public PotionSlot potionslot;

        public PurifyingButton purifyingbutton;
        public override void OnInitialize()
        {
            iGrid = new UIGrid();
            iGrid.Width.Set(200, 0);
            iGrid.Height.Set(200, 0);
            iGrid.HAlign = 0.5f;
            iGrid.VAlign = 0.5f;
            potionslot = new(this);
            purifyingbutton = new(this);
            Append(iGrid);
            iGrid.Append(purifyingbutton);
            iGrid.Append(potionslot);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Panel, iGrid.GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class PotionSlot : UIElement
    {
        public PotionCraftState PotionCraftState;
        public PotionSlot(PotionCraftState potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(120f, 0f);
            Height.Set(120f, 0f);
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
                Main.inventoryScale = 2.3076925f;
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

    public class PurifyingButton:UIElement
    {

        public PotionCraftState PotionCraftState;

        public static TestPotion AsPotion(Item item)
        {
            if (item.ModItem is TestPotion testPotion)
            {
                return testPotion;
            }
            Mod instance = ModContent.GetInstance<PotionCraft>();
            if (item.ModItem==null)
            {
                instance.Logger.Warn($"Item was erroneously casted to Potion");
            }
            else
            {
                instance.Logger.Warn($"Item was erroneously casted to Potion");
            }
            return ModContent.GetInstance<TestPotion>();
        }

        public PurifyingButton(PotionCraftState potionCraftState)
        {
            PotionCraftState= potionCraftState;
            Width.Set(200f, 0);
            Height.Set(32f, 0);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (PotionCraftState.Potion.IsAir) return;
            TestPotion tes= AsPotion(PotionCraftState.Potion);
            tes.BuffDictionary.Add(34, 300);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }
    }

}

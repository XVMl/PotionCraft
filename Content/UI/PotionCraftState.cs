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
        public static bool ActiveState;

        public enum CraftUIState
        {
            Purificating,
            MashUp,
            Boling
        }

        public static CraftUIState CraftState;

        public override bool IsLoaded() => ActiveState;
        public override string Layers_FindIndex => "Vanilla: Interface Logic 2";

        public UIElement area;
        public override void OnInitialize()
        {
            area=new UIElement() 
            { 
                HAlign=0.5f,
                VAlign=0.5f,
            };
            area.Width.Set(400f,0);
            area.Height.Set(300f, 0);
            Append(area);

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.BackGround1,area.GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class PotionSlot<T> : UIElement where T : AutoUIState
    {
        public T PotionCraftState;

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
                Main.inventoryScale = 2.0f;
                ItemSlot.Draw(spriteBatch, ref PotionCraftState.Potion, 21, GetDimensions().Position());
                //if (IsMouseHovering)
                //{
                //    Main.LocalPlayer.mouseInterface = true;
                //    Main.HoverItem = PotionCraftState.Potion.Clone();
                //    Main.hoverItemName = "a";
                //    return;
                //}
            }
        }


    }

}

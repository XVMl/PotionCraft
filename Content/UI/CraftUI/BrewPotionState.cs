using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;

using static PotionCraft.Assets;
namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.BrewPotion;

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BrewPotionState> PotionSlot;

        private BrewPotionButton BrewPotionButton;

        public int PotionCount = 20;

        private UIElement Area;
        public override void OnInitialize()
        {

            Width.Set(920, 0);
            Height.Set(640, 0);
            HAlign = 0.5f;
            VAlign = 0.5f;
            Area = new UIElement()
            {
                HAlign = 0.2f,
                VAlign = 0.3f,
            };
            Area.Width.Set(270f, 0);
            Area.Height.Set(270f, 0);
            Append(Area);

            PotionSlot = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };
            Area.Append(PotionSlot);

            BrewPotionButton = new BrewPotionButton(this)
            {
                HAlign = 0.5f,
                VAlign = 1f,
            };
            Append(BrewPotionButton);

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            //spriteBatch.Draw(UITexture("PotionCraftBG").Value, GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class BrewPotionButton : PotionElement<BrewPotionState>
    {

        public BrewPotionButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void BrewPotion(Item potion)
        {
            if (!IsPotion(potion)) return;
            PotionCraftState.Potion.stack = PotionCraftState.PotionCount;
            Main.LocalPlayer.BuyItem(50 * PotionCraftState.PotionCount);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
            BrewPotion((PotionCraftState.Potion));
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var rectangle = GetDimensions().ToRectangle();
            spriteBatch.Draw(Assets.UI.Button, rectangle, Color.White);
            ItemSlot.DrawSavings(spriteBatch,rectangle.X, rectangle.Y);
        }

    }
}

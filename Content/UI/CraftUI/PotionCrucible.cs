using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionCrucible : PotionElement<BrewPotionState>
    {
        private UIElement Crucible;

        public Button<BrewPotionState> MashUp;

        public Button<BrewPotionState> Putity;

        private BrewPotionState BrewPotionState;
        public PotionCrucible(BrewPotionState brewPotionState)
        {
            BrewPotionState = brewPotionState;
            PotionCraftState = brewPotionState;
            Height.Set(400, 0);
            Width.Set(400, 0);
            Crucible = new UIElement();
            Crucible.Width.Set(384, 0);
            Crucible.Height.Set(234, 0);
            Crucible.Top.Set(250, 0);
            Crucible.HAlign = .5f;
            Append(Crucible);

            MashUp = new(Assets.UI.HelpIcon, Color.White, brewPotionState)
            {
                Name = "MashUp",
                Active = false
            };
            MashUp.Width.Set(32, 0);
            MashUp.Height.Set(32, 0);
            MashUp.Top.Set(160, 0);
            MashUp.Left.Set(360, 0);
            MashUp.OnClike = () =>
            {
                BrewPotionState.MashUp(brewPotionState.CreatPotion, brewPotionState.currentItem);
                brewPotionState.Craft = null;
                brewPotionState.PotionMaterial = null;
                Putity.Active = false;
                MashUp.Active = false;
                brewPotionState.Refresh();
            };
            MashUp.TransitionAnimation = () =>
            {
                MashUp.A = MathHelper.Lerp(MashUp.A, MashUp.Active ? 1 : .7f, .05f);
            };
            MashUp.HoverTexture = Assets.UI.HelpIconActive;
            Append(MashUp);

            Putity = new(Assets.UI.HelpIcon, Color.White, brewPotionState)
            {
                Name = "Putity",
                Active = false
            };
            Putity.Width.Set(32, 0);
            Putity.Height.Set(32, 0);
            Putity.Top.Set(200, 0);
            Putity.Left.Set(360, 0);
            Putity.OnClike = () =>
            {
                BrewPotionState.Putify(brewPotionState.CreatPotion, brewPotionState.currentItem);
                brewPotionState.Craft = null;
                brewPotionState.PotionMaterial = null;
                Putity.Active = false;
                MashUp.Active = false;
                brewPotionState.Refresh();
            };
            Putity.TransitionAnimation = () =>
            {
                Putity.A = MathHelper.Lerp(Putity.A, Putity.Active ? 1 : .7f, .05f);
            };
            Putity.HoverTexture = Assets.UI.HelpIconActive;
            Append(Putity);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MashUp.Update(gameTime);
            Putity.Update(gameTime);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!PotionCraftState.Active) return;
            if (Main.mouseItem.IsAir)
                return;

            if (!PotionList.ContainsKey(Main.mouseItem.Name) && Main.mouseItem.ModItem is not MagicPanacea)
                return;

            BrewPotionState.AddPotion(Main.mouseItem);
            Main.LocalPlayer.HeldItem.TurnToAir();
            Main.mouseItem.TurnToAir();
            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Crucible, Crucible.GetDimensions().ToRectangle(), Color.White);
            base.Draw(spriteBatch);
        }

    }

}

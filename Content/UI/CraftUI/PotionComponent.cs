using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using Terraria.UI;
using Microsoft.Xna.Framework;
using PotionCraft.Content.UI.PotionTooltip;
using Terraria.GameInput;
namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionComponent : PotionElement<BrewPotionState>
    {
        private BrewPotionState _brewPotionState;

        private PotionList Potion;

        private PotionList Material;

        private UIElement Area;
        public PotionComponent(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            _brewPotionState = brewPotionState;
            Width.Set(346f, 0);
            Height.Set(486f, 0);

            Area = new();
            Area.Width.Set(290, 0);
            Area.Height.Set(400, 0);
            Area.Top.Set(42, 0);
            Area.Left.Set(36, 0);
            Area.OverflowHidden = true;
            Append(Area);

            Potion = new(brewPotionState);
            Potion.Width.Set(290, 0);
            Potion.Height.Set(400, 0);
            Area.Append(Potion);

            Material = new(brewPotionState);
            Material.Width.Set(290, 0);
            Material.Height.Set(180, 0);
            Area.Append(Material);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Potion.Update(gameTime);
            Material.Top.Set(Potion.Height.Pixels + 40f, 0);
        }

        public void ComponentUpdate()
        {
            Potion.ListUpdate(_brewPotionState.CreatPotion);
            Potion.tarHeigth = _brewPotionState.PotionMaterial is null ? 400 : 180;
            if(_brewPotionState.PotionMaterial is not null)
            Material.ListUpdate(_brewPotionState.PotionMaterial.ModItem as BasePotion);
        }

        public void SettarHeight(float h) => Potion.tarHeigth = h;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.UI2, GetDimensions().Position(), Color.White);
            base.Draw(spriteBatch);
        }

        private class PotionList : PotionElement<BrewPotionState>
        {
            private UIElement PotionArea;

            public PotionIngredients _potionIngredients;

            private float ListTop;

            public float tarHeigth;

            public float tarTop;

            public PotionList(BrewPotionState brewPotionState)
            {
                PotionCraftState = brewPotionState;
                PotionArea = new UIElement();
                PotionArea.Width.Set(290, 0);
                PotionArea.Height.Set(400, 0);
                PotionArea.OverflowHidden = true;
                Append(PotionArea);
                _potionIngredients = new PotionIngredients(brewPotionState);
                _potionIngredients.Width.Set(275, 0);
                _potionIngredients.Height.Set(400, 0);
                PotionArea.Append(_potionIngredients);
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);
                _potionIngredients.Update(gameTime);
                ListTop +=  PlayerInput.ScrollWheelValueOld - PlayerInput.ScrollWheelValue;
                var offset = MathHelper.Lerp(_potionIngredients.Top.Pixels, ListTop*.1f, .1f);
                _potionIngredients.Top.Set(offset, 0);
                if (tarHeigth == 0)
                    return;
                Height.Set(MathHelper.Lerp(Height.Pixels, tarHeigth, .1f), 0);
                PotionArea.Height.Set(MathHelper.Lerp(PotionArea.Height.Pixels, tarHeigth, .1f), 0);
            }

            public void ListUpdate(BasePotion potion)
            {
                _potionIngredients.UIgrid.Clear();
                _potionIngredients.SetPotionCraftState(potion);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Assets.UI.UI5, GetDimensions().ToRectangle(),new Rectangle(0,0,290,(int)PotionArea.Height.Pixels), Color.White);
                spriteBatch.Draw(Assets.UI.UI5, GetDimensions().Position()+new Vector2(0, PotionArea.Height.Pixels-18), new Rectangle(0, 380, 290, 18), Color.White);
                base.Draw(spriteBatch);
            }

        }

        private class Ingredients : PotionElement<BrewPotionState>
        {
            private PotionIngredients _potionIngredients;

            public Ingredients(BrewPotionState brewPotionState)
            {
                PotionCraftState = brewPotionState;
                _potionIngredients = new(brewPotionState);


            }
        }

    }
}

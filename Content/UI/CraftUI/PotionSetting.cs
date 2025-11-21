using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using static PotionCraft.Assets;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.LanguageHelper;


namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSetting : PotionElement<BrewPotionState>
    {
        private UIElement BG;

        private Button delete;

        public Button brew;

        public Slider slider;

        private Button autouse;

        private Button potionlock;

        private Button packing;

        private Button Hajimi;

        public PotionSetting(BrewPotionState brewPotionState)
        {
            Width.Set(384f, 0);
            Height.Set(300f, 0);
            
            BG = new();
            BG.Width.Set(384, 0);
            BG.Height.Set(224, 0);
            Append(BG);

            PotionCraftState = brewPotionState;
            delete = new Button(UITexture("Delete"), Color.White,brewPotionState, "Delete");
            delete.Height.Set(34, 0);
            delete.Width.Set(96, 0);
            delete.Left.Set(50, 0);
            delete.Top.Set(170, 0);
            BG.Append(delete);

            brew = new Button(UITexture("Brew"), Color.White,brewPotionState, "Brew");
            brew.Height.Set(34, 0);
            brew.Width.Set(96, 0);
            brew.Left.Set(250, 0);
            brew.Top.Set(170, 0);
            brew.OnClike = brewPotionState.BrewPotion;
            BG.Append(brew);

            slider = new Slider(brewPotionState,"数量");
            slider.Left.Set(90, 0);
            slider.Top.Set(112, 0);
            slider.text.TextColor = Deafult;
            BG.Append(slider);

            autouse = new Button(Assets.UI.Icon, Color.White, new Rectangle(40,0,18,18),brewPotionState);
            autouse.Height.Set(18, 0);
            autouse.Width.Set(18, 0);
            autouse.Left.Set(116, 0);
            autouse.Top.Set(46, 0);
            autouse.Iconcolor = Deafult;
            autouse.Value = brewPotionState.CreatPotion.AutoUse;
            autouse.OnClike = () =>
            {
                autouse.Value = !autouse.Value;
                brewPotionState.CreatPotion.AutoUse = !autouse.Value;;
            };
            BG.Append(autouse);

            potionlock = new Button(Assets.UI.Icon, Color.White, new Rectangle(62, 0, 18, 18),brewPotionState);
            potionlock.Height.Set(18, 0);
            potionlock.Width.Set(18, 0);
            potionlock.Left.Set(40, 0);
            potionlock.Top.Set(46, 0);
            potionlock.Iconcolor = Deafult;
            potionlock.OnClike = () =>
            {
                potionlock.Value = !potionlock.Value;
                brewPotionState.CreatPotion.IsPackage = !potionlock.Value;
                potionlock.Rectangle = potionlock.Value ? new Rectangle(62, 0, 18, 18) : new Rectangle(80, 0, 18, 18); ;
            };
            BG.Append(potionlock);

            packing = new Button(Assets.UI.Icon, Color.White, new Rectangle(20, 0, 18, 18),brewPotionState);
            packing.Height.Set(18, 0);
            packing.Width.Set(18, 0);
            packing.Left.Set(76, 0);
            packing.Top.Set(46, 0);
            packing.Iconcolor = Deafult;
            packing.OnClike = () =>
            {
                packing.Value = !packing.Value;
                brewPotionState.CreatPotion.IsPackage = !packing.Value;
                packing.Rectangle = packing.Value ? new Rectangle(0, 0, 18, 18) : new Rectangle(20, 0, 18, 18); ;
            };
            BG.Append(packing);

            Hajimi = new(Assets.UI.HajimiIcon, Color.White,brewPotionState);
            Hajimi.Width.Set(40, 0);
            Hajimi.Height.Set(34, 0);
            Hajimi.Top.Set(240, 0);
            Hajimi.Left.Set(300, 0);
            Hajimi.HoverTexture = Assets.UI.HajimiIconHover;
            Append(Hajimi);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            slider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.UI4, BG.GetDimensions().ToRectangle(), Color.White);
            Utils.DrawBorderString(spriteBatch, "花费:", GetDimensions().Position() + new Vector2(40, 80), Deafult, 1f);
            BrewPotionState.DrawCost(spriteBatch, slider.value*1000, GetDimensions().Position()+new Vector2(90,80));
            ItemSlot.DrawSavings(spriteBatch, GetDimensions().X, GetDimensions().Y+200);
            base.Draw(spriteBatch);
        }

    }
}

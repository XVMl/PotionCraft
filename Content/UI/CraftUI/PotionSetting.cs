using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using static PotionCraft.Assets;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.LanguageHelper;
using PotionCraft.Content.UI.DialogueUI;
using System;
using Terraria.GameContent;


namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSetting : PotionElement<BrewPotionState>
    {
        private UIElement BG;

        private Button<BrewPotionState> delete;

        public Button<BrewPotionState> brew;

        public Slider slider;

        private Button<BrewPotionState> autouse;

        private Button<BrewPotionState> potionlock;

        private Button<BrewPotionState> packing;

        private Button<BrewPotionState> Hajimi;

        private Button<BrewPotionState> help;

        public PotionSetting(BrewPotionState brewPotionState)
        {
            Width.Set(384f, 0);
            Height.Set(300f, 0);
            
            BG = new();
            BG.Width.Set(384, 0);
            BG.Height.Set(224, 0);
            Append(BG);

            PotionCraftState = brewPotionState;
            delete = new Button<BrewPotionState>(UITexture("Delete"), Color.White,brewPotionState, "Delete");
            delete.Name = "ClearAll";
            delete.Height.Set(34, 0);
            delete.Width.Set(96, 0);
            delete.Left.Set(50, 0);
            delete.Top.Set(170, 0);
            delete.OnClike = brewPotionState.ClearAll;
            BG.Append(delete);

            brew = new Button<BrewPotionState>(UITexture("Brew"), Color.White,brewPotionState, "Brew");
            brew.Name = "BrewPotion";
            brew.Height.Set(34, 0);
            brew.Width.Set(96, 0);
            brew.Left.Set(250, 0);
            brew.Top.Set(170, 0);
            brew.OnClike = brewPotionState.BrewPotion;
            BG.Append(brew);

            slider = new Slider(brewPotionState, "数量")
            {
                Name = "NumberSlider",
                onChange = () =>
                {
                    if (brewPotionState.PreviewPotion is not null)
                        brewPotionState.PreviewPotion.stack = slider.value;
                }
            };
            slider.text.TextColor = Deafult;
            slider.Left.Set(90, 0);
            slider.Top.Set(112, 0);
            BG.Append(slider);

            autouse = new Button<BrewPotionState>(Assets.UI.Icon, Color.White, new Rectangle(40,0,18,18),brewPotionState);
            autouse.Name = "AutoUse";
            autouse.Height.Set(18, 0);
            autouse.Width.Set(18, 0);
            autouse.Left.Set(116, 0);
            autouse.Top.Set(46, 0);
            autouse.Iconcolor = Deafult;
            autouse.Value = brewPotionState.CreatPotion.AutoUse;
            autouse.OnClike = () =>
            {
                autouse.Value = !autouse.Value;
                brewPotionState.CreatPotion.AutoUse = autouse.Value;
                brewPotionState.Refresh();
            };
            
            BG.Append(autouse);

            potionlock = new Button<BrewPotionState>(Assets.UI.Icon, Color.White, new Rectangle(82, 0, 18, 18),brewPotionState);
            potionlock.Name = "LockPotion";
            potionlock.Height.Set(18, 0);
            potionlock.Width.Set(18, 0);
            potionlock.Left.Set(40, 0);
            potionlock.Top.Set(46, 0);
            potionlock.Iconcolor = Deafult;
            potionlock.OnClike = () =>
            {
                potionlock.Value = !potionlock.Value;
                brewPotionState.CreatPotion.CanEditor = !potionlock.Value;
                potionlock.Rectangle = potionlock.Value ? new Rectangle(82, 0, 18, 18) : new Rectangle(64, 0, 18, 18);
                brewPotionState.Refresh();
            };
            BG.Append(potionlock);

            packing = new(Assets.UI.Icon, Color.White, new Rectangle(22, 0, 18, 18), brewPotionState)
            {
                Name = "PackingPotion",
                Value = true,
            };
            packing.Height.Set(18, 0);
            packing.Width.Set(18, 0);
            packing.Left.Set(76, 0);
            packing.Top.Set(46, 0);
            packing.Iconcolor = Deafult;
            packing.OnClike = () =>
            {
                packing.Value = !packing.Value;
                brewPotionState.CreatPotion.IsPackage = !packing.Value;
                packing.Rectangle = packing.Value ? new Rectangle(22, 0, 18, 18) : new Rectangle(0, 0, 18, 18);
                brewPotionState.Refresh();
            };
            BG.Append(packing);

            help = new(Assets.UI.HelpIcon, Color.White,brewPotionState);
            help.Name = "HelpButton";
            help.Width.Set(32, 0);
            help.Height.Set(32, 0);
            help.Top.Set(240, 0);
            help.Left.Set(360, 0);
            help.OnClike = () =>
            {
                help.Value = !help.Value;
                PotionCraftUI.UIstate.TryGetValue(nameof(DialogueState), out var state);
                state.A = 0;
                state.Top.Set(80, 0);
                state.Active = !state.Active;
            };
            help.HoverTexture = Assets.UI.HelpIconActive;
            Append(help);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            slider.Update(gameTime);
            brew.Update(gameTime);
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

using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using static PotionCraft.Assets;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.LanguageHelper;
using PotionCraft.Content.UI.DialogueUI;
using Terraria.ModLoader.UI.Elements;
using Terraria.ModLoader;
using System;
using PotionCraft.Content.Items;


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

        private Button help;

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
            delete.OnClike = brewPotionState.ClearAll;
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
            slider.onChange = () =>
            {
                if(brewPotionState.PreviewPotion is not null)
                brewPotionState.PreviewPotion.stack = slider.value;
            };
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
                brewPotionState.CreatPotion.AutoUse = !autouse.Value;
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
                brewPotionState.CreatPotion.CanEditor = !potionlock.Value;
                potionlock.Rectangle = potionlock.Value ? new Rectangle(62, 0, 18, 18) : new Rectangle(80, 0, 18, 18); 
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
                packing.Rectangle = packing.Value ? new Rectangle(0, 0, 18, 18) : new Rectangle(20, 0, 18, 18); 
            };
            BG.Append(packing);

            help = new(Assets.UI.HelpIcon, Color.White,brewPotionState);
            help.Width.Set(32, 0);
            help.Height.Set(32, 0);
            help.Top.Set(240, 0);
            help.Left.Set(360, 0);
            help.OnClike = () =>
            {
                help.Value = !help.Value;
                DialogueState.Activity = help.Value;
            };
            help.HoverTexture = Assets.UI.HelpIconActive;
            Append(help);
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

    public class PotionBaseSelect : PotionElement<BrewPotionState>
    {
        private UIGrid List;

        public PotionBaseSelect(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            List = new UIGrid();
            Width.Set(342f, 0);
            Height.Set(400f, 0);
            List.Width.Set(342, 0);
            List.Height.Set(360, 0);
            Append(List);
        }

        public void Init(BrewPotionState BrewPotionState)
        {
            if (List.Count != 0) return;
            var item = new Item();
            item.SetDefaults(ModContent.ItemType<BasePotion>()+10);
            List.Add(new ItemIcon(BrewPotionState, item) { 
                OnClick = () =>
                {
                    Main.NewText("!!!");
                    BrewPotionState.CreatPotion.texture = item.ModItem.Texture;
                    Main.NewText(BrewPotionState.CreatPotion.texture);
                    BrewPotionState.Refresh();
                }
            });
            ModContent.GetInstance<PotionCraft>().Logger.Debug(item);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.ColorUI, GetDimensions().ToRectangle(), new Rectangle(0, 12, 342, 12), Color.White*A);
            spriteBatch.Draw(Assets.UI.ColorUI, GetDimensions().Position(), new Rectangle(0,0,342,12),Color.White*A);
            spriteBatch.Draw(Assets.UI.ColorUI, GetDimensions().Position() + new Vector2(0,400), new Rectangle(0, 212, 342, 12), Color.White * A);
            base.Draw(spriteBatch);
        }

    }

}

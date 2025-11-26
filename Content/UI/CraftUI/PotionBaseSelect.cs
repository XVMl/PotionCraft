using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI.Elements;
using Terraria.ModLoader;
using PotionCraft.Content.Items;
using PotionCraft.Content.System.AutoLoaderSystem;


namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionBaseSelect : AutoUIState
    {
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        public override bool Isload() => true;

        private UIGrid List;

        public override void OnInitialize()
        {
            Top.Set(300, 0);
            Left.Set(20, 0);
            Active = false;
            A = 1;
            List = new UIGrid();
            Width.Set(342f, 0);
            Height.Set(400f, 0);
            List.Width.Set(342, 0);
            List.Height.Set(360, 0);
            Append(List);
            //TransitionAnimation = () =>
            //{
            //    Init(this);
            //    var top = MathHelper.Lerp(Top.Pixels, Active ? 300 : 270, .05f);
            //    A = MathHelper.Lerp(A, Active ? 1 : 0, .05f);
            //    Top.Set(top, 0);
            //};
        }

        public void Init(PotionBaseSelect potionBaseSelect)
        {
            if (List.Count != 0) return;
            var item = new Item();
            item.SetDefaults(ModContent.ItemType<BasePotion>()+10);
            PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
            state.Active = !state.Active;

            List.Add(new ItemIcon<PotionBaseSelect>(potionBaseSelect,item) { 
                OnClick = () =>
                {
                    PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
                    BrewPotionState brewPotionState = (BrewPotionState)state;
                    brewPotionState.CreatPotion.IconID = item.type;
                    brewPotionState.Refresh();
                }
            });
            ModContent.GetInstance<PotionCraft>().Logger.Debug(item);
        }

        public void Init(PotionBaseSelect BrewPotionState,Item item)
        {
            if (LoaderPotionOrMaterial.PotionList.ContainsValue(item.type))
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
                BrewPotionState brewPotionState = (BrewPotionState)state;
                var data = new MaterialData(item.Name, "", Base.Magic, 1);
                brewPotionState.CreatPotion.Item.useStyle = data.UseStyle;
                brewPotionState.CreatPotion.IconID = item.type;
                brewPotionState.Refresh();
            }

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

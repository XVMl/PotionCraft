using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI.Elements;
using Terraria.ModLoader;
using PotionCraft.Content.System.AutoLoaderSystem;
using System.Collections.Generic;
using Terraria.GameContent;


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
            List.PaddingLeft = 20f;
            List.PaddingRight = 20f;
            List.ListPadding = 40f;

            Append(List);
            //TransitionAnimation = () =>
            //{
            //    Init(this);
            //    var top = MathHelper.Lerp(Top.Pixels, Active ? 300 : 270, .05f);
            //    A = MathHelper.Lerp(A, Active ? 1 : 0, .05f);
            //    Top.Set(top, 0);
            //};
        }

        public override void Update(GameTime gameTime)
        {
            Init(this);
            List.Update(gameTime);
            foreach (var item in List)
            {
                item.Update(gameTime);
            }
        }

        public void Init(PotionBaseSelect potionBaseSelect)
        {
            if (List.Count != 0) return;
            var styles = new List<string>{"Style2","Style3","Style4"};

            styles.ForEach(style =>
            {
                var item = new Item();
                ModContent.TryFind("PotionCraft", style, out ModItem modItem);
                item.SetDefaults(modItem.Type);
                PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
                List.Add(new ItemIcon<PotionBaseSelect>(potionBaseSelect, item)
                {
                    Name = "PotionStyle",
                    OnClick = () =>
                    {
                        PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
                        BrewPotionState brewPotionState = (BrewPotionState)state;
                        brewPotionState.CreatPotion.IconID = item.type;
                        Texture2D value = TextureAssets.Item[item.type].Value;
                        Rectangle frame = ((Main.itemAnimations[item.type] == null) ? value.Frame() : Main.itemAnimations[item.type].GetFrame(value));
                        var _origin = frame.Size() / 2;


                        brewPotionState.Refresh();
                    }
                });
            });

            var itemicon = new ItemIcon<PotionBaseSelect>(potionBaseSelect)
            {
                Slot = true,
                Name = "ItemSlot"
            };
            itemicon.OnClick = itemicon.AddItem;
            List.Add(itemicon);

        }

        private void CustomMaterial(PotionBaseSelect potionBaseSelect)
        {

            //PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
            //BrewPotionState brewPotionState = (BrewPotionState)state;
            //brewPotionState.CreatPotion.IconID = item.type;
            //brewPotionState.Refresh();
        }

        public void Init(PotionBaseSelect BrewPotionState,Item item)
        {
            if (LoaderPotionOrMaterial.Foods.Contains(item.Name))
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

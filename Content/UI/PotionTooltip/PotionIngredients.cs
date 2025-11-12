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
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using static PotionCraft.Content.System.LanguageHelper;
namespace PotionCraft.Content.UI.PotionTooltip
{
    public class PotionIngredients:PotionElement<TooltipUI>
    {
        public UIGrid UIgrid;

        public PotionIngredients(AutoUIState potionCraftState)
        {
            PotionCraftState = null;
            UIgrid = new UIGrid()
            {
                OverflowHidden = true,
            };
            UIgrid.Width.Set(350, 0);
            UIgrid.Height.Set(320, 0);
            UIgrid.PaddingLeft = 30;
            UIgrid.PaddingTop = 10;
            Append(UIgrid);
        }

        public void SetPotionCraftState(TooltipUI tooltipUI,Item item=null)
        {
            PotionCraftState = tooltipUI;
            if (PotionCraftState == null) return;
            BasePotion basePotion = AsPotion(item);
            foreach (var ingredientElement in basePotion.PotionDictionary.Select(ingredient => new IngredientElement(ingredient.Value.BuffId, ingredient.Value.ItemId,ingredient.Value.Counts)))
            {
                UIgrid.Add(ingredientElement);
                UIgrid.RecalculateChildren();
            }

        }
        public void SetPotionCraftState(BasePotion potion)
        {
            if (PotionCraftState is null) return;
            foreach (var ingredientElement in potion.PotionDictionary.Select(ingredient => new IngredientElement(ingredient.Value.BuffId, ingredient.Value.ItemId,ingredient.Value.Counts)))
            {
                UIgrid.Add(ingredientElement);
                UIgrid.RecalculateChildren();
            }

        }
    }

    public class IngredientElement:UIElement
    {
        private int IngredientType;

        private int Count;

        private UIText IngredientName;

        private int IngredientId;

        private int overlenght;

        private float LeftOffset;

        private UIElement Area;
        public IngredientElement(int BuffId, int ingredientType,int count)
        {
            Width.Set(150, 0);
            Height.Set(50, 0);
            IngredientType = ingredientType;
            Count = count;
            var buffname = Lang.GetBuffName(BuffId);

            OverflowHidden = true;
            Area = new();
            Area.Left.Set(50, 0);
            Area.Top.Set(10, 0);
            Area.Width.Set(100, 0);
            Area.Height.Set(50, 0);
            Area.OverflowHidden = true;
            Append(Area);
            IngredientName = new(buffname, .8f)
            {
                OverflowHidden = true,
                TextColor = Deafult
            };
            IngredientName.Top.Set(10f, 0);
            Main.instance.LoadItem(IngredientType);
            IngredientName.Width.Set(100, 0);
            IngredientName.Height.Set(50, 0);
            overlenght = (int)FontAssets.MouseText.Value.MeasureString(buffname).X-100;
            Area.Append(IngredientName);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if (!PotionCraftModPlayer.PotionCraftKeybind.Current)
            {
                LeftOffset = 0;
                return;
            }
            if (overlenght>0 &&LeftOffset<overlenght)
                LeftOffset += 0.3f;
            
            base.DrawChildren(spriteBatch);
            IngredientName.Left.Set(-LeftOffset, 0);
            Utils.DrawBorderString(spriteBatch, Count.ToString(), GetDimensions().ToRectangle().TopLeft() + new Vector2(30, 25), Color.White);
            spriteBatch.Draw(TextureAssets.Item[IngredientType].Value, GetDimensions().ToRectangle().TopLeft() + new Vector2(20, 25), null, Color.White, 0, TextureAssets.Item[IngredientType].Value.Size() / 2, 1f, 0, 0);

        }
    }
}

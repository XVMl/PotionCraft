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
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PotionCraft.Content.UI.PotionTooltip
{
    public class PotionIngredients:PotionElement<TooltipUI>
    {
        public UIGrid UIgrid;

        public PotionIngredients(TooltipUI tooltipUI)
        {
            PotionCraftState = null;
            Width.Set(200, 0);
            Height.Set(200, 0);
            UIgrid = new UIGrid()
            {
                //Width = { Percent = 50f },
                //Height = { Percent = 50f },
                ListPadding = 5f,
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            UIgrid.Width.Set(200, 0);
            UIgrid.Height.Set(200, 0);

            Append(UIgrid);
        }

        public void SetPotionCraftState(TooltipUI tooltipUI,Item item=null)
        {
            PotionCraftState = tooltipUI;
            if (PotionCraftState == null) return;
            BasePotion basePotion = AsPotion(item);
            foreach (var ingredientElement in basePotion.PotionDictionary.Select(ingredient => new IngredientElement(ingredient.Value.ItemId,ingredient.Value.Counts)))
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

        private UIText IngredientTime;

        private UIImage IngredientImage;
        public IngredientElement(int ingredientType,int count)
        {
            Width.Set(200, 0);
            Height.Set(50, 0);
            IngredientType = ingredientType;
            Count = count;
            IngredientTime = new("111")
            {
                HAlign = 0.05f,
                VAlign = 0.05f
            };
            Main.instance.LoadItem(IngredientType);
            IngredientTime.Width.Set(100, 0);
            IngredientTime.Height.Set(30, 0);
            Append(IngredientTime);
            
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            Utils.DrawBorderString(spriteBatch, Count.ToString(), GetDimensions().ToRectangle().TopLeft() + new Vector2(10, 10), Color.White);
            Utils.DrawBorderString(spriteBatch, Lang.GetItemName(IngredientType).Value, GetDimensions().ToRectangle().TopLeft() + new Vector2(10, 0), Color.White);
            spriteBatch.Draw(TextureAssets.Item[IngredientType].Value, GetDimensions().ToRectangle().TopLeft() + new Vector2(10, 0), null, Color.White, 0, TextureAssets.Item[IngredientType].Value.Size() / 2, 1f, 0, 0);
        }
    }
}

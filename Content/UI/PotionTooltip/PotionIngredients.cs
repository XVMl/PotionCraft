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
            foreach (var ingredient in basePotion.PotionDictionary)
            {
                Main.NewText(ingredient.Value.ItemId);
                IngredientElement ingredientElement = new(ingredient.Value.ItemId);
                UIgrid.Add(ingredientElement);
            }

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (PotionCraftState == null) return;
            //spriteBatch.Draw(Assets.UI.Button, UIgrid.GetDimensions().ToRectangle(), Color.White);
            //spriteBatch.Draw(TextureAssets.Item[ItemID.BattlePotion].Value, GetDimensions().ToRectangle().TopLeft(),null, Color.White,0, TextureAssets.Item[ItemID.BattlePotion].Value.Size()/2,1f,0,0);
        }
    }

    public class IngredientElement:UIElement
    {
        private int IngredientType;

        private UIText IngredientTime;
        public IngredientElement(int ingredientType)
        {
            Width.Set(40, 0);
            Height.Set(40, 0);
            IngredientType = ingredientType;
            IngredientTime = new(Lang.GetBuffName(ingredientType));
            IngredientTime.HAlign= 0.5f;
            IngredientTime.VAlign = 0.5f;
            IngredientTime.Width.Set(30, 0);
            IngredientTime.Height.Set(20, 0);
            Append(IngredientTime);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureAssets.Item[IngredientType].Value, GetDimensions().ToRectangle().TopLeft(), null, Color.White, 0, TextureAssets.Item[IngredientType].Value.Size() / 2, 1f, 0, 0);

        }
    }
}

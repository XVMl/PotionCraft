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
using static PotionCraft.Content.System.LanguageHelper;
namespace PotionCraft.Content.UI.PotionTooltip
{
    public class PotionIngredients:PotionElement<TooltipUI>
    {
        public UIGrid UIgrid;

        public PotionIngredients(TooltipUI tooltipUI)
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
        
    }

    public class IngredientElement:UIElement
    {
        private int IngredientType;

        private int Count;

        private UIText IngredientTime;

        private int IngredientId;
        public IngredientElement(int BuffId, int ingredientType,int count)
        {
            Width.Set(150, 0);
            Height.Set(50, 0);
            IngredientId = BuffId;
            IngredientType = ingredientType;
            Count = count;
            IngredientTime = new("",.9f)
            {
                HAlign = 0.05f,
                VAlign = 0.05f
            };
            OverflowHidden = true;
            Main.instance.LoadItem(IngredientType);
            IngredientTime.Width.Set(100, 0);
            IngredientTime.Height.Set(30, 0);
            Append(IngredientTime);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if (!PotionCraftModPlayer.PotionCraftKeybind.Current)
                return;
            var buffname = Lang.GetBuffName(IngredientId);
            //var conut = FontAssets.MouseText.Value.MeasureString(IngredientName);
            //var scale = 1f;
            //if (conut.X > 100)
            //{ 
            //    buffname = buffname.Replace(" ", "\n");
            //    scale = 0.5f;
            //}
            Utils.DrawBorderString(spriteBatch, Count.ToString(), GetDimensions().ToRectangle().TopLeft() + new Vector2(30, 25), Color.White);
            Utils.DrawBorderString(spriteBatch, buffname, GetDimensions().ToRectangle().TopLeft() + new Vector2(50, 15), Deafult);
            spriteBatch.Draw(TextureAssets.Item[IngredientType].Value, GetDimensions().ToRectangle().TopLeft() + new Vector2(20, 25), null, Color.White, 0, TextureAssets.Item[IngredientType].Value.Size() / 2, 1f, 0, 0);

        }
    }
}

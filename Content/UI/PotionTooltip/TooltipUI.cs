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
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.UI.PotionTooltip
{
    /// <summary>
    /// 此类用于修改药水的提示栏
    /// </summary>
    public class TooltipUI : AutoUIState
    {
        public override bool IsLoaded() => Show;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private bool Show;
        private UIElement Area;

        private string currentpotionname = "";

        private PotionIngredients PotionIngredients;
        public override void OnInitialize()
        {
            Area = new();
            Area.Width.Set(300f, 0f);
            Area.Height.Set(300f, 0f);
            Append(Area);
            PotionIngredients = new(this) { 
                HAlign= 0.5f,
                VAlign = 1f,
            };
            Area.Append(PotionIngredients);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = Main.MouseScreen;
            Area.Left.Set(pos.X, 0);
            Area.Top.Set(pos.Y, 0);
            Show = Main.HoverItem.type == ModContent.ItemType<BasePotion>();
            if (Show)
            {
                if (currentpotionname != PotionElement<TooltipUI>.AsPotion(Main.HoverItem).PotionName)
                {
                    PotionIngredients.UIgrid.Clear();
                    currentpotionname = PotionElement<TooltipUI>.AsPotion(Main.HoverItem).PotionName;
                    PotionIngredients.SetPotionCraftState(this, Main.HoverItem);
                }
            }
            else
            {
                PotionIngredients.SetPotionCraftState(null);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Show) return;
            spriteBatch.Draw(Assets.UI.Tooltip, Area.GetDimensions().ToRectangle(), Color.White);
            //BasePotion basePotion = Main.HoverItem;
            
        }
    }
}

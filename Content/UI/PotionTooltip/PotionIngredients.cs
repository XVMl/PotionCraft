using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using PotionCraft.Content.System.PotionBuffEffectIntensity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace PotionCraft.Content.UI.PotionTooltip
{
    public class PotionIngredients:PotionElement<TooltipUI>
    {
        public PotionIngredients(TooltipUI tooltipUI)
        {
            PotionCraftState = null;
            Width.Set(300, 0);
            Height.Set(300, 0);
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (PotionCraftState == null) return;
            ItemSlot.Draw(spriteBatch, ref PotionCraftState.Potion, 21, GetDimensions().Position());

        }
    }
}

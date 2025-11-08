using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Assets;
namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSynopsis: PotionElement<BrewPotionState>
    {
        private ColorSelector colorSelector;
        public PotionSynopsis(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(342, 0);
            Height.Set(414, 0);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            var tex = UITexture("UI1").Value;
            spriteBatch.Draw(tex, GetDimensions().ToRectangle().TopLeft(), Color.White);    
            base.Draw(spriteBatch);
        }

    }
    
    
}

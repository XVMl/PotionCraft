using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Assets;
namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSynopsis: PotionElement<BrewPotionState>
    {
        private Button coloreelectorbutton;

        public PotionSynopsis(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(342, 0);
            Height.Set(414, 0);
            coloreelectorbutton = new(Assets.UI.ColorSelector, Color.White);
            coloreelectorbutton.Width.Set(18, 0);
            coloreelectorbutton.Height.Set(18, 0);
            coloreelectorbutton.Top.Set(290, 0);
            coloreelectorbutton.Left.Set(26, 0);
            coloreelectorbutton.onClike = () =>
                ColorSelector.Active = !ColorSelector.Active;
            Append(coloreelectorbutton);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var tex = UITexture("UI1").Value;
            spriteBatch.Draw(tex, GetDimensions().ToRectangle().TopLeft(), Color.White);    
            base.Draw(spriteBatch);
        }

    }
    
    
}

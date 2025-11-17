using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using System;
using static PotionCraft.Assets;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
namespace PotionCraft.Content.UI.CraftUI
{
    public class Slider : PotionElement<BrewPotionState>
    {
        public int value=1000;

        private float maxvalue=199;

        private bool mouseLeft;

        public UIText text;

        public Action onChange;

        public Slider(BrewPotionState brewPotionState,int width=220)
        {
            Width.Set(width, 0);
            Height.Set(22f, 0);
            PotionCraftState = brewPotionState;
        }

        public Slider(BrewPotionState brewPotionState, string text, int width = 220)
        {
            Width.Set(width, 0);
            Height.Set(22f, 0);
            PotionCraftState = brewPotionState;
            this.text= new UIText(text) 
            { 
                VAlign=.5f
            };
            this.text.Left.Set(-50, 0);
            Append(this.text);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
        }

        public override void Update(GameTime gameTime)
        {
            if (!mouseLeft)
                return;
            maxvalue = Utils.Clamp((int)(CaculateMoney(Main.LocalPlayer) / 1000), 1, 999);
            value = Utils.Clamp((int)((Main.MouseScreen.X - GetDimensions().X) *maxvalue/Width.Pixels), 0, (int)maxvalue);
            onChange?.Invoke();
            if (!Main.mouseLeft)
                mouseLeft = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var offset = Utils.Clamp(value/maxvalue*Width.Pixels,0,Width.Pixels -28);
            spriteBatch.Draw(UITexture("Slider").Value,new Rectangle((int)GetDimensions().X,(int)GetDimensions().Y, 4,22),new Rectangle(0,0,4,22), Color.White);
            spriteBatch.Draw(UITexture("Slider").Value, new Rectangle((int)GetDimensions().X + 4, (int)GetDimensions().Y, (int)Width.Pixels - 8, 22), new Rectangle(10, 0, 1, 22), Color.White);
            spriteBatch.Draw(UITexture("Slider").Value, new Rectangle((int)GetDimensions().X +(int)GetDimensions().Width - 4, (int)GetDimensions().Y, 4, 22), new Rectangle(110, 0, 4, 22),Color.White);
            spriteBatch.Draw(UITexture("Slider").Value, new Rectangle((int)GetDimensions().X + 4, (int)GetDimensions().Y, (int)offset, 22), new Rectangle(4, 0, 2, 22), Color.White);
            var icon = UITexture("Slidericon").Value;
            spriteBatch.Draw(icon, GetDimensions().Position() + new Vector2(offset+14, 11), null, Color.White, 0, icon.Size() / 2, 1, SpriteEffects.None, 0); ;
        }

        public override void LeftMouseDown(UIMouseEvent evt) => mouseLeft = true;

        public static long CaculateMoney(Player player)
        {
            var num = Utils.CoinsCount(out _, player.bank.item);
            var num2 = Utils.CoinsCount(out _, player.bank2.item);
            var num3 = Utils.CoinsCount(out _, player.bank3.item);
            var num4 = Utils.CoinsCount(out _, player.bank4.item);
            var num5 = Utils.CoinsCount(out _, player.inventory, 58, 57, 56, 55, 54);
            var num6 = Utils.CoinsCombineStacks(out _, num, num2, num3, num4,num5);
            return num6;
        }
    }
    
    
}

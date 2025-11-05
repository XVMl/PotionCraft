using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Assets;
using Luminance.Core.Graphics;
using System;
namespace PotionCraft.Content.UI.CraftUI
{
    public class ColorSelector: PotionElement<BrewPotionState>
    {
        private UIElement palette;

        private UIElement select;

        static ManagedShader shader = ShaderManager.GetShader("PotionCraft.ColorSelector");

        private float R = 1;
        private float G = 0;
        private float B = 0;

        public ColorSelector(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(342f, 0);
            Height.Set(188f, 0);
            palette = new UIElement()
            {
                HAlign =0.5f,
                VAlign = 0.5f,
            };
            palette.Width.Set(300, 0);
            palette.Height.Set(150, 0);
            
            select = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 1f,
            };
            select.Width.Set(400, 0);
            select.Height.Set(8, 0);
            Append(select);
            Append(palette);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (Main.MouseScreen.X > palette.GetDimensions().Position().X && Main.MouseScreen.Y > palette.GetDimensions().Position().Y
                && Main.MouseScreen.X < palette.GetDimensions().Position().X + palette.GetDimensions().Width && Main.MouseScreen.Y < palette.GetDimensions().Position().Y + palette.GetDimensions().Height)
                SelectColor();


            if (Main.MouseScreen.X > select.GetDimensions().Position().X && Main.MouseScreen.Y > select.GetDimensions().Position().Y
                && Main.MouseScreen.X < select.GetDimensions().Position().X + select.GetDimensions().Width && Main.MouseScreen.Y < select.GetDimensions().Position().Y + select.GetDimensions().Height)
                SetColor();
        }

        private Color SelectColor()
        {
            var data = (Main.MouseScreen - palette.GetDimensions().Position()) / new Vector2(palette.GetDimensions().Width, palette.GetDimensions().Height);
            var color = new Vector3(1 - data.Y, 1 - data.Y, 1 - data.Y) - ((1 - data.Y) * data.X * (Vector3.One - new Vector3(R, G, B)));
                Main.NewText(new Color(color));
            return new Color(color);
        }

        private Color SetColor()
        {
            float data = 1-(float)((Main.MouseScreen.X - palette.GetDimensions().X) / palette.GetDimensions().Width);
            Vector3 color = new Vector3(.5f, .5f, .5f) + new Vector3(.5f, .5f, .5f) * new Vector3(
                (float)Math.Cos(2 * Math.PI * (data + 0)),
                (float)Math.Cos(2 * Math.PI * (data + 0.33f)),
                (float)Math.Cos(2 * Math.PI * (data + 0.67f))
            );
            R = color.X;
            G = color.Y;
            B =  color.Z;
            Main.NewText(color);
            return new Color(color);
        }


        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            Main.NewText("!!!");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Main.spriteBatch.Draw(UITexture("Ui3").Value, Main.LocalPlayer.position - Main.screenPosition, Color.White);
            Main.spriteBatch.Draw(UITexture("Slider").Value, Main.LocalPlayer.position-Main.screenPosition, Color.White);
            Main.spriteBatch.Draw(UITexture("Slidericon").Value, Main.LocalPlayer.position - Main.screenPosition, Color.White);


            spriteBatch.Draw(UITexture("ColorUI").Value, GetDimensions().ToRectangle().TopLeft(), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone,null, Main.UIScaleMatrix);
            shader.TrySetParameter("R", R);
            shader.TrySetParameter("G", G);
            shader.TrySetParameter("B", B);
            shader.Apply();
            var tex = UITexture("Pixel").Value;
            Main.spriteBatch.Draw(UITexture("ColorUI").Value, palette.GetDimensions().ToRectangle(),  Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            var color = ShaderManager.GetShader("PotionCraft.ColorVariation");
            color.Apply();
            Main.spriteBatch.Draw(UITexture("ColorUI").Value, select.GetDimensions().ToRectangle(), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }
    }
    
    
}

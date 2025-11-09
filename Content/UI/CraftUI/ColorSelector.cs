using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Assets;
using Luminance.Core.Graphics;
using System;
using Newtonsoft.Json.Linq;
namespace PotionCraft.Content.UI.CraftUI
{
    public class ColorSelector: PotionElement<BrewPotionState>
    {
        private UIElement palette;

        private ColorSelectorProgess paletteprogress;

        private UIElement select;

        private ColorSelectorProgess selectprogress;

        static ManagedShader shader = ShaderManager.GetShader("PotionCraft.ColorSelector");
        
        private bool mouseLeft;

        public Color Color = Color.Red;

        public Vector2 mousedata;

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

            paletteprogress = new(Color);
            paletteprogress.Left.Set(100, 0);
            paletteprogress.Top.Set(100, 0);

            select = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 1f,
            };
            select.Width.Set(400, 0);
            select.Height.Set(8, 0);
            selectprogress = new(new Color(R,G,B));
            selectprogress.Left.Set(100, 0);
            selectprogress.Top.Set(300, 0);
            
            Append(select);
            Append(palette);
            Append(paletteprogress);
            Append(selectprogress);
        }

        public override void Update(GameTime gameTime)
        {
            if (!mouseLeft)
                return;
            if (!Main.mouseLeft)
                mouseLeft = false;
            if (Main.MouseScreen.X > palette.GetDimensions().Position().X && Main.MouseScreen.Y > palette.GetDimensions().Position().Y
                && Main.MouseScreen.X < palette.GetDimensions().Position().X + palette.GetDimensions().Width && Main.MouseScreen.Y < palette.GetDimensions().Position().Y + palette.GetDimensions().Height)
                SelectColor(); 

            if (Main.MouseScreen.X > select.GetDimensions().Position().X && Main.MouseScreen.Y > select.GetDimensions().Position().Y
                && Main.MouseScreen.X < select.GetDimensions().Position().X + select.GetDimensions().Width && Main.MouseScreen.Y < select.GetDimensions().Position().Y + select.GetDimensions().Height)
                SetColor();
        }
        //TODO 完善功能
        private void SelectColor()
        {   
            paletteprogress.Left.Set(Main.MouseScreen.X - palette.Left.Pixels,0);
            paletteprogress.Top.Set(Main.MouseScreen.Y - palette.Top.Pixels, 0);
            var data = (Main.MouseScreen - palette.GetDimensions().Position()) / new Vector2(palette.GetDimensions().Width, palette.GetDimensions().Height);
            var color = new Vector3(1 - data.Y, 1 - data.Y, 1 - data.Y) - ((1 - data.Y) * data.X * (Vector3.One - new Vector3(R, G, B)));
            Color = new Color(color);
            paletteprogress.Color = Color;
            mousedata = data;
        }

        private void SetColor()
        {
            selectprogress.Left.Set(Main.MouseScreen.X - select.Left.Pixels, 0);
            selectprogress.Top.Set(Main.MouseScreen.Y - select.Top.Pixels, 0);
            float data = 1 - (float)((Main.MouseScreen.X - palette.GetDimensions().X) / palette.GetDimensions().Width);
            Vector3 color = new Vector3(.5f, .5f, .5f) + new Vector3(.5f, .5f, .5f) * new Vector3(
                (float)Math.Cos(2 * Math.PI * (data + 0)),
                (float)Math.Cos(2 * Math.PI * (data + 0.33f)),
                (float)Math.Cos(2 * Math.PI * (data + 0.67f))
            );
            R = color.X;
            G = color.Y;
            B =  color.Z;
            selectprogress.Color = new Color(R, G, B);

            Color = new Color(new Vector3(1 - mousedata.Y, 1 - mousedata.Y, 1 - mousedata.Y) - ((1 - mousedata.Y) * mousedata.X * (Vector3.One - new Vector3(R, G, B))));
            paletteprogress.Color = Color;
        }

        public override void LeftMouseDown(UIMouseEvent evt) => mouseLeft = true;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(UITexture("ColorUI").Value, GetDimensions().ToRectangle().TopLeft(), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone,null, Main.UIScaleMatrix);
            shader.TrySetParameter("R", R);
            shader.TrySetParameter("G", G);
            shader.TrySetParameter("B", B);
            shader.Apply();
            Main.spriteBatch.Draw(UITexture("ColorUI").Value, palette.GetDimensions().ToRectangle(),  Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            var color = ShaderManager.GetShader("PotionCraft.ColorVariation");
            color.Apply();
            Main.spriteBatch.Draw(UITexture("ColorUI").Value, select.GetDimensions().ToRectangle(), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }
    }
    
    public class ColorSelectorProgess:UIElement
    {
        public Color Color;

        private UIElement ShouwColor;

        public ColorSelectorProgess(Color color)
        {
            Color = color;
            ShouwColor = new()
            {
                VAlign = .5f,
                HAlign = .5f
            };
            Append(ShouwColor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Assets.UI.Circular, GetDimensions().Position(), null, Color.White, 0, new Vector2(15, 15), 0.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(Assets.UI.Circular, ShouwColor.GetDimensions().Position(), null, Color, 0, new Vector2(15, 15), 0.4f, SpriteEffects.None, 0);
        }
    }


}

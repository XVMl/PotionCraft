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
    public class ColorSelector : AutoUIState
    {
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        public override bool Isload() => true;

        private UIElement palette;

        private ColorSelectorProgess paletteprogress;

        private UIElement select;

        private ColorSelectorProgess selectprogress;

        static ManagedShader shader;

        private bool palettemouseLeft;

        private bool selectmouseLeft;

        public static Color Color = Color.White;

        private Vector2 mousedata;

        private Vector2 Offset = Vector2.Zero;

        private float R = 1;
        private float G = 0;
        private float B = 0;

        public override void OnInitialize()
        {
            Width.Set(342f, 0);
            Height.Set(224f, 0);
            Top.Set(300, 0);
            Left.Set(20, 0);
            palette = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 0.4f,
            };
            palette.Width.Set(300, 0);
            palette.Height.Set(150, 0);

            paletteprogress = new(Color)
            {
                HAlign = 0f,
                VAlign = 0
            };

            select = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = .9f,
            };
            select.Width.Set(300, 0);
            select.Height.Set(8, 0);
            selectprogress = new(new Color(R, G, B));
            selectprogress.VAlign = .4f;
            TransitionAnimation = () =>
            {
                //var top = MathHelper.Lerp(Top.Pixels, Active ? 300 : 270, .1f);
                //Top.Set(top, 0);
                A = MathHelper.Lerp(A, Active ? 1 : 0, .05f);
            };
            Append(select);
            Append(palette);
            palette.Append(paletteprogress);
            select.Append(selectprogress);

        }

        public override void Update(GameTime gameTime)
        {

            if (palettemouseLeft)
                SelectColor();

            if (selectmouseLeft)
                SetColor();

            if (!selectmouseLeft && !palettemouseLeft && GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()) && Main.mouseLeft)
            {
                if (Offset == Vector2.Zero)
                    Offset = Main.MouseScreen - GetDimensions().Position();
                Left.Set((int)(Main.MouseScreen - Offset).X, 0);
                Top.Set((int)(Main.MouseScreen - Offset).Y, 0);
            }
            else
            {
                Offset = Vector2.Zero;
            }

            if (Main.mouseLeft)
                return;
            palettemouseLeft = false;
            selectmouseLeft = false;

            base.Update(gameTime);
        }

        private void SelectColor()
        {
            var data = Vector2.Clamp((Main.MouseScreen - palette.GetDimensions().Position()) / new Vector2(palette.GetDimensions().Width, palette.GetDimensions().Height),
                Vector2.Zero, Vector2.One); ;

            var color = new Vector3(1 - data.Y, 1 - data.Y, 1 - data.Y) - ((1 - data.Y) * data.X * (Vector3.One - new Vector3(R, G, B)));
            Color = new Color(color);
            paletteprogress.Left.Set(0, data.X);
            paletteprogress.Top.Set(0, data.Y);
            paletteprogress.Color = Color;
            mousedata = data;
        }

        private void SetColor()
        {
            float data = Utils.Clamp(1 - (float)((Main.MouseScreen.X - select.GetDimensions().X) / select.GetDimensions().Width), 0, 1);
            Vector3 color = new Vector3(.5f, .5f, .5f) + new Vector3(.5f, .5f, .5f) * new Vector3(
                (float)Math.Cos(2 * Math.PI * (data + 0)),
                (float)Math.Cos(2 * Math.PI * (data + 0.33f)),
                (float)Math.Cos(2 * Math.PI * (data + 0.67f))
            );
            R = color.X;
            G = color.Y;
            B = color.Z;
            selectprogress.Color = new Color(R, G, B);
            selectprogress.Left.Set(0, (1 - data));

            Color = new Color(new Vector3(1 - mousedata.Y, 1 - mousedata.Y, 1 - mousedata.Y) - ((1 - mousedata.Y) * mousedata.X * (Vector3.One - new Vector3(R, G, B))));
            paletteprogress.Color = Color;
        }

        public override void LeftMouseDown(UIMouseEvent evt) 
        {
            if (!Active)
                return;

            if (Main.MouseScreen.X > palette.GetDimensions().Position().X && Main.MouseScreen.Y > palette.GetDimensions().Position().Y
                && Main.MouseScreen.X < palette.GetDimensions().Position().X + palette.GetDimensions().Width && Main.MouseScreen.Y < palette.GetDimensions().Position().Y + palette.GetDimensions().Height)
                palettemouseLeft = true;

            if (Main.MouseScreen.X > select.GetDimensions().Position().X && Main.MouseScreen.Y > select.GetDimensions().Position().Y
            && Main.MouseScreen.X < select.GetDimensions().Position().X + select.GetDimensions().Width && Main.MouseScreen.Y < select.GetDimensions().Position().Y + select.GetDimensions().Height)
                selectmouseLeft = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
                return;
            shader ??= ShaderManager.GetShader("PotionCraft.ColorSelector");
            spriteBatch.Draw(UITexture("ColorUI").Value, GetDimensions().ToRectangle().TopLeft(), Color.White * A);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone,null, Main.UIScaleMatrix);
            shader.TrySetParameter("R", R);
            shader.TrySetParameter("G", G);
            shader.TrySetParameter("B", B);
            shader.TrySetParameter("alapha", A);
            shader.Apply();
            Main.spriteBatch.Draw(UITexture("ColorUI").Value, palette.GetDimensions().ToRectangle(),  Color.White );
            
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            var color = ShaderManager.GetShader("PotionCraft.ColorVariation");
            color.TrySetParameter("alapha", A);
            color.Apply();
            Main.spriteBatch.Draw(UITexture("ColorUI").Value, select.GetDimensions().ToRectangle(), Color.White * A);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            base.Draw(spriteBatch);
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

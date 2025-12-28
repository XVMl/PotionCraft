using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class Button<T> : PotionElement<T> where T : AutoUIState
{
    public Asset<Texture2D> Texture;

    public Asset<Texture2D> HoverTexture;

    private Asset<Texture2D> _asset;

    public Rectangle Rectangle = Rectangle.Empty;

    public Action OnClike;

    public Action OnHover;

    public Action OnActive;

    public string Text;

    public float Scale;

    public bool Value;

    public bool Rotation;

    public Color Color;

    public Color Iconcolor = Color.White;
    public Button(Asset<Texture2D> texture2D ,Color color, T State, string text ="",float scale=1)
    {
        Texture  = texture2D;
        PotionCraftState = State;
        this.Text = text;
        this.Color = color;
        this.Scale = scale;
        _asset = texture2D;
    }
    public Button(Asset<Texture2D> texture2D, Color color, Rectangle rectangle, T State, string text = "", float scale = 1)
    {
        Texture = texture2D;
        Rectangle = rectangle;
        PotionCraftState = State;
        this.Text = text;
        this.Color = color;
        this.Scale = scale;
        _asset = texture2D;
    }


    public override void LeftClick(UIMouseEvent evt)
    {
        if (!Active)
            return;
        base.LeftClick(evt);
        OnClike?.Invoke();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        SoundEngine.PlaySound(SoundID.MenuClose);

        if (HoverTexture is not null && Active)
            _asset = HoverTexture;
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        _asset = Texture;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Rotation)
            spriteBatch.Draw(Assets.UI.Icon.Value, GetDimensions().Position() + Rectangle.Size()/2, Rectangle, Iconcolor * A, (float)Main.time * .03f, Rectangle.Size() / 2, 1, SpriteEffects.None, 0);
        else if (!Rectangle.IsEmpty)
            spriteBatch.Draw(_asset.Value, GetDimensions().ToRectangle(), Rectangle, Iconcolor * A);
        else
            spriteBatch.Draw(_asset.Value, GetDimensions().ToRectangle(), Iconcolor*A);

        var v2 = FontAssets.MouseText.Value.MeasureString(Text);
        Utils.DrawBorderString(spriteBatch, Text,GetDimensions().Position()+ new Vector2(30,(Height.Pixels - v2.Y)/2) , Color*A, 1f);
        base.Draw(spriteBatch);
    }

}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class Button : PotionElement<BrewPotionState>
{
    public Asset<Texture2D> Texture;

    public Rectangle Rectangle =Rectangle.Empty;

    public Action OnClike;

    public string Text;

    public float Scale;

    public bool Value;

    public Color Color;

    public Color Iconcolor = Color.White;
    public Button(Asset<Texture2D> texture2D ,Color color,string text ="",float scale=1)
    {
        Texture  = texture2D;
        this.Text = text;
        this.Color = color;
        this.Scale = scale;
    }
    public Button(Asset<Texture2D> texture2D, Color color, Rectangle rectangle, string text = "", float scale = 1)
    {
        Texture = texture2D;
        Rectangle = rectangle;
        this.Text = text;
        this.Color = color;
        this.Scale = scale;
    }
    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        OnClike?.Invoke();
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        SoundEngine.PlaySound(SoundID.MenuClose);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if(!Rectangle.IsEmpty)
            spriteBatch.Draw(Texture.Value, GetDimensions().ToRectangle(),Rectangle ,Iconcolor);
        else
            spriteBatch.Draw(Texture.Value, GetDimensions().ToRectangle(), Iconcolor);
        Utils.DrawBorderString(spriteBatch, Text,GetDimensions().Position()+new Vector2(30,Height.Pixels/2) , Color, 0.75f);
        base.Draw(spriteBatch);
    }

}
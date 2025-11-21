using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace PotionCraft.Content.UI.CraftUI;

public class Text<T>:PotionElement<T> where T : AutoUIState
{
    
    private UIText _text;

    private string _currentText;
    
    private string _showText;

    private int _pos;
    public Text(T potionCraftState)
    {
        PotionCraftState = potionCraftState;
        _text = new("");
        Width.Set(400,0);
        Height.Set(100,0);
    }

    public void SetText(string text)
    {
        _showText = text;
        _pos = 0;
        _currentText = string.Empty;
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        _currentText = _showText;
        _pos = _showText.Length;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (Main.GameUpdateCount % 10 == 1 && _showText != null && _pos <_showText.Length) 
            _currentText += _showText[_pos++];
        
        _text.SetText(_currentText);
    }
    
    // public override void Draw(SpriteBatch spriteBatch)
    // {
    //     base.Draw(spriteBatch);
    //     ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, currenttext, GetDimensions().Position(),Color.White, 0f, Vector2.Zero, Vector2.One);
    // }
}
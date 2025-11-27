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

    private string _currentText="";
    
    private string _showText="";

    private int _pos;

    private int count;
    public Text(T potionCraftState)
    {
        PotionCraftState = potionCraftState;
        _text = new("",1.2f);
        //_text.TextColor = Color.Black;
        _text.Left.Set(50, 0);
        _text.Top.Set(40, 0);
        _text.VAlign = .5f;
        Append(_text);
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
        if (_currentText.Equals(_showText)) 
            return;
        _currentText = _showText;
        _pos = _showText.Length;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (count == 1 && _showText != null && _pos <_showText.Length) 
            _currentText += _showText[_pos++];
        count = (count + 1) % 8;
        _text.SetText(_currentText);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        //ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, currenttext, GetDimensions().Position(), Color.White, 0f, Vector2.Zero, Vector2.One);
    }
}
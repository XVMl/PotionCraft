using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class TipsElement<T>:PotionElement<T> where T : AutoUIState
{
    
    private UIText text;

    private UIElement Area;

    private string current;
    
    private string showtext;

    private int pos;
    public TipsElement(T potioncraftState)
    {
        PotionCraftState = potioncraftState;
        text = new("");
        Width.Set(400,0);
        Height.Set(200,0);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!PotionCraftState.Active()) return;
        var s = CurrentElement;
        if (!showtext.Equals(s))
        {
            showtext = LanguageHelper.TryGetLanguagValue(CurrentElement);
            pos = 0;
            current = string.Empty;
        }
        if (Main.GameUpdateCount % 10 == 1 && showtext != null && pos <showtext.Length)
        {
            current += showtext[pos];
            pos++;
        }
        text.SetText(current);
        
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
    }
}
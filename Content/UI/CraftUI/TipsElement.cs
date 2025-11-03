using System;
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
        var s = LanguageHelper.TryGetLanguagValue(CurrentElement);
        if (!showtext.Equals(s))
        {
            showtext = s;
            current = string.Empty;
        }
        if (Main.GameUpdateCount % 10 == 1)
            current +=showtext;
        text.SetText(current);
        
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
    }
}
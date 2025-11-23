using System;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

using static PotionCraft.Content.System.LanguageHelper;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueElement:PotionElement<DialogueState>
{
    private Text<DialogueState> _dialogue;

    private UIElement _dialoguearea;
    
    private string _currentElement="";
    
    public DialogueElement(DialogueState dialogueState)
    {
        PotionCraftState = dialogueState;
        TransitionAnimation = () =>
        { 
            var top = MathHelper.Lerp(Top.Pixels, Active ? 0 : 36, .05f);
            Top.Set(top, 0);
        };
        _dialogue =new Text<DialogueState>(dialogueState);
        _dialoguearea = new UIElement();
        _dialoguearea.Width.Set(424, 0);
        _dialoguearea.Height.Set(108, 0);
        Append(_dialoguearea);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        _dialogue.LeftClick(evt);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!_currentElement.Equals(CurrentElement)) return;
        var text = TryGetLanguagValue($"Dialogue.{CurrentElement}");
        _dialogue.SetText(text);
        _currentElement = CurrentElement;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.UI.Dialogue.Value, _dialoguearea.GetDimensions().Position(),Color.White);

        //var offset = (float)Math.Abs(Math.Sin(Main.GameUpdateCount % 10))*2;
        //spriteBatch.Draw(Assets.UI.Dialogue.Value, _dialoguearea.GetDimensions().Position()+ new Vector2(0,offset),null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}
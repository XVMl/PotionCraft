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
        Height.Set(176, 0);
        Width.Set(626, 0);
        PotionCraftState = dialogueState;
        //TransitionAnimation = () =>
        //{ 
        //    var top = MathHelper.Lerp(Top.Pixels, Active ? 0 : 36, .05f);
        //    Top.Set(top, 0);
        //};
        _dialogue =new Text<DialogueState>(dialogueState);
        _dialogue.Left.Set(20,0);
        Append(_dialogue);
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
        _dialogue.Update(gameTime);
        if ((_currentElement == AutoUIState.CurrentElement)|| string.IsNullOrEmpty(AutoUIState.CurrentElement))
            return;
        var text = TryGetLanguagValue($"Dialogue.Prompt.{AutoUIState.CurrentElement}");
        
        _dialogue.SetText(FontAssets.MouseText.Value.CreateWrappedText(text,150));
        _currentElement = AutoUIState.CurrentElement;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.UI.DialogueUI.Value, GetDimensions().Position(), Color.White);
        base.Draw(spriteBatch);
        spriteBatch.Draw(Assets.NPCs.Hajimi, GetDimensions().Position()+new Vector2(460,38), Color.White);
        spriteBatch.Draw(Assets.NPCs.Emoji2, GetDimensions().Position() + new Vector2(460, 36), Color.White);
        //if (Main.GameUpdateCount % 20U >= 10U)
        //    return;
        //spriteBatch.Draw(Assets.NPCs.Emoji1, GetDimensions().Position() + new Vector2(460, 36), Color.White);
    }
}
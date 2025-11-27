using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueState:AutoUIState
{

    public static bool Activity;

    public override bool Isload() => true;
    
    public override string LayersFindIndex => "Vanilla: Mouse Text";

    private DialogueElement dialogueElement;
    
    public override void OnInitialize()
    {
        //Width.Set(500, 0);
        //Height.Set(200, 0);
        HAlign = .5f;
        Top.Set(100, 0);
        dialogueElement = new(this);
        dialogueElement.HAlign = .5f;
        Append(dialogueElement);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        dialogueElement.Update(gameTime);
    }
}
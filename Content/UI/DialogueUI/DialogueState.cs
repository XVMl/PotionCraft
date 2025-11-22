using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueState:AutoUIState
{
    public override bool Active() => Activity;

    public bool Activity;

    public override bool Isload() => true;
    
    public override string LayersFindIndex => "Vanilla: Mouse Text";

    private DialogueElement dialogueElement;
    
    public override void OnInitialize()
    {
        dialogueElement = new(this);
        Append(dialogueElement);
    }
}
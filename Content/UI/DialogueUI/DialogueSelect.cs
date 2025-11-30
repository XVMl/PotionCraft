
using Microsoft.Xna.Framework;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueSelect:PotionElement<DialogueState>
{
    private Button<DialogueState> Help;

    private Button<DialogueState> Question;

    DialogueState DialogueState;
    public DialogueSelect(DialogueState dialogueState)
    {
        PotionCraftState = dialogueState;
        DialogueState = dialogueState;
        Height.Set(250, 0);
        Width.Set(436, 0);

    }

    public void Init(DialogueState dialogueState)
    {
        Help = new(Assets.UI.DialogueElement, Color.White, dialogueState);
        Help.Name = "HelpButton";
        Help.Width.Set(436, 0);
        Help.Height.Set(72, 0);
        Help.OnClike = () =>
        {
            Main.NewText("!!!!");
        };
        Help.HoverTexture = Assets.UI.DialogueElementActive;
        Append(Help);

        Question = new(Assets.UI.DialogueElement, Color.White, dialogueState);
        Question.Name = "QuestionButton";
        Question.Width.Set(436, 0);
        Question.Height.Set(72, 0);
        Question.VAlign = 1f;
        Question.OnClike = () =>
        {
            dialogueState.question.Active = true;
        };
        Question.HoverTexture = Assets.UI.DialogueElementActive;
        Append(Question);

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Active)
            RemoveAllChildren();
        else
            Init(DialogueState);
    }

}
    

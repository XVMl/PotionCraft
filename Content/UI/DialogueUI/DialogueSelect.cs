
using Microsoft.Xna.Framework;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using Terraria;
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
        Help = new(Assets.UI.DialogueElement, Color.White, dialogueState,"药剂示例");
        Help.Name = "HelpButton";
        Help.Width.Set(436, 0);
        Help.Height.Set(72, 0);
        Help.OnClike = () =>
        {
            Active = false;
            PotionCraftUI.UIstate.TryGetValue(nameof(ExamplePotion), out var state);
            state.Active = true;
            ExamplePotion example = (ExamplePotion)state;
            example.Init(example);
        };
        Help.HoverTexture = Assets.UI.DialogueElementActive;
        Append(Help);

        Question = new(Assets.UI.DialogueElement, Color.White, dialogueState,"炼药挑战");
        Question.Name = "QuestionButton";
        Question.Width.Set(436, 0);
        Question.Height.Set(72, 0);
        Question.VAlign = 1f;
        Question.OnClike = () =>
        {
            Active = false;
            PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);
            state.Active = true;
            ((DialogueQuestion)state).InitPreItem(4);
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
    


using log4net.Core;
using Microsoft.Xna.Framework;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using Terraria;
using Terraria.ModLoader.UI.Elements;
namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueSelect:PotionElement<DialogueState>
{
    private Button<DialogueState> Help;

    private Button<DialogueState> Question;

    private Button<DialogueState> level1;

    private Button<DialogueState> level2;
    
    private Button<DialogueState> level3;

    private Button<DialogueState> level4;
    
    private UIGrid iGrid;

    DialogueState DialogueState;
    public DialogueSelect(DialogueState dialogueState)
    {
        PotionCraftState = dialogueState;
        DialogueState = dialogueState;
        Height.Set(250, 0);
        Width.Set(436, 0);
        iGrid.Width.Set(Parent.Width.Pixels, 0);
        iGrid.Height.Set(Parent.Height.Pixels, 0);
        Append(iGrid);

        Help = new(Assets.UI.DialogueElement, Color.White, dialogueState, "药剂示例");
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
        //Append(Help);
        iGrid.Add(Help);

        Question = new(Assets.UI.DialogueElement, Color.White, dialogueState, "炼药挑战");
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
        //Append(Question);
        iGrid.Add(Question);


        level1 = new Button<DialogueState>(Assets.UI.DialogueElement, Color.White, dialogueState, "简单")
        {
            Name = "SelectLevel",
            OnClike = () =>
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);
                ((DialogueQuestion)state).InitPreItem(4);
                iGrid.Clear();
            }
        };
        level1.Width.Set(436, 0);
        level1.Height.Set(72, 0);
        level1.HoverTexture = Assets.UI.DialogueElementActive;

        level2 = new Button<DialogueState>(Assets.UI.DialogueElement, Color.White, dialogueState, "简单")
        {
            Name = "SelectLevel",
            OnClike = () =>
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);
                ((DialogueQuestion)state).InitPreItem(4);
                iGrid.Clear();
            }
        };
        level2.Width.Set(436, 0);
        level2.Height.Set(72, 0);
        level2.HoverTexture = Assets.UI.DialogueElementActive;

        level3 = new Button<DialogueState>(Assets.UI.DialogueElement, Color.White, dialogueState, "简单")
        {
            Name = "SelectLevel",
            OnClike = () =>
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);
                ((DialogueQuestion)state).InitPreItem(4);
                iGrid.Clear();
            }
        };
        level3.Width.Set(436, 0);
        level3.Height.Set(72, 0);
        level3.HoverTexture = Assets.UI.DialogueElementActive;

        level4 = new Button<DialogueState>(Assets.UI.DialogueElement, Color.White, dialogueState, "简单")
        {
            Name = "SelectLevel",
            OnClike = () =>
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);
                ((DialogueQuestion)state).InitPreItem(4);
                iGrid.Clear();
            }
        };
        level4.Width.Set(436, 0);
        level4.Height.Set(72, 0);
        level4.HoverTexture = Assets.UI.DialogueElementActive;

    }

    public void Init(DialogueState dialogueState)
    {
        if (iGrid.Count != 0)
            return;
        iGrid.Add(Help);
        iGrid.Add(Question);
    }

    private void SelectDifficulty(int level)
    {
        PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);

        if (Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().remainingTime > 0)
            goto End;

        iGrid.Clear();

        iGrid.Add(level1);
        iGrid.Add(level2); 
        iGrid.Add(level3); 
        iGrid.Add(level4);

        End:

        Active = false;
        state.Active = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Active)
            iGrid.Clear();
        else
            Init(DialogueState);
    }

}
    

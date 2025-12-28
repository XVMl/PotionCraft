using Luminance.Common.Utilities;
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
    
    private UIGrid iGrid;

    DialogueState DialogueState;
    public DialogueSelect(DialogueState dialogueState)
    {
        PotionCraftState = dialogueState;
        DialogueState = dialogueState;
        Height.Set(450, 0);
        Width.Set(436, 0);
        iGrid = new();
        iGrid.Height.Set(450, 0);
        iGrid.Width.Set(436, 0);
        iGrid.ListPadding = 50f;
        Append(iGrid);

        Help = new(Assets.UI.DialogueElement, Color.White, dialogueState, "药剂示例");
        Help.Name = "HelpButton";
        Help.Width.Set(426, 0);
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
        iGrid.Add(Help);

        Question = new(Assets.UI.DialogueElement, Color.White, dialogueState, "炼药挑战");
        Question.Name = "QuestionButton";
        Question.Width.Set(436, 0);
        Question.Height.Set(72, 0);
        Question.OnClike = () =>
        {
            SelectDifficulty(dialogueState);
        };
        Question.HoverTexture = Assets.UI.DialogueElementActive;
        //Append(Question);
        iGrid.Add(Question);

    }

    public void Init(DialogueState dialogueState)
    {
        if (iGrid.Count != 0)
            return;
        iGrid.Add(Help);
        iGrid.Add(Question);
    }

    public void SelectDifficultyButton(DialogueState dialogueState)
    {
        for(int i=1;i<=4;i++)
        {
            var button = new Button<DialogueState>(Assets.UI.DialogueElement, Color.White, dialogueState, "简单")
            {
                Name = "SelectLevel",
                OnClike = () =>
                {
                    if (!string.IsNullOrEmpty(Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName))
                        return;
                    PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);
                    state.Active = true;
                    Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName =LanguageHelper.CreatQuestion(i);
                    ((DialogueQuestion)state).InitPreItem();
                    Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().remainingTime = Utilities.MinutesToFrames(6);
                    iGrid.Clear();
                }
            };
            button.Width.Set(436, 0);
            button.Height.Set(72, 0);
            button.HoverTexture = Assets.UI.DialogueElementActive;
            iGrid.Add(button);
        }
        
    }

    private void SelectDifficulty(DialogueState dialogueState)
    {
        PotionCraftUI.UIstate.TryGetValue(nameof(DialogueQuestion), out var state);

        if (Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().remainingTime > 0)
        {
            
            state.Active = true;
            Main.NewText(Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName);
            ((DialogueQuestion)state).InitPreItem();
            return;
        }

        iGrid.Clear();

        SelectDifficultyButton(dialogueState);
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
    

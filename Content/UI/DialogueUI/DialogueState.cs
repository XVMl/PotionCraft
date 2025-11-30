using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueState:AutoUIState
{

    public static bool Activity;

    public override bool Isload() => true;
    
    public override string LayersFindIndex => "Vanilla: Mouse Text";

    private DialogueElement dialogueElement;

    private DialogueSelect dialogueSelect;

    public DialogueQuestion question;

    private Button<DialogueState> crucible;

    private Button<DialogueState> Hajimi;

    public override void OnInitialize()
    {
        Width.Set(700, 0);
        Height.Set(600, 0);
        HAlign = .5f;
        Top.Set(80, 0);
        TransitionAnimation = () =>
        {
            var top = MathHelper.Lerp(Top.Pixels, Active ? 100 : 80, .1f);
            A = MathHelper.Lerp(A, Active ? 1 : 0, .05f);
            Top.Set(top, 0);
        };
        
        dialogueElement = new(this);
        dialogueElement.HAlign = .5f;
        Append(dialogueElement);


        dialogueSelect = new(this);
        dialogueSelect.A = 0;
        dialogueSelect.Active =false;
        dialogueSelect.HAlign = .5f;
        dialogueSelect.TransitionAnimation = () =>
        {
            var top = MathHelper.Lerp(dialogueSelect.Top.Pixels, dialogueSelect.Active ? 300 : 270, .05f);
            dialogueSelect.A = MathHelper.Lerp(dialogueSelect.A, dialogueSelect.Active ? 1 : 0, .05f);
            dialogueSelect.Top.Set(top, 0);
        };
        dialogueSelect.Top.Set(270, 0);
        Append(dialogueSelect);

        question = new(this);
        question.A = 0;
        question.Active = false;
        question.HAlign = .5f;
        question.TransitionAnimation = () =>
        {
            var top = MathHelper.Lerp(question.Top.Pixels, question.Active ? 250 : 220, .05f);
            question.A = MathHelper.Lerp(question.A, question.Active ? 1 : 0, .05f);
            question.Top.Set(top, 0);
        };
        question.Top.Set(220, 0);
        Append(question);

        crucible = new(Assets.UI.CrucibleIcon, Color.White, this);
        crucible.Name = "CrucibleButton";
        crucible.Width.Set(32, 0);
        crucible.Height.Set(32, 0);
        crucible.Top.Set(145, 0);
        crucible.OnClike = () =>
        {
            PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
            state.Active = true;

            dialogueSelect.Active = false;
            dialogueSelect.Top.Set(270, 0);
            dialogueSelect.A = 0;
        };
        crucible.HoverTexture = Assets.UI.CrucibleIconActive;
        Append(crucible);

        Hajimi = new(Assets.UI.HajimiIcon, Color.White, this);
        Hajimi.Name = "HajimiButton";
        Hajimi.Width.Set(40, 0);
        Hajimi.Height.Set(34, 0);
        Hajimi.Top.Set(100, 0);
        Hajimi.OnClike = () =>
        {
            PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
            state.Active = false;
            ((BrewPotionState)state).InitPotion();
            PotionCraftUI.UIstate.TryGetValue(nameof(ColorSelector), out var colorselect);
            colorselect.Active = false;
            //PotionCraftUI.UIstate.TryGetValue(nameof(DialogueState), out var dialogue);
            //dialogue.Active = false;
            PotionCraftUI.UIstate.TryGetValue(nameof(PotionBaseSelect), out var potionbase);
            potionbase.Active = false;

            dialogueSelect.Active = true;
            question.InitPreItem(4);

        };
        Hajimi.HoverTexture = Assets.UI.HajimiIconHover;
        Append(Hajimi);

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        dialogueElement.Update(gameTime);
        question.Update(gameTime);
    }
}
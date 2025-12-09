using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static PotionCraft.Content.System.LanguageHelper;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueQuestion :AutoUIState
{
    private ItemIcon<DialogueQuestion> _previewIcon;

    private ItemIcon<DialogueQuestion> _submitIcon;

    private Button<DialogueQuestion> _previewButton;

    private Button<DialogueQuestion> _submitButton;

    private Button<DialogueQuestion> _submit;

    private Text<DialogueQuestion> _question;

    private Button<DialogueQuestion> _close;

    private Button<DialogueQuestion> _contract;

    private Button<DialogueQuestion> _zoom;

    private UIElement iElement;

    private bool compactWindos;

    private float tarHeigth=406;

    private Vector2 Offset = Vector2.Zero;

    public override bool Isload() => true;

    public override string LayersFindIndex => "Vanilla: Mouse Text";
    
    private enum SlotState
    {
        Preview,
        Submit,
    }

    public override void OnInitialize()
    {
        Width.Set(626, 0);
        Height.Set(450, 0);
        Top.Set(600, 0);
        Left.Set(350, 0);
        Active = false;
        Init(this);
    }

    public void Init(DialogueQuestion dialogueQuestion)
    {
        _question = new(dialogueQuestion);
        _question.Left.Set(30, 0);
        _question.Top.Set(20, 0);
        Append(_question);

        _submitIcon = new ItemIcon<DialogueQuestion>(dialogueQuestion)
        { 
            Slot = true,
            Name = "ItemSlot"
        };
        _submitIcon.Top.Set(268, 0);
        _submitIcon.Left.Set(258, 0);
        _submitIcon.OnClick = _submitIcon.AddItem;
        //Append(_submitIcon);

        _previewIcon = new(dialogueQuestion)
        {
            Name = "SynopsisIcon",
            ExtraInformation = true,
        };
        _previewIcon.Top.Set(268, 0);
        _previewIcon.Left.Set(258, 0);
        Append(_previewIcon);

        _previewButton = new(Assets.UI.Icon, Color.White, new Rectangle(98, 0, 18, 18), dialogueQuestion);
        _previewButton.Name = "PreviewButton";
        _previewButton.Width.Set(18, 0);
        _previewButton.Height.Set(18, 0);
        _previewButton.Left.Set(232, 0);
        _previewButton.Top.Set(272, 0);
        _previewButton.Iconcolor = Deafult;
        _previewButton.OnClike = () =>
        {
            if (!_submitIcon.Active)
                return;

            _submitIcon.Active = false;
            RemoveChild(_submitIcon);

            _previewIcon.Active = true;
            Append(_previewIcon);
        };
        Append(_previewButton);

        _submitButton = new(Assets.UI.Icon, Color.White, new Rectangle(118, 0, 18, 18), dialogueQuestion);
        _submitButton.Name = "SubmitButton";
        _submitButton.Width.Set(18, 0);
        _submitButton.Height.Set(18, 0);
        _submitButton.Left.Set(232, 0);
        _submitButton.Top.Set(294, 0);
        _submitButton.Iconcolor = Deafult;
        _submitButton.OnClike = () =>
        {
            if (!_previewIcon.Active)
                return;
            
            _previewIcon.Active = false;
            RemoveChild(_previewIcon);

            _submitIcon.Active = true;
            Append(_submitIcon);
        };
        Append(_submitButton);

        _submit = new(Assets.UI.Icon, Color.White, new Rectangle(138, 0, 18, 18), dialogueQuestion);
        _submit.Name = "Submit";
        _submit.Width.Set(18, 0);
        _submit.Height.Set(18, 0);
        _submit.Left.Set(368, 0);
        _submit.Top.Set(352, 0);
        _submit.Iconcolor = Deafult;
        //_submit.OnClike = () =>
        //{
        //    _coloreelectorbutton.Active = !_coloreelectorbutton.Active;
        //};
        Append(_submit);

        _close = new(Assets.UI.Windos, Color.White, new Rectangle(0, 0, 18, 18), dialogueQuestion);
        _close.Name = "Close";
        _close.Width.Set(18, 0);
        _close.Height.Set(18, 0);
        _close.Left.Set(20, 0);
        _close.Top.Set(20, 0);
        _close.OnClike = () =>
        {
            Active = false;
        };
        Append(_close);

        _contract = new(Assets.UI.Windos, Color.White, new Rectangle(20, 0, 18, 18), dialogueQuestion);
        _contract.Name = "Contract";
        _contract.Width.Set(18, 0);
        _contract.Height.Set(18, 0);
        _contract.Left.Set(45, 0);
        _contract.Top.Set(20, 0);
        _contract.OnClike = () =>
        {
            compactWindos = true;
            CalculateHeight();
        };
        Append(_contract);

        _zoom = new(Assets.UI.Windos, Color.White, new Rectangle(40, 0, 18, 18), dialogueQuestion);
        _zoom.Name = "Zoom";
        _zoom.Width.Set(18, 0);
        _zoom.Height.Set(18, 0);
        _zoom.Left.Set(70, 0);
        _zoom.Top.Set(20, 0);
        _zoom.OnClike = () =>
        {
            compactWindos = false;
            CalculateHeight();
        };
        Append(_zoom);

        iElement = new();
        iElement.Width.Set(268, 0);
        iElement.Height.Set(10,0);
        Append(iElement);
    }

    public void InitPreItem(int level)
    {
        if (!string.IsNullOrEmpty(Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName))
            return;
        Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName =  CreatQuestion(level);
        var item = new Item();
        item = CreatePotionByName(Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName);
        
        _previewIcon.Item = item;
        CalculateHeight();
    }

    public void CalculateHeight()
    {
        var bracket = LanguageManager.Instance.ActiveCulture.Name != "zh-Hans";
        var name = LocationTranslate(Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().PotionName, bracket);
        var text = WrapTextWithColors_ComPact(name, compactWindos ? 250 : 450);
        //var height = FontAssets.MouseText.Value.MeasureString(text.Item1).Y;
        tarHeigth = text.Item2*33 + 250;
        _question.SetText(text.Item1);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _question.Update(gameTime);

        Width.Set(MathHelper.Lerp(Width.Pixels, compactWindos? 348 : 626, .1f), 0);
        Height.Set(MathHelper.Lerp(Height.Pixels, tarHeigth, .1f), 0);

        _previewIcon.Top.Set(MathHelper.Lerp(_previewIcon.Top.Pixels, Height.Pixels - 138, .1f), 0);
        _previewIcon.Left.Set(MathHelper.Lerp(_previewIcon.Left.Pixels, compactWindos ? 120:258, .1f), 0);
        
        _submitIcon.Top.Set(MathHelper.Lerp(_submitIcon.Top.Pixels, Height.Pixels - 138, .1f), 0);
        _submitIcon.Left.Set(MathHelper.Lerp(_submitIcon.Left.Pixels, compactWindos ? 120 : 258, .1f), 0);

        _previewButton.Top.Set(MathHelper.Lerp(_previewButton.Top.Pixels, Height.Pixels - 134, .1f), 0);
        _previewButton.Left.Set(MathHelper.Lerp(_previewButton.Left.Pixels, compactWindos ? 94 : 232, .1f), 0);

        _submitButton.Top.Set(MathHelper.Lerp(_submitButton.Top.Pixels, Height.Pixels - 112, .1f), 0);
        _submitButton.Left.Set(MathHelper.Lerp(_submitButton.Left.Pixels, compactWindos ? 94 : 232, .1f), 0);

        _submit.Top.Set(MathHelper.Lerp(_submit.Top.Pixels, Height.Pixels - 54, .1f), 0);
        _submit.Left.Set(MathHelper.Lerp(_submit.Left.Pixels, compactWindos ? 230 : 368, .1f), 0);

        iElement.Top.Set(MathHelper.Lerp(iElement.Top.Pixels, Height.Pixels - 152, .1f), 0);
        iElement.Left.Set(MathHelper.Lerp(iElement.Left.Pixels, compactWindos ? 32 : 170, .1f), 0);



        if (GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()) && Main.mouseLeft)
        {
            if (Offset == Vector2.Zero)
                Offset = Main.MouseScreen - GetDimensions().Position();

            Left.Set((int)(Main.MouseScreen- Offset).X, 0);
            Top.Set((int)(Main.MouseScreen -Offset).Y, 0);
        }
        else
        {
            Offset = Vector2.Zero;
        }

    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        _question.EndLoadText();
    }

    public static Item CreatePotionByName(string name)
    {
        var item = new Item();
        item.SetDefaults(ModContent.ItemType<BasePotion>());
        var modItem = AsPotion(item);
        var list = name.Split(' ');
        var stack = new Stack<Dictionary<string, PotionData>>();
        foreach (var craft in list)
        {
            switch (craft)
            {
                case " ":
                case "":
                    break;
                case "+":
                    stack.Push(MashUp(stack.Pop(), stack.Pop()));
                    break;
                case "@":
                    stack.Push(Putify(stack.Pop()));
                    break;
                default:
                    BuffID.Search.TryGetId(craft, out var buffid);
                    Item material = new Item();
                    material.SetDefaults(PotionWithBuffList[buffid]);
                    var potiondata = new PotionData(craft, material.type, 1, 1, material.buffType);
                    stack.Push(new Dictionary<string, PotionData>
                    {
                        { craft, potiondata }
                    });
                    break;
            }
        }
        modItem._Name = name;
        modItem.PotionDictionary = stack.Pop();
        return item;
    }

    public static Dictionary<string, PotionData> MashUp(Dictionary<string, PotionData> first, Dictionary<string, PotionData>second)
    {
        foreach (var data in first)
        {
            if (second.TryGetValue(data.Key,out var value))
            {
                value.Counts += data.Value.Counts;
                continue;
            }
            second.TryAdd(data.Key, data.Value);
        }
        return second;
    }

    private static Dictionary<string, PotionData> Putify(Dictionary<string, PotionData> dict)
    {
        foreach (var data in dict)
        {
            data.Value.Counts *= 2;
        }
        return dict;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y, 14, 14), new Rectangle(0, 0, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X+14, (int)GetDimensions().Y, (int)Width.Pixels - 28, 14), new Rectangle(14, 0, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X+ (int)Width.Pixels - 14, (int)GetDimensions().Y, 14, 14), new Rectangle(612, 0, 14, 14), Color.White);

        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y+14, 14, (int)Height.Pixels - 28), new Rectangle(0, 14, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + (int)Width.Pixels - 14, (int)GetDimensions().Y+14, 14, (int)Height.Pixels - 28), new Rectangle(612, 14, 14, 14), Color.White);

        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y + (int)Height.Pixels - 14, 14, 14), new Rectangle(0, 392, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + 14, (int)GetDimensions().Y + (int)Height.Pixels - 14, (int)Width.Pixels - 28, 14), new Rectangle(14, 392, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + (int)Width.Pixels - 14, (int)GetDimensions().Y + (int)Height.Pixels - 14, 14, 14), new Rectangle(612, 392, 14, 14), Color.White);
        
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + 14, (int)GetDimensions().Y + 14, (int)Width.Pixels - 28, (int)Height.Pixels - 28), new Rectangle(14, 14, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, iElement.GetDimensions().ToRectangle(), new Rectangle(170, 252, 268, 10), Color.White);

        base.Draw(spriteBatch);

    }

}
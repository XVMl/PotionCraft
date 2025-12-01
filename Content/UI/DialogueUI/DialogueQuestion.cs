using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using Newtonsoft.Json.Linq;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static PotionCraft.Content.System.LanguageHelper;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueQuestion :AutoUIState
{
    private ItemIcon<DialogueQuestion> _preview;

    private ItemIcon<DialogueQuestion> _submit;

    private Text<DialogueQuestion> _question;

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
        Height.Set(300, 0);
        Active = false;
        Init(this);

    }

    public void Init(DialogueQuestion dialogueQuestion)
    {
        _question = new(dialogueQuestion);
        _question.Left.Set(30, 0);
        Append(_question);
    }

    public void InitPreItem(int level)
    {
        var bracket = LanguageManager.Instance.ActiveCulture.Name != "zh-Hans";
        var question =  CreatQuestion(level);
        //var item = new Item();
        //item.SetDefaults(ModContent.ItemType<BasePotion>());
        //var preitem = AsPotion(item);
        //preitem._Name = question;
        Mod instance = ModContent.GetInstance<PotionCraft>();
        instance.Logger.Debug(question);
        var name = LocationTranslate(question,bracket);
        instance.Logger.Debug(question);
        var text = WrapTextWithColors_ComPact(name,450);
        var height = MeasureString_Cursor(text.Item1);
        Height.Set(200 + height.Y, 0);
        _question.SetText(text.Item1);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _question.Update(gameTime);

        if(GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()) && Main.mouseLeft)
        {
            Left.Set((int)(Main.MouseScreen.X ), 0);
            Top.Set((int)(Main.MouseScreen.Y), 0);
            //var Dvalue = GetDimensions().Position()-Main.MouseScreen;

            //Left.Set((int)(Main.MouseScreen.X + Dvalue.X), 0);
            //Top.Set((int)(Main.MouseScreen.Y + Dvalue.Y), 0);
        }

    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        _question.EndLoadText();
    }

    public static Item CreatePotionByName()
    {
        var item = new Item();
        item.SetDefaults(ModContent.ItemType<BasePotion>());
        var modItem = AsPotion(item);
        var list = modItem._Name.Split(' ');
        var material = new Item();
        foreach (var craft in list)
        {
            switch (craft)
            {
                case "+":
                   BrewPotionState.MashUp(modItem,material);
                    break;
                case "@":
                   BrewPotionState.Putify(modItem,material); 
                    break;
                default:
                    material = new Item();
                    material.SetDefaults(ItemID.Search.GetId(craft));
                    break;
            }
        }
        return item;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.UI.Question, GetDimensions().Position(), new Rectangle(0, 0, 626, 12), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X,(int)GetDimensions().Y+12,626, (int)Height.Pixels-12), new Rectangle(0, 20, 626, 40), Color.White);
        spriteBatch.Draw(Assets.UI.Question, GetDimensions().Position() + new Vector2(0, 248 + Height.Pixels - 260), new Rectangle(0, 248, 626, 158), Color.White);
        base.Draw(spriteBatch);

    }

}
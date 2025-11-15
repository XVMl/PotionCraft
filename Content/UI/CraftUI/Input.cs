using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PotionCraft.Content.System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class Input:PotionElement<BrewPotionState>
{
    public bool Canedit;
        
    public bool Inputting;
        
    public string Currentvalue="";
        
    public List<(string,string)> Recordvalue=new();
        
    public string Showstring="";

    private string SelectColor = LanguageHelper.Deafult_Hex;
        
    public Action Onchange;
    public Input(BrewPotionState brewPotionState,string currentValue=null)
    {
        PotionCraftState = brewPotionState;
        Width.Set(180f, 0f);
        Height.Set(50f, 0f);
        Recordvalue = LanguageHelper.ParseText(currentValue);
    }

    public Input(Action onchange, BrewPotionState brewPotionState,string currentvalue=null )
    {
        PotionCraftState = brewPotionState;
        Recordvalue = LanguageHelper.ParseText(currentvalue);
        Onchange = onchange;
    }
        
    private void InputText()
    {
        //if(!Canedit)
        //    return;
        Inputting = true;
        Main.blockInput = true;
    }

    private void HandleInputText()
    {
        if (Main.keyState.IsKeyDown(Keys.Escape))
        {
            EndInputText();
        }

        PlayerInput.WritingText = true;
        Main.instance.HandleIME();
        //if (!Currentvalue.IsNormalized())
        //    Currentvalue = StListText();
        Main.CurrentInputTextTakerOverride = true;
        Currentvalue = Main.GetInputText(Currentvalue);
    }

    private void EndInputText()
    {
        if (!Inputting)
            return;
        Main.blockInput = false;
        Inputting = false;
        if(!string.IsNullOrEmpty(Currentvalue))
            Recordvalue.Add((SelectColor, Currentvalue));
        Currentvalue = "";
        Showstring = string.Join("", Recordvalue.Select(s=>string.IsNullOrEmpty(s.Item1) ? LanguageHelper.Deafult_Hex : s.Item1.Insert(10,s.Item2)));
        Main.NewText(Showstring);
        Mod mod = ModContent.GetInstance<PotionCraft>();
        mod.Logger.Debug(Showstring);

        Onchange?.Invoke();
    }

    public override void Update(GameTime gameTime)
    {
        if (!PotionCraftState.Active())
            return;
        if (Main.mouseLeft && !IsMouseHovering)
        {
            EndInputText();
        }
        
    }

    private string StListText()
    {
        if (Recordvalue.Count == 0)
            return "";
        var text = Recordvalue.Last().Item2;
        Recordvalue.Remove(Recordvalue.Last());
        return text;
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        if (!PotionCraftState.Active())
            return;
        InputText();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.UI.UI1,GetDimensions().ToRectangle(),new Rectangle(46,276,246,106),Color.White);
        
        Utils.DrawBorderString(spriteBatch, Showstring, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
        var vector2 = GetDimensions().Position() +
                      FontAssets.MouseText.Value.MeasureString(string.Join("", Recordvalue.Select(s => s.Item2)));

        if (!Inputting)
            return;

        HandleInputText();
        //spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
        Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
        Utils.DrawBorderString(spriteBatch, Currentvalue, vector2,
            LanguageHelper.HexToColor(SelectColor), 0.75f, 0f, 0f, -1);
        if (Main.GameUpdateCount % 20U >= 10U)
            return;
        Utils.DrawBorderString(spriteBatch, "|", vector2 + FontAssets.MouseText.Value.MeasureString(Currentvalue), Color.White, 0.75f, 0.0f, 0.0f, -1);
    }

}
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
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class Input:PotionElement<BrewPotionState>
{
    private BrewPotionState _brewPotionState;
    
    public bool Canedit;

    private bool Inputting;

    private string Currentvalue;
        
    public List<(string,string)> Recordvalue=new();
        
    public string Showstring="";
    
    private string SelectColor => LanguageHelper.RGBToHex(_brewPotionState.colorSelector.Color);

    private string TempColor;
    
    public Action Onchange;
    public Input(BrewPotionState brewPotionState,string currentValue=null)
    {
        _brewPotionState = brewPotionState;
        PotionCraftState = brewPotionState;
        Width.Set(180f, 0f);
        Height.Set(50f, 0f);
        Recordvalue = LanguageHelper.ParseText(currentValue);
    }

    public Input(Action onchange, BrewPotionState brewPotionState,string currentvalue=null )
    {
        _brewPotionState = brewPotionState;
        PotionCraftState = brewPotionState;
        Recordvalue = LanguageHelper.ParseText(currentvalue);
        Onchange = onchange;
    }
        
    private void InputText()
    {
        if(!Canedit)
            return;
        Inputting = true;
        Main.blockInput= true;
    }

    private void HandleInputText()
    {
        if (Main.keyState.IsKeyDown(Keys.Escape))
        {
            EndInputText();
        }

        PlayerInput.WritingText = true;
        Main.instance.HandleIME();
        if (string.IsNullOrEmpty(Currentvalue) && Main.inputText.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
            Recordvalue.Remove(Recordvalue.Last());
        
        Currentvalue = Main.GetInputText(Currentvalue);
    }

    private void EndInputText()
    {
        Main.blockInput = false;
        Inputting = false;
        foreach (var word in Currentvalue)
           Recordvalue.Add((SelectColor, word.ToString()));
           
        Showstring = string.Join("", Recordvalue.Select(s=>string.IsNullOrEmpty(s.Item1) ? LanguageHelper.Deafult_Hex : s.Item1.Insert(10,s.Item2)));
        Onchange?.Invoke();
    }

    public override void Update(GameTime gameTime)
    {
        if (!PotionCraftState.Active())
            return;
       
        if (Main.mouseLeft && !IsMouseHovering)
            EndInputText();
        
        if (Inputting)
            HandleInputText();
        
        if (GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()))
            HandleMouseScroll();
    }

    private string StListText()
    {
        if (Recordvalue.Count == 0)
            return "";
        var text = Recordvalue.Last().Item2;
        TempColor = Recordvalue.Last().Item1;
        Recordvalue.Remove(Recordvalue.Last());
        return text;
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        if (!PotionCraftState.Active())
            return;
        InputText();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Utils.DrawBorderString(spriteBatch, Showstring, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
        var vector2 = GetDimensions().Position() + 
                      FontAssets.MouseText.Value.MeasureString(string.Join("",Recordvalue.Select(s=>s.Item2))) ;
        if (Inputting)
        {
            spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
            Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
            Utils.DrawBorderString(spriteBatch, Currentvalue, vector2,
                LanguageHelper.HexToColor(SelectColor), 0.75f, 0f, 0f, -1);
        }
        if (Main.GameUpdateCount % 20U >= 10U)
            return;
        Utils.DrawBorderString(spriteBatch, "|", vector2+FontAssets.MouseText.Value.MeasureString(Currentvalue), Color.White, 0.75f, 0.0f, 0.0f, -1);
    }
        
}
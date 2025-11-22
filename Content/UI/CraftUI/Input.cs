using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Repository.Hierarchy;
using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PotionCraft.Content.System;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using static PotionCraft.Content.System.LanguageHelper;

namespace PotionCraft.Content.UI.CraftUI;

public class Input:PotionElement<BrewPotionState>
{
    private BrewPotionState _brewPotionState;

    public Asset<Texture2D> Asset;

    public bool Canedit;
        
    public bool Inputting;
        
    public string Currentvalue="";
        
    public List<(string,string)> Recordvalue=new();
        
    public string Showstring="";
    
    private Color SelectColor => _brewPotionState.colorSelector.Color;

    private string TempColor="";

    private string TempText="";
    
    public Action Onchange;
    public Input(BrewPotionState brewPotionState,string currentValue=null)
    {
        _brewPotionState = brewPotionState;
        PotionCraftState = brewPotionState;
        Width.Set(180f, 0f);
        Height.Set(50f, 0f);
        Recordvalue = ParseText(currentValue);
        OverflowHidden = true;
    }

    public Input(Action onchange, BrewPotionState brewPotionState,string currentvalue=null )
    {
        _brewPotionState = brewPotionState;
        PotionCraftState = brewPotionState;
        Recordvalue = ParseText(currentvalue);
        Onchange = onchange;
    }
        
    private void InputText()
    {
        if(!Canedit)
            return;
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
        if (string.IsNullOrEmpty(Currentvalue)&&Recordvalue.Count!=0)
            StListText();
        
        Currentvalue = Main.GetInputText(Currentvalue);
    }

    private void EndInputText()
    {
        if (!Inputting)
            return;
        Main.blockInput = false;
        Inputting = false;
        var original = GetUnmodfiedPart(TempText, ref Currentvalue);
        if(!string.IsNullOrEmpty(original))
            Recordvalue.Add((TempColor,original));

        if (!string.IsNullOrEmpty(Currentvalue))
            Recordvalue.Add((RGBToHex(SelectColor), Currentvalue));

        Currentvalue = "";
        TempColor = "";
        TempColor = "";
        Refresh();
        Onchange?.Invoke();
    }

    public void Refresh()
    {
        Showstring = string.Join("", Recordvalue.Select(s => string.IsNullOrEmpty(s.Item1) ? Deafult_Hex.Insert(10,s.Item2) : $"[c/{s.Item1}:{s.Item2}]"));
        Showstring = WrapTextWithColors(Showstring, 180).Item1;
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

    private void StListText()
    {
        if (Recordvalue.Count == 0)
            return ;
        Currentvalue = Recordvalue.Last().Item2;
        TempText = Currentvalue;
        TempColor = Recordvalue.Last().Item1;
        Recordvalue.Remove(Recordvalue.Last());
        Refresh();
    }

    public static string GetUnmodfiedPart(string original,ref string modified)
    {
        var minLength =Math.Min(original.Length, modified.Length);
        for (int i = 0;i< minLength; i++)
        {
            if (original[i] == modified[i])
                continue;
            
            modified = modified[i..];
            return original[..i];
        }

        modified = modified[minLength..];
        return original[..minLength];
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        if (!PotionCraftState.Active())
            return;
        InputText();
    }
    
    public static string GetUnmodifiedPart(string original, string modified)
    {
        var minLength = Math.Min(original.Length, modified.Length);
        for (var i = 0; i < minLength; i++)
        {
            if (original[i] != modified[i])
            {
                return original[..i];
            }
        }

        return original == modified ? original : original[..minLength];
    }
    
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if(Asset is not null)
            spriteBatch.Draw(Asset.Value,GetDimensions().ToRectangle(),new Rectangle(46,276,246,106),Color.White);
        
        Utils.DrawBorderString(spriteBatch, Showstring, GetDimensions().Position(), Color.White, 1f, 0f, 0f, -1);
        var vector2 = GetDimensions().Position() +
                       MeasureString_Cursor(DeleteTextColor_SaveString(Showstring));
        if (!Inputting)
            return;

        HandleInputText();
         Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
        Utils.DrawBorderString(spriteBatch, Currentvalue, vector2,
            Color.White, 1f, 0f, 0f, -1);
        if (Main.GameUpdateCount % 20U >= 10U)
            return;
        Utils.DrawBorderString(spriteBatch, "|", vector2+ new Vector2(FontAssets.MouseText.Value.MeasureString(Currentvalue).X,0), Color.White, 1f, 0.0f, 0.0f, -1);
    }
        
}
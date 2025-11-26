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
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
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
    
    private Color SelectColor => ColorSelector.Color;

    private string TempColor ="FFFFFF";

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
        if (string.IsNullOrEmpty(Currentvalue) && Recordvalue.Count!=0 && Main.inputText.IsKeyDown(Keys.Back))
            StListText();
        
        Currentvalue = Main.GetInputText(Currentvalue);
    }

    private void EndInputText()
    {
        if (!Inputting)
            return;
        Main.blockInput = false;
        Inputting = false;
        var original = GetUnmodifiedPart(TempText, ref Currentvalue);
        if(!string.IsNullOrEmpty(original))
            Recordvalue.Add((TempColor,original));

        if (!string.IsNullOrEmpty(Currentvalue))
            Recordvalue.Add((RGBToHex(SelectColor), Currentvalue));

        Currentvalue = "";
        TempColor = "";
        TempColor = Deafult_Hex;
        Refresh();
        Onchange?.Invoke();
    }

    public void Refresh()
    {
        Showstring = string.Join("", Recordvalue.Select(s => string.IsNullOrEmpty(s.Item1) ? Deafult_Hex.Insert(10,s.Item2) : $"[c/{s.Item1}:{s.Item2}]"));
        Showstring = WrapTextWithColors_ComPact(Showstring, 180).Item1;
        // Mod instance = ModContent.GetInstance<PotionCraft>();
        // instance.Logger.Debug(Showstring);
    }

    public override void Update(GameTime gameTime)
    {
        if (!PotionCraftState.Active)
            return;
        if (Main.mouseLeft && !IsMouseHovering)
            EndInputText();

        if (!string.IsNullOrEmpty(TempText))
            TempText = GetUnmodifiedPart(TempText, Currentvalue);
        
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

    public static string GetUnmodifiedPart(string original,ref string modified)
    {
        var minLength =Math.Min(original.Length, modified.Length);
        for (var i = 0;i< minLength; i++)
        {
            if (original[i] == modified[i])
                continue;
            
            modified = modified[i..];
            return original[..i];
        }

        modified = modified[minLength..];
        return original[..minLength];
    }

    public static string GetUnmodifiedPart(string original,string modified)
    {
        var minLength =Math.Min(original.Length, modified.Length);
        for (var i = 0;i< minLength; i++)
        {
            if (original[i] == modified[i])
                continue;

            return original[..i];
        }

        return original[..minLength];
    }
    public override void LeftClick(UIMouseEvent evt)
    {
        if (!PotionCraftState.Active)
            return;
        InputText();
    }
    
    
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if(Asset is not null)
            spriteBatch.Draw(Asset.Value,GetDimensions().ToRectangle(),new Rectangle(46,276,246,106),Color.White);

        Utils.DrawBorderString(spriteBatch, Showstring, GetDimensions().Position(), Color.White, 1f, 0f, 0f, -1);
        var vector2 = GetDimensions().Position() + MeasureString_Cursor(Showstring);

        if (!Inputting)
            return;
        
        HandleInputText();

        var show = "";
        
        if (!string.IsNullOrEmpty(TempText))
            show +=$"[c/{TempText}:"+ GetUnmodifiedPart(TempText, Currentvalue)+"]";
        
        if (!string.IsNullOrEmpty(Currentvalue))
            show += $"[c/{RGBToHex(SelectColor)}:{Currentvalue}]";
        Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
        Utils.DrawBorderString(spriteBatch, show, vector2,Color.White);
        if (Main.GameUpdateCount % 20U >= 10U)
            return;
        Utils.DrawBorderString(spriteBatch, "|", vector2 + MeasureString_Cursor(Currentvalue), Color.White, 1f, 0.0f, 0.0f, -1);
    }

}
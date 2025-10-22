using System;
using static PotionCraft.Assets;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class ShoppingElement:UIElement
{
    private UIElement Area;
    
    private UIText PriceText;

    private UIImageButton AddButton;

    private UIImageButton ReduceButton;

    private UIImageButton MaxButton;
    
    private BaseFuildInput Input;

    private int MaxCount;

    private int Price;
    
    private int Count;
    public override void OnInitialize()
    {
        Area = new();
        Area.Width.Set(300,0);
        Area.Height.Set(150,0);
        Input = new BaseFuildInput(() =>
        {
            Count = Convert.ToInt32(Input.Currentvalue);
        });
        ReduceButton = new UIImageButton(UITexture(""));
        ReduceButton.OnLeftClick += Reduce;
        AddButton = new UIImageButton(UITexture(""));
        AddButton.OnLeftClick += Add;
        MaxButton = new UIImageButton(UITexture(""));
        MaxButton.OnLeftClick += Max;
    }

    private void Add(UIMouseEvent evt, UIElement listener)
    {
        Count++;
    }

    private void Reduce(UIMouseEvent evt, UIElement listener)
    {
        Count--;
    }

    private void Max(UIMouseEvent evt, UIElement listener)
    {
        Count = CaculateMoney(Main.LocalPlayer);
    }
    
    private int CaculateMoney(Player player)
    {
        var num = Utils.CoinsCount(out _, player.bank.item);
        var num2 = Utils.CoinsCount(out _, player.bank2.item);
        var num3 = Utils.CoinsCount(out _, player.bank3.item);
        var num4 = Utils.CoinsCount(out _, player.bank4.item);
        var num5 = Utils.CoinsCombineStacks(out _, num, num2, num3, num4);
        return (int)(num5/Price >MaxCount ? MaxCount : num5/Price);
    }

    private void BuyThis(Player player)
    {
        Main.LocalPlayer.BuyItem(Price * Count);
    }
    
}

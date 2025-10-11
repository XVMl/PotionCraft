using PotionCraft.Content.System;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PotionCraft.Content.UI.CraftUI;

public class ShoppingElement:UIElement
{
    private UIElement Area;
    
    private UIText Price;

    private UIImageButton AddButton;

    private UIImageButton ReduceButton;

    private UIImageButton MaxButton;

    public override void OnInitialize()
    {
        Area = new();
        Area.Width.Set(300,0);
        Area.Height.Set(150,0);
        
    }
    
}

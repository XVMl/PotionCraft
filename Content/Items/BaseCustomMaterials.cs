using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace PotionCraft.Content.Items;

public class BaseCustomMaterials(string Datapath) : ModItem
{
    private string Datapath = Datapath;

    public override void SetStaticDefaults()
    {
        
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 32;
        
    }
    
    

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale,
        int whoAmI)
    {
        return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
    }
    
}
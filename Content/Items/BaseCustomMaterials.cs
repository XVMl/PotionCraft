using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PotionCraft.Content.Items;
[Autoload(false)]
public class BaseCustomMaterials : ModItem
{

    private string path ;

    private string name ;
    public BaseCustomMaterials(){}

    public BaseCustomMaterials(string name, string path)
    {
        this.path = path;
        this.name = name;
    }

    public override string Name => name;

    public override string Texture => Assets.Path.Items + path;

    public override void SetStaticDefaults()
    {
        
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System.AutoLoaderSystem;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PotionCraft.Content.Items;
[Autoload(false)]
public class BaseCustomMaterials : ModItem
{
    
    private MaterialData Materialdata;
    public BaseCustomMaterials(){}

    public BaseCustomMaterials(MaterialData  materialData)
    {
        Materialdata = materialData;
    }

    public override string Name => Materialdata.Name;

    public override string Texture => Assets.Path.Items + Materialdata.Name;

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
        //tooltips.Add(new TooltipLine(Mod, "", Materialdata.Tooltips));

    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale,
        int whoAmI)
    {
        return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
    }
    
}

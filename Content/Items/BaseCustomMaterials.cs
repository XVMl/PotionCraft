using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using PotionCraft.Content.System.AutoLoaderSystem;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static PotionCraft.Assets;

namespace PotionCraft.Content.Items;
[Autoload(false)]
public class BaseCustomMaterials : ModItem
{
    
    public MaterialData Materialdata;

    public BaseCustomMaterials(MaterialData  materialData)
    {
        Materialdata = materialData;
    }


    public override string Name => Materialdata.Name;

    public override string Texture => Path.Items + Materialdata.Name;

    protected override bool CloneNewInstances => true;

    public override void SetStaticDefaults()
    {
        
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 9999;
        Item.useAnimation = 45;
        Item.useTime = 45;
        Item.useStyle = ItemUseStyleID.EatFood;
    }
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);
        //tooltips.Add(new TooltipLine(Mod, "", Materialdata.Tooltips));

    }

    public override bool? UseItem(Player player)
    {
        Main.NewText(Name);
        Main.NewText(Materialdata.Name);
        return true;
    }

    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Texture2D draw = ItemsTexture(Materialdata.Name).Value;
        spriteBatch.Draw(draw, position, null, Color.White, 0, draw.Size() / 2, scale*Materialdata.ScaleSize, SpriteEffects.None, 0);
        return false;
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale,
        int whoAmI)
    {
        return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
    }
    
}

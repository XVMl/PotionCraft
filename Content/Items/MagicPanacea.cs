using PotionCraft.Content.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.Items
{
    internal class MagicPanacea:ModItem
    {
        public override string Texture => Assets.Path.Items + Name;
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Master;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 2;
            Item.UseSound = SoundID.Item3;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }
    }
}

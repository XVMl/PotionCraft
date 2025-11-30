using log4net.Core;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PotionCraft.Content.System.AutoUIState;

using static PotionCraft.Content.System.LanguageHelper;
namespace PotionCraft.Content.Items
{
    internal class TestSwitch:ModItem
    {

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = 0;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 2;
            Item.UseSound = SoundID.Item3;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            Mod instance = ModContent.GetInstance<PotionCraft>();
            var question = CreatQuestion(4);
            instance.Logger.Debug(question);
            //var question = "DarkMageBookMount StardustMinion @ @ @ + DarkMageBookMount + LeafCrystal @ + GlitteryButterfly @ + @ Campfire + @ WellFed2 @ @ + @ OnFire3 + @ BabyRedPanda @ @ + @ TerraFartMinecartRight @ + @ Flipper @ @ + WarTable + DD2BetsyPet + ";
            var name = LocationTranslate(question);
            instance.Logger.Debug(name);
            Main.NewText(name);
            //var text = WrapTextWithColors_ComPact(name, 300);
            //var height = MeasureString_Cursor(text.Item1);
            return true;
        }
    }
}

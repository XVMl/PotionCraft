using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using System.Reflection;
using Microsoft.Xna.Framework;
using PotionCraft.Content.System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

namespace PotionCraft.Content.Items
{
    public class TestPotion : ModItem
    {
        public string PotionName = "";

        public override LocalizedText DisplayName => base.DisplayName;

        public Dictionary<int, int> BuffDictionary = new();

        static readonly ConstructorInfo Internal_TooltipLine = typeof(TooltipLine).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,new Type[]{
        typeof(string),
        typeof(string)
        },null)!;
        //static readonly MethodInfo SetName = typeof(LocalizedText).GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Instance)!;
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Master;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 2;
            Item.consumable = true;
            Item.UseSound = SoundID.Item3;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["BuffID"] = BuffDictionary.Keys.ToList();
            tag["BuffTime"] = BuffDictionary.Values.ToList();
            tag["PotionName"] = PotionName;
        }

        public override void LoadData(TagCompound tag)
        {
            var BuffID = tag.Get<List<int>>("BuffID");
            var BuffTime = tag.Get<List<int>>("BuffTime");
            BuffDictionary = BuffID.Zip(BuffTime, (k, v) => new { Key = k, value = v }).ToDictionary(x => x.Key, x => x.value);
            PotionName = tag.GetString("PotionName");
            UpdataName();
        }

        public void Purifying()
        {
            foreach (var buff in BuffDictionary.Keys.ToList())
            {
                BuffDictionary[buff] *= 2;
            }
            if (BuffDictionary.Count == 1)
            {
                PotionName = "纯化" + PotionName;
            }
            else
            {
                PotionName = "纯化(" + PotionName + ")";
            }
        }

        public void UpdataName()
        {
            //try
            //{
            //    SetName.Invoke(DisplayName, [PotionName]);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
             return base.PreDrawTooltip(lines, ref x, ref y);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Internal_TooltipLine==null)
            {
                return;
            }
            
            tooltips.Add((TooltipLine)Internal_TooltipLine.Invoke(["ItemName", PotionName + "药剂"]));
            tooltips.Add(new TooltipLine(Mod, "s", Language.GetTextValue("Mods.PotionCraft.BuffColors.Calamity.2")));
        }

        public override bool? UseItem(Player player)
        {
            foreach (var item in BuffDictionary)
            {
                player.AddBuff(item.Key, item.Value);
            }
            string s = LanguageHelper.ColorfulBuffName(0);
            if (s!=null)
            {
                Main.NewText(s);
            }
            return true;
        }

    }
}

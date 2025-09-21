using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Reflection;
using PotionCraft.Content.System;
using System.Collections.ObjectModel;

namespace PotionCraft.Content.Items
{
    public class BasePotion : ModItem
    {
        public override string Texture =>Assets.Path.Items+"BasePotion";

        /// <summary>
        /// 将会显示的药剂名
        /// </summary>
        public string PotionName = "";
        /// <summary>
        /// 精炼的次数，用于改变字体颜色
        /// </summary>
        public int PurifyingCount;
        /// <summary>
        /// 组合的次数，用于改变字体颜色
        /// </summary>
        public int MashUpCount;
        /// <summary>
        /// 是否为酒类
        /// </summary>
        public bool Wine;
        /// <summary>
        /// 是否为魔法
        /// </summary>
        public bool Magic;
        
        public string Signatures = "";
        
        public Dictionary<int, PotionData> PotionDictionary=new();
        
        static readonly ConstructorInfo Internal_TooltipLine = typeof(TooltipLine).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
        [
            typeof(string),
        typeof(string)
        ],null)!;
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
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
            Item.UseSound = SoundID.Item3;
        }

        public override void SaveData(TagCompound tag)
        {
            var potiondatalist = PotionDictionary.Select(potiondata => new TagCompound()
            {
                { "BuffID", potiondata.Value.BuffId }, { "BuffTime", potiondata.Value.BuffTime },
                { "Counts", potiondata.Value.Counts }, { "ItemId", potiondata.Value.ItemId }
            }).ToList();
            tag["PotionData"] = potiondatalist;
            tag["PotionName"] = PotionName;
            tag["Signatures"] = Signatures;
            tag["PurifyingCount"] = PurifyingCount;
            tag["MashUpCount"] = MashUpCount;
        }

        public override void LoadData(TagCompound tag)
        {
            var potiondatalist = tag.GetList<TagCompound>("PotionData");
            foreach (var potion in potiondatalist)
            {
                PotionDictionary.Add(potion.Get<int>("BuffID"),
                    new PotionData(potion.GetInt("BuffID"),
                        potion.GetInt("ItemId"),
                        potion.GetInt("Counts"),
                        potion.GetInt("BuffTime")));
            }
            Signatures =  tag.GetString("Signatures");
            PotionName = tag.GetString("PotionName");
            PurifyingCount = tag.Get<int>("PurifyingCount");
            MashUpCount = tag.Get<int>("MashUpCount");
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Internal_TooltipLine==null)
            {
                return;
            }
            tooltips.Clear();
            tooltips.Add(new TooltipLine(Mod, "", PotionName));
        }

        public override bool? UseItem(Player player)
        {
            foreach (var item in PotionDictionary)
            {
                player.AddBuff(item.Key, item.Value.BuffTime);
            }
            return true;
        }

    }
}

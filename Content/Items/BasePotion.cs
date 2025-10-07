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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using static PotionCraft.Assets;
using static PotionCraft.Content.System.LanguageHelper;
using Terraria.Localization;
using Terraria.Audio;

namespace PotionCraft.Content.Items
{
    public class BasePotion : ModItem
    {
        public override string Texture => Path.Items + "BasePotion";
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
        /// <summary>
        /// 备注的文本
        /// </summary>
        public string Signatures = "";
        /// <summary>
        /// 用于记录药剂的药水数据
        /// </summary>
        public Dictionary<int, PotionData> PotionDictionary = new();
        /// <summary>
        /// 用于记录药剂的绘制列表
        /// </summary>
        public List<int> DrawPotionList = new();
        /// <summary>
        /// 用于记录药剂的绘制列表
        /// </summary>
        public List<int> DrawCountList = new();
        //TODO 添加文本的记录
        /// <summary>
        /// 用于记录展示于物品栏上方药剂的名称文本
        /// </summary>
        public string HotbarPotionName =>DeleteTextColor(PotionName);
        /// <summary>
        /// 用于记录药剂的基液名称
        /// </summary>
        public string BaseName;
        /// <summary>
        /// 用于记录药剂的使用方式
        /// </summary>
        public int PotionUseStyle = ItemUseStyleID.DrinkLiquid;
        /// <summary>
        /// 用于记录药剂的使用声音
        /// </summary>
        public int PotionUseSounds = (int)PotionUseSound.Item2;

        public static readonly MethodInfo ItemSound = typeof(SoundID).GetMethod("ItemSound", BindingFlags.NonPublic | BindingFlags.Static, [typeof(int)]);

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Pink;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
        }

        public override void SaveData(TagCompound tag)
        {
            var potiondatalist = PotionDictionary.Select(potiondata => new TagCompound()
            {
                { "BuffID", potiondata.Value.BuffId }, { "BuffTime", potiondata.Value.BuffTime },
                { "Counts", potiondata.Value.Counts }, { "ItemId", potiondata.Value.ItemId }
            }).ToList();
            tag["PotionData"] = potiondatalist;
            tag["DrawPotionList"] = DrawPotionList;
            tag["DrawCountList"] = DrawCountList;
            tag["PotionName"] = PotionName;
            tag["Signatures"] = Signatures;
            tag["PurifyingCount"] = PurifyingCount;
            tag["MashUpCount"] = MashUpCount;
            tag["PotionUseStyle"] = PotionUseStyle;
            tag["PotionUseSound"] = PotionUseSounds;
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
            DrawPotionList = tag.GetList<int>("DrawPotionList").ToList();
            DrawCountList = tag.GetList<int>("DrawCountList").ToList();
            Signatures = tag.GetString("Signatures");
            PotionName = tag.GetString("PotionName");
            PurifyingCount = tag.Get<int>("PurifyingCount");
            MashUpCount = tag.Get<int>("MashUpCount");
            PotionUseStyle = tag.Get<int>("PotionUseStyle");
            PotionUseSounds = tag.Get<int>("PotionUseSound");
            if (PotionUseSounds == 0) PotionUseSounds = 1;
            Item.UseSound = (SoundStyle)ItemSound.Invoke(null, [PotionUseSounds]);
            Item.useStyle = PotionUseStyle;
        }
        
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor,
            Vector2 origin, float scale)
        {
            if (DrawPotionList.Count == 0)
                return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            for (int i = 0; i < DrawPotionList.Count; i++)
            {
                for (int j = 0; j < DrawCountList[i]; j++)
                {
                    Texture2D draw = TextureAssets.Item[DrawPotionList[i]].Value;
                    spriteBatch.Draw(draw, position + new Vector2(i * 10, j * 10), null, Color.White, 0, draw.Size() / 2, scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (DrawPotionList.Count == 0)
                return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            for (int i = 0; i < DrawPotionList.Count; i++)
            {
                for (int j = 0; j < DrawCountList[i]; j++)
                {
                    Texture2D draw = TextureAssets.Item[DrawPotionList[i]].Value;
                    spriteBatch.Draw(draw, Item.position - Main.screenPosition + new Vector2(i * 10, j * 10), null, Color.White, 0, draw.Size() / 2, scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Clear();
        }

        public override bool? UseItem(Player player)
        {
            Item.useStyle = PotionUseStyle;
            foreach (var item in PotionDictionary)
            {
                player.AddBuff(item.Key, item.Value.BuffTime);
            }
            return true;
        }

    }
}

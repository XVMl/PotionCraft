using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using static PotionCraft.Assets;
using static PotionCraft.Content.System.LanguageHelper;
using Terraria.Localization;
using Terraria.Audio;
using Microsoft.CodeAnalysis;
using System.IO;
using System;

namespace PotionCraft.Content.Items
{
    public class BasePotion : ModItem
    {
        public override string Texture => Assets.Path.Items + "Style1";
        /// <summary>
        /// 将会显示的药剂名
        /// </summary>
        public string PotionName
        {
            get
            {
                if (CanCustomName) 
                    return $"{CustomName}";
                return LanguageManager.Instance.ActiveCulture.Name switch
                {
                    "zh-Hans" => $"{LocationTranslate(_Name, false)} {Deafult_Hex.Insert(10,BaseName)}",
                    _ => $"{LocationTranslate(_Name)} {Deafult_Hex.Insert(10, BaseName)}",
                };
            }
        }

        /// <summary>
        /// 用药水名、+和@记录的类似于后缀表达式的特殊名称，用于快速比对和本地化翻译
        /// </summary>
        [NetSend] 
        public string _Name="";
        /// <summary>
        /// 
        /// </summary>
        [NetSend]
        public string CustomName="";
        /// <summary>
        /// 是否可以自定义名称
        /// </summary>
        [NetSend]
        public bool CanCustomName;
        /// <summary>
        /// 精炼的次数，用于改变字体颜色
        /// </summary>
        [NetSend]
        public int PurifyingCount;
        /// <summary>
        /// 组合的次数，用于改变字体颜色
        /// </summary>
        [NetSend]
        public int MashUpCount;
        /// <summary>
        /// 是否为酒类
        /// </summary>
        [NetSend]
        public bool Wine;
        /// <summary>
        /// 是否为魔法
        /// </summary>
        [NetSend]
        public bool Magic;
        /// <summary>
        /// 备注的文本
        /// </summary>
        [NetSend]
        public string Signatures = "";
        /// <summary>
        /// 用于记录药剂的药水数据
        /// </summary>
        public Dictionary<string, PotionData> PotionDictionary = new();
        /// <summary>
        /// 用于记录药剂的绘制列表
        /// </summary>
        public List<int> DrawPotionList = new();
        /// <summary>
        /// 用于记录药剂的绘制列表
        /// </summary>
        public List<int> DrawCountList = new();
        /// <summary>
        /// 用于记录展示于物品栏上方药剂的名称文本
        /// </summary>
        public string HotbarPotionName =>DeleteTextColor(PotionName);
        /// <summary>
        /// 用于记录药剂的基液名称
        /// </summary>
        [NetSend]
        public string BaseName = TryGetLanguagValue("BaseName.0");
        /// <summary>
        /// 用于记录药剂的使用方式
        /// </summary>
        [NetSend]
        public int PotionUseStyle = ItemUseStyleID.DrinkLiquid;
        /// <summary>
        /// 用于记录药剂的使用声音
        /// </summary>
        [NetSend]
        public int PotionUseSounds = (int)PotionUseSound.Item2;
        /// <summary>
        /// 用于记录药剂是否包装
        /// </summary>
        [NetSend]
        public bool IsPackage = true;
        /// <summary>
        /// 药剂的图标ID
        /// </summary>
        public int IconID
        {
            get
            {
                var id = ModContent.ItemType<BasePotion>();

                if (ItemID.Search.ContainsName(IconName))
                    id = ItemID.Search.GetId(IconName);
                
                return id;
            }
            set
            {
                IconName = ItemID.Search.GetName(value);
            }
        }
        /// <summary>
        /// 用于记录药剂查找图标的名称
        /// </summary>
        [NetSend]
        public string IconName = "";
        /// <summary>
        /// 用于记录药剂是否自动使用
        /// </summary>
        [NetSend]
        public bool AutoUse;
        /// <summary>
        /// 用于记录可以编辑的玩家名称
        /// </summary>
        [NetSend]
        public string EditorName="";
        /// <summary>
        /// 用于记录是否可以编辑
        /// </summary>
        [NetSend]
        public bool CanEditor = true;
        /// <summary>
        /// 
        /// </summary>
        //public Rectangle Frame = Rectangle.Empty;

        [NetSend]
        public int useAnimation=45;

        [NetSend]
        public int useTime=45;


        public static readonly MethodInfo ItemSound = typeof(SoundID).GetMethod("ItemSound", BindingFlags.NonPublic | BindingFlags.Static, [typeof(int)]);

        public override void SetStaticDefaults()
        {
         
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.White;
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
                { "BuffName", potiondata.Value.BuffName },
                { "ItemID", potiondata.Value.ItemId },
                { "Counts", potiondata.Value.Counts },
                { "BuffTime", potiondata.Value.BuffTime },
                { "BuffID", potiondata.Value.BuffId }, 
            }).ToList();
            tag["PotionData"] = potiondatalist;
            tag["DrawPotionList"] = DrawPotionList;
            tag["DrawCountList"] = DrawCountList;
            tag["CustomName"] = CustomName;
            tag["Signatures"] = Signatures;
            tag["PurifyingCount"] = PurifyingCount;
            tag["MashUpCount"] = MashUpCount;
            tag["PotionUseStyle"] = PotionUseStyle;
            tag["PotionUseSound"] = PotionUseSounds;
            tag["IsPackage"] = IsPackage;
            tag["IconName"] = IconName;
            tag["CanCustomName"] = CanCustomName;
            tag["_Name"] = _Name;
            tag["CanEditor"] = CanEditor;
            tag["useTime"] = useTime;
            tag["useAnimation"] = useAnimation;
            //tag["Farme"] = RectangleTostirng(Frame);
        }

        public static Rectangle StringToRectangle(string s)
        {
            var vectorstring = s.Split(',');
            int[] ints = [0, 0, 0, 0];
            for (int i = 0;i<vectorstring.Length; i++)
            {
                ints[i] = int.Parse(vectorstring[i]);
            }
            return new Rectangle(ints[0], ints[1], ints[2], ints[3]);
        }

        public static string RectangleTostirng(Rectangle v4) => $"{v4.X},{v4.Y},{v4.Width},{v4.Height}";

        public override void LoadData(TagCompound tag)
        {
            var potiondatalist = tag.GetList<TagCompound>("PotionData");
            foreach (var potion in potiondatalist)
            {
                PotionDictionary.Add(potion.Get<string>("BuffName"),
                    new PotionData(
                        potion.GetString("BuffName"),
                        potion.GetInt("ItemID"),
                        potion.GetInt("Counts"),
                        potion.GetInt("BuffTime"),
                        potion.GetInt("BuffID"))
                    );
            }
            DrawPotionList = tag.GetList<int>("DrawPotionList").ToList();
            DrawCountList = tag.GetList<int>("DrawCountList").ToList();
            Signatures = tag.GetString("Signatures");
            CustomName = tag.GetString("CustomName");
            PurifyingCount = tag.Get<int>("PurifyingCount");
            MashUpCount = tag.Get<int>("MashUpCount");
            PotionUseStyle = tag.Get<int>("PotionUseStyle");
            PotionUseSounds = tag.Get<int>("PotionUseSound");
            IsPackage = tag.Get<bool>("IsPackage");
            CanCustomName = tag.Get<bool>("CanCustomName");
            IconName = tag.Get<string>("IconName");
            _Name = tag.GetString("_Name");
            CanEditor= tag.GetBool("CanEditor");

            useAnimation = tag.GetInt("useAnimation");
            useTime = tag.GetInt("useTime");
            Item.useAnimation = useAnimation;
            Item.useTime = useTime;
            //Frame = StringToRectangle(tag.GetString("Frame"));
            if (IconID == -1) IconID = Item.type;
            if (PotionUseSounds == 0) PotionUseSounds = 1;
            Item.UseSound = (SoundStyle)ItemSound.Invoke(null, [PotionUseSounds]);
            Item.useStyle = PotionUseStyle;
        }
        
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor,
            Vector2 origin, float scale)
        {
            if (IsPackage)
            {
                Main.instance.LoadItem(IconID);
                var icon = TextureAssets.Item[IconID].Value;
                var iframe = ((Main.itemAnimations[Item.type] == null) ? icon.Frame() : Main.itemAnimations[Item.type].GetFrame(icon));

                //if (iframe.Equals(Rectangle.Empty))
                //{
                //    Texture2D value = TextureAssets.Item[IconID].Value;
                //    Frame = ((Main.itemAnimations[IconID] == null) ? value.Frame() : Main.itemAnimations[IconID].GetFrame(value));
                //}
                var _origin = iframe.Size() / 2;
                spriteBatch.Draw(icon, position, iframe, Color.White, 0, _origin, scale*1.1f, SpriteEffects.None, 0);
                return false;
            }
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
            if (IsPackage)
            {
                Main.instance.LoadItem(IconID);
                var icon = TextureAssets.Item[IconID].Value;
                var iframe = ((Main.itemAnimations[Item.type] == null) ? icon.Frame() : Main.itemAnimations[Item.type].GetFrame(icon));

                //if (Frame.Equals(Rectangle.Empty))
                //{
                //    Texture2D value = TextureAssets.Item[IconID].Value;
                //    Frame = ((Main.itemAnimations[IconID] == null) ? value.Frame() : Main.itemAnimations[IconID].GetFrame(value));
                //}
                var _origin = iframe.Size() / 2;
                spriteBatch.Draw(icon, Item.position - Main.screenPosition, iframe, Color.White, 0, _origin, scale*1.1f, SpriteEffects.None, 0);
                return false;
            }
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

        public override void NetSend(BinaryWriter writer)
        {
            foreach (var field in GetType().GetFields())
            {
                if (field.GetCustomAttribute<NetSend>() is null)
                    continue;
                var type = field.FieldType;
                if(type == typeof(int))
                {
                    writer.Write((int)field.GetValue(this));
                }
                else if (type == typeof(string))
                {
                    var value = (string)field.GetValue(this);
                    value ??= "";
                    writer.Write(value);
                }
                else if (type == typeof(bool))
                { 
                    writer.Write((bool)field.GetValue(this));
                }
                //Mod instance = ModContent.GetInstance<PotionCraft>();
                //instance.Logger.Debug(field.Name + " " + field.GetValue(this));
            }


            writer.Write(PotionDictionary.Count);
            foreach (var item in PotionDictionary)
            {
                writer.Write(item.Value.BuffName);
                writer.Write(item.Value.ItemId);
                writer.Write(item.Value.Counts);
                writer.Write(item.Value.BuffTime);
                writer.Write(item.Value.BuffId);
            }

            writer.Write(DrawPotionList.Count);
            foreach (int item in DrawPotionList)
            {
                writer.Write(item);
            }
            writer.Write(DrawCountList.Count);
            foreach (int item in DrawCountList)
            {
                writer.Write(item);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            foreach (var field in GetType().GetFields())
            {
                if (field.GetCustomAttribute<NetSend>() is null)
                    continue;


                //Mod instance = ModContent.GetInstance<PotionCraft>();
                //instance.Logger.Debug(field.Name);
                var type = field.FieldType;
                if (type == typeof(int))
                {
                    field.SetValue(this, reader.ReadInt32());
                }
                else if (type == typeof(string))
                {
                    field.SetValue(this, reader.ReadString());
                }
                else if (type == typeof(bool))
                {
                    field.SetValue(this, reader.ReadBoolean());
                }

            }

            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var BuffName = reader.ReadString();
                var ItemID = reader.ReadInt32();
                var Count = reader.ReadInt32();
                var BuffTime = reader.ReadInt32();
                var BuffId = reader.ReadInt32();
                PotionDictionary.Add(BuffName,
                    new PotionData(BuffName,
                    ItemID,
                    Count,
                    BuffTime,
                    BuffId
                ));
            }

            var potionlistcount = reader.ReadInt32();
            for (int i = 0; i < potionlistcount; i++)
            {
                DrawPotionList.Add(reader.ReadInt32());
            }
            var countlistconut = reader.ReadInt32();
            for (int i = 0; i < countlistconut; i++)
            {
                DrawCountList.Add(reader.ReadInt32());
            }

            Item.useAnimation = useAnimation;
            Item.useTime = useTime;
            Item.useStyle = PotionUseStyle;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Clear();
        }

        public override bool? UseItem(Player player)
        {
            foreach (var item in PotionDictionary)
            {
                player.AddBuff(item.Value.BuffId, item.Value.BuffTime);
            }
            return true;
        }

    }

    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false)]
    public class NetSend : Attribute
    {
        public bool Synchronize;

        public NetSend() 
        {
            Synchronize = true;
        }
        public NetSend(bool synchronize)
        {
            Synchronize = synchronize;
        }
    }
}

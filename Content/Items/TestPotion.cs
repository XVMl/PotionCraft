using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PotionCraft.Content.Items
{
    public class TestPotion:ModItem
    {
        public static string PotionName;

        public static Dictionary<int, int> BuffDictionary=new();

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
            Item.consumable = true;
            Item.UseSound = SoundID.Item3;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["BuffID"] = BuffDictionary.Keys.ToList();
            tag["BuffTime"] =BuffDictionary.Values.ToList();
        }

        public override void LoadData(TagCompound tag)
        {
            var BuffID = tag.Get<List<int>>("BuffID");
            var BuffTime = tag.Get<List<int>>("BuffTime");
            BuffDictionary = BuffID.Zip(BuffTime, (k, v) => new { Key = k, value = v }).ToDictionary(x => x.Key, x => x.value);
        }

        public void Purifying(Action action)
        {
            
        }

        public override bool? UseItem(Player player)
        {
            
            return true;
        }

    }
}

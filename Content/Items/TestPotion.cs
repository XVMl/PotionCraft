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

namespace PotionCraft.Content.Items
{
    public class TestPotion:ModItem
    {
        public string PotionName="";

        public Dictionary<int, int> BuffDictionary=new();

        static readonly MethodInfo SetName = typeof(LocalizedText).GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Instance)!;
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
            if (BuffDictionary.Count==1)
            {
                PotionName = "纯化" + PotionName;
            }
            else
            {
                PotionName = "纯化(" + PotionName+")";
            }
        }

        public void MashUp()
        { 
            
        }

        public void UpdataName()
        {
            try
            {
                SetName.Invoke(DisplayName,[PotionName]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Potion", PotionName));
        }

        public override bool? UseItem(Player player)
        {
            Main.NewText(DisplayName.Key + " " + DisplayName.Value);
            return true;
        }

    }
}

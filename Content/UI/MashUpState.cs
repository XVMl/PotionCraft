using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.UI
{
    public class MashUpState : AutoUIState
    {
        public override bool IsLoaded() => PotionCraftState.CraftState == CraftUIState.MashUp && PotionCraftState.ActiveState;

        public override string Layers_FindIndex => "Vanilla: Interface Logic 3";

        private PotionSlot<MashUpState> PotionSlot;

        private MaterialSlot<MashUpState> MaterialSlot;

        protected CreatedPotionSlot<MashUpState> CreatedPotionSlot;

        private MashUpButton mashupbutton;

        public override void OnInitialize()
        {
            PotionSlot = new(this)
            {
                HAlign = 0.4f,
                VAlign = 0.5f,
            };
            Append(PotionSlot);

            CreatedPotionSlot = new(this)
            { 
                HAlign = 0.5f,
                VAlign = 0.45f,
            };
            Append(CreatedPotionSlot);

            MaterialSlot = new(this)
            {
                HAlign = 0.6f,
                VAlign = 0.5f,
            };
            Append(MaterialSlot);

            mashupbutton = new(this)
            {  
                VAlign = 0.65f,
                HAlign = 0.5f,
            };
            Append(mashupbutton);

        }

    }

    public class MashUpButton:PotionElement
    {
        private MashUpState MashUpState { get; set; }
        public MashUpButton(MashUpState mashUpState) 
        {
            MashUpState = mashUpState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        public void MashUp(Item Potion, TestPotion Material)
        {
            TestPotion CreatedPotion = AsPotion(Potion.Clone());
            foreach (var buff in Material.BuffDictionary.Keys.ToList())
            {
                if (CreatedPotion.BuffDictionary.TryGetValue(buff, out int value))
                {
                    CreatedPotion.BuffDictionary[buff] += value;
                }
                else
                {
                    CreatedPotion.BuffDictionary.Add(buff, value);
                }
            }
            CreatedPotion.PotionName += " 和 " + Material.DisplayName.Value;
        }

        public void MashUp(Item Potion, Item Material)
        {
            TestPotion CreatedPotion = AsPotion(Potion.Clone());
            if (CreatedPotion.BuffDictionary.ContainsKey(Material.buffType))
            {
                CreatedPotion.BuffDictionary[Material.buffType] += Material.buffTime;
            }
            else
            {
                CreatedPotion.BuffDictionary.TryAdd(Material.buffType, Material.buffTime);
            }

            CreatedPotion.PotionName += " 和 " + Lang.GetBuffName(Material.buffType);
            CreatedPotion.UpdataName();
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            Item item = new();
            item.SetDefaults(ModContent.ItemType<TestPotion>());
            AsPotion(item).PotionName += "Test";
            MashUpState.Material = item.Clone();

            if (MashUpState.Potion.IsAir || MashUpState.Material.IsAir) return;
            TestPotion tes = AsPotion(MashUpState.Potion);
            if (IsMaterial(MashUpState.Material))
            {
                tes.MashUp(MashUpState.Material);
            }
            else
            {
                tes.MashUp(AsPotion(MashUpState.Material));
            }

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }
}


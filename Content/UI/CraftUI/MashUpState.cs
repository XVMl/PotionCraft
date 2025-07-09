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
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static PotionCraft.Content.System.LanguageHelper;
namespace PotionCraft.Content.UI.CraftUI
{
    public class MashUpState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.MashUp;

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<MashUpState> PotionSlot;

        private MaterialSlot<MashUpState> MaterialSlot;

        private CreatedPotionSlot<MashUpState> CreatedPotionSlot;

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

    public class MashUpButton : PotionElement<MashUpState>
    {
        //private MashUpState MashUpState { get; set; }
        public MashUpButton(MashUpState mashUpState)
        {
            PotionCraftState = mashUpState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void MashUp(Item Potion, TestPotion Material)
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
            if (CreatedPotion.BuffDictionary.Count == 0)
            {
                CreatedPotion.PotionName += Material.DisplayName.Value;
            }
            else
            {
                CreatedPotion.PotionName += " " + ColorfulFont("MashUp" + Math.Min(14, CreatedPotion.PurifyingCount).ToString()) + " " + Material.DisplayName.Value;
            }
        }

        private void MashUp(Item Potion, Item Material)
        {
            PotionCraftState.CreatedPotion = Potion.Clone();
            TestPotion CreatedPotion = AsPotion(PotionCraftState.CreatedPotion);
            if (CreatedPotion.BuffDictionary.ContainsKey(Material.buffType))
            {
                CreatedPotion.BuffDictionary[Material.buffType] += Material.buffTime;
            }
            else
            {
                CreatedPotion.BuffDictionary.TryAdd(Material.buffType, Material.buffTime);
            }
            if (CreatedPotion.BuffDictionary.Count == 1)
            {
                CreatedPotion.PotionName += ColorfulBuffName(Material.buffType);
            }
            else
            {
                CreatedPotion.MashUpCount++;
                CreatedPotion.PotionName += " " + ColorfulFont("MashUp.And." + Math.Min(14, CreatedPotion.MashUpCount).ToString()) + " " + ColorfulBuffName(Material.buffType);
            }
        }

        public override void CraftClick(UIMouseEvent evt)
        {
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            if (IsMaterial(PotionCraftState.Material))
            {
                MashUp(PotionCraftState.Potion, PotionCraftState.Material);
            }
            else
            {
                MashUp(PotionCraftState.Potion, AsPotion(PotionCraftState.Material));
            }
            //Item item = new();
            //item.SetDefaults(ModContent.ItemType<TestPotion>());
            //AsPotion(item).PotionName += "Test";
            //MashUpState.Material = item.Clone();
            base.CraftClick(evt);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }
}


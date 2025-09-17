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
        public MashUpButton(MashUpState mashUpState)
        {
            PotionCraftState = mashUpState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void MashUp(Item potion, BasePotion material)
        {
            BasePotion createdPotion = AsPotion(potion.Clone());
            foreach (var buff in material.PotionDictionary)
            {
                createdPotion.PotionDictionary.TryAdd(buff.Key, new PotionData(
                    buff.Value.BuffId,
                    buff.Value.ItemId,
                    1,
                    buff.Value.BuffTime
                ));
                createdPotion.PotionDictionary[buff.Key].BuffTime+= buff.Value.BuffTime;

            }
            createdPotion.MashUpCount+=material.MashUpCount;
            createdPotion.PotionName += createdPotion.PotionDictionary.Count == 0
                ? material.DisplayName.Value
                : $" {TryGetMashUpText(Math.Min(14, createdPotion.MashUpCount))} {material.DisplayName.Value}";
        }

        
        private void MashUp(Item potion, Item material)
        {
            PotionCraftState.CreatedPotion = potion.Clone();
            BasePotion createdPotion = AsPotion(PotionCraftState.CreatedPotion);
            createdPotion.PotionDictionary.TryAdd(material.buffType, new PotionData(
                material.buffType,
                 material.type,
                 1,
                 material.buffType
            ));
            createdPotion.PotionDictionary[material.buffType].BuffTime+= material.buffTime;

            createdPotion.PotionName += createdPotion.PotionDictionary.Count == 1
                ? TryGetPotionText(material.buffType)
                : $" {TryGetMashUpText(Math.Min(14, createdPotion.MashUpCount))} {TryGetPotionText(material.buffType)}";
            createdPotion.MashUpCount++;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            if (IsMaterial(PotionCraftState.Material))
            {
                MashUp(PotionCraftState.Potion, PotionCraftState.Material);
            }
            else
            {
                MashUp(PotionCraftState.Potion, AsPotion(PotionCraftState.Material));
            }

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }
}


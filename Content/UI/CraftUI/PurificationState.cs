using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using static PotionCraft.Content.System.LanguageHelper;
using Microsoft.Xna.Framework;

namespace PotionCraft.Content.UI.CraftUI
{
    public class PurificationState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.Purification;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<PurificationState> potionslot;

        private MaterialSlot<PurificationState> material;

        private CreatedPotionSlot<PurificationState> CreatedPotionSlot;

        private PurifyingButton purifyingbutton;

        public override void OnInitialize()
        {
            potionslot = new(this)
            {
                HAlign = 0.45f,
                VAlign = 0.5f,
            };
            Append(potionslot);

            material = new(this)
            {
                HAlign = 0.55f,
                VAlign = 0.5f,
            };
            Append(material);

            purifyingbutton = new(this)
            {
                VAlign = 0.65f,
                HAlign = 0.5f,
            };
            Append(purifyingbutton);

            CreatedPotionSlot = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.45f,
            };
            Append(CreatedPotionSlot);

        }

    }

    public class PurifyingButton : PotionElement<PurificationState>
    {

        public PurifyingButton(PurificationState purificatingState)
        {
            PotionCraftState = purificatingState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void Purifying(Item potion, Item material)
        {
            if (!AsPotion(potion).PotionName.Equals(AsPotion(material).PotionName) &&
                material.type != ModContent.ItemType<MagicPanacea>())
            {
                return;
            }
            PotionCraftState.CreatedPotion = potion.Clone();
            BasePotion createdPotion = AsPotion(PotionCraftState.CreatedPotion);
            foreach (var buff in createdPotion.BuffDictionary.Keys.ToList())
            {
                createdPotion.BuffDictionary[buff] *= 2;
            }
            createdPotion.PurifyingCount++;
            if (createdPotion.BuffDictionary.Count == 1)
            {
                createdPotion.PotionName = TryGetPurifyText(Math.Min(12, createdPotion.PurifyingCount)) + " " + createdPotion.PotionName;
            }
            else
            {
                createdPotion.PotionName = TryGetPurifyText(Math.Min(12, createdPotion.PurifyingCount))+GetBracketText(Math.Min(12, createdPotion.PurifyingCount)) + createdPotion.PotionName+ GetBracketText(Math.Min(12, createdPotion.PurifyingCount),right:true);
            }
        }

        public override void CraftClick(UIMouseEvent evt)
        {
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            Purifying(PotionCraftState.Potion, PotionCraftState.Material);
            base.CraftClick(evt);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

}

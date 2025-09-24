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
            BasePotion createdPotion;
            if (IsMaterial(potion) && Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().CanNOBasePotion)
            {
                Item item = new();
                item.SetDefaults(ModContent.GetInstance<BasePotion>().Type);
                PotionCraftState.CreatedPotion = item.Clone();
                createdPotion = AsPotion(PotionCraftState.CreatedPotion);
                createdPotion.PotionDictionary.TryAdd(potion.buffType, new PotionData(
                    potion.buffType,
                    potion.type,
                    0,
                    potion.buffTime
                ));
            }
            else
            {
                PotionCraftState.CreatedPotion = potion.Clone();
                createdPotion = AsPotion(PotionCraftState.CreatedPotion);
                createdPotion.PotionDictionary = createdPotion.PotionDictionary.ToDictionary(k => k.Key, v => v.Value);
            }
            createdPotion.PotionDictionary = createdPotion.PotionDictionary.ToDictionary(k => k.Key, v => v.Value);
            foreach (var buff in createdPotion.PotionDictionary)
            {
                createdPotion.PotionDictionary[buff.Key].BuffTime *= 2;
                createdPotion.PotionDictionary[buff.Key].Counts *= 2;
            }
            createdPotion.PotionName = $"{TryGetPurifyText(Math.Min(12, createdPotion.PurifyingCount))} {GetBracketText(Math.Min(12, createdPotion.PurifyingCount))}{createdPotion.PotionName}{GetBracketText(Math.Min(12, createdPotion.PurifyingCount), right: true)}";
            createdPotion.PurifyingCount++;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            Purifying(PotionCraftState.Potion, PotionCraftState.Material);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

}

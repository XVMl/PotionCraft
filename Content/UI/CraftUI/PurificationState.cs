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
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.Purificating;
        public override string Layers_FindIndex => "Vanilla: Mouse Text";

        public PotionSlot<PurificationState> potionslot;

        public MaterialSlot<PurificationState> material;

        protected CreatedPotionSlot<PurificationState> CreatedPotionSlot;

        public PurifyingButton purifyingbutton;

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

        private void Purifying(Item Potion, Item Material)
        {
            if (!AsPotion(Potion).PotionName.Equals(AsPotion(Material)) && Material.type != ModContent.ItemType<MagicPanacea>())
            {
                return;
            }
            PotionCraftState.CreatedPotion = Potion.Clone();
            TestPotion CreatedPotion = AsPotion(PotionCraftState.CreatedPotion);
            foreach (var buff in CreatedPotion.BuffDictionary.Keys.ToList())
            {
                CreatedPotion.BuffDictionary[buff] *= 2;
            }
            CreatedPotion.PurifyingCount++;
            if (CreatedPotion.BuffDictionary.Count == 1)
            {
                CreatedPotion.PotionName = ColorfulFont("Purifying." + Math.Min(12, CreatedPotion.PurifyingCount).ToString()) + " " + CreatedPotion.PotionName;
            }
            else
            {
                CreatedPotion.PotionName = ColorfulFont("Purifying." + Math.Min(12, CreatedPotion.PurifyingCount).ToString()) + "(" + CreatedPotion.PotionName + ")";
            }
        }

        public override void CraftClick(UIMouseEvent evt)
        {
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            Purifying(PotionCraftState.Potion, PotionCraftState.Material);
            base.CraftClick(evt);
        }

        //public override void LeftClick(UIMouseEvent evt)
        //{
        //    if (PotionCraftState.Material.IsAir) return;
        //    TestPotion tes = AsPotion(PotionCraftState.Potion);
        //    tes.BuffDictionary.TryAdd(34, 300);
        //    tes.Purifying();
        //}

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

}

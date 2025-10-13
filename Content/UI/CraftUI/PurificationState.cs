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
using static PotionCraft.Assets;
using PotionCraft.Content.UI.PotionTooltip;
namespace PotionCraft.Content.UI.CraftUI
{
    public class PurificationState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.Purification;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<PurificationState> PotionSlot;

        private MaterialSlot<PurificationState> MaterialSlot;

        private CreatedPotionSlot<PurificationState> CreatedPotionSlot;

        private PurifyingButton purifyingbutton;

        private UIElement Area;

        public override void OnInitialize()
        {
            Width.Set(920, 0);
            Height.Set(640, 0);
            HAlign = 0.5f;
            VAlign = 0.5f;
            Area = new UIElement()
            {
                HAlign = 0.2f,
                VAlign = 0.3f,
            };
            Area.Width.Set(570f, 0);
            Area.Height.Set(570f, 0);
            Append(Area);

            PotionSlot = new(this)
            {
                HAlign = 0.1f,
                VAlign = 0.5f,
                TexturePath = "ItemSlot"
            };
            Area.Append(PotionSlot);

            CreatedPotionSlot = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.45f,
            };
            Area.Append(CreatedPotionSlot);
            
            MaterialSlot = new(this)
            {
                HAlign = 1f,
                VAlign = 0.5f,
            };
            Area.Append(MaterialSlot);

            purifyingbutton = new(this)
            {
                VAlign = 0.5f,
                HAlign = 0.0f,
            };
            Append(purifyingbutton);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UITexture("PotionCraftBG").Value, Area.GetDimensions().ToRectangle(), Color.White);
            base.Draw(spriteBatch);
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

        private void Purifying(Item potion, Item MaterialSlot)
        {
            if (!TooltipUI.CheckPotion(AsPotion(potion), AsPotion(MaterialSlot)) && MaterialSlot.type != ModContent.ItemType<MagicPanacea>()) return;
            BasePotion createdPotion = CloneOrCreatPotion(PotionCraftState, potion);

            foreach (var buff in createdPotion.PotionDictionary)
            {
                buff.Value.BuffTime *= 2;
                buff.Value.Counts *= 2;
            }
            for (int i = 0; i < createdPotion.DrawPotionList.Count; i++)
            {
                createdPotion.DrawCountList[i] *= 2;
            }
            createdPotion.PurifyingCount++;
            createdPotion._Name += "@ ";
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            Purifying(PotionCraftState.Potion, PotionCraftState.Material);
            PotionCraftState.Potion.stack--;
            PotionCraftState.Material.stack--;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

}

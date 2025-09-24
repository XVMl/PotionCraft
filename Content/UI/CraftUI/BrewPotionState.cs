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
using Microsoft.Xna.Framework;

namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BrewPotionState> PotionSlot;
        
        public int PotionCount = 1;

        public override void OnInitialize()
        {
            PotionSlot = new(this)
            {
                HAlign = 0.45f,
                VAlign = 0.5f,
            };
            Append(PotionSlot);
            
        }

        public static BasePotion GetPotionInstance(Item potion)
        {
            BasePotion basePotion = ModContent.GetInstance<BasePotion>();
            BasePotion oldpotion =AsPotion(potion);
            basePotion.PotionName = oldpotion.PotionName;
            basePotion.PotionDictionary = oldpotion.PotionDictionary;
            basePotion.MashUpCount = oldpotion.MashUpCount;
            basePotion.PurifyingCount = oldpotion.PurifyingCount;
            basePotion.Wine = oldpotion.Wine;
            basePotion.Signatures = oldpotion.Signatures;
            basePotion.Magic = oldpotion.Magic;
            return basePotion;
        }

    }

    public class BrewPotionButton : PotionElement<BrewPotionState>
    {

        public BrewPotionButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void BrewPotion(Item potion)
        {
            if (!IsPotion(potion)) return;
            PotionCraftState.Potion.stack = PotionCraftState.PotionCount;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }
}

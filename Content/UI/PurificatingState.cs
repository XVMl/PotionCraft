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

namespace PotionCraft.Content.UI
{
    public class PurificatingState : AutoUIState
    {
        public override bool IsLoaded() => PotionCraftState.CraftState == PotionCraftState.CraftUIState.Purificating && PotionCraftState.ActiveState;
       
        public override string Layers_FindIndex => "Vanilla: Interface Logic 2";

        public PotionSlot<PurificatingState> potionslot;

        public PurifyingButton purifyingbutton;

        public override void OnInitialize()
        {
            potionslot = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };
            Append(potionslot);
            purifyingbutton = new(this)
            {
                VAlign = 0.75f,
                HAlign = 0.5f,
            };
            Append(purifyingbutton);
        }
    }

    public class PurifyingButton : PotionElement
    {

        public PurificatingState purificatingState;

        public PurifyingButton(PurificatingState purificatingState)
        {
            this.purificatingState = purificatingState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (purificatingState.Potion.IsAir) return;
            TestPotion tes = AsPotion(purificatingState.Potion);
            tes.BuffDictionary.TryAdd(34, 300);
            tes.Purifying();
            tes.UpdataName();
            foreach (var item in tes.BuffDictionary)
            {
                Main.NewText(item.Key);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

}

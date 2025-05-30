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
        public override bool IsLoaded()
        {
            return true;
        }
        public override string Layers_FindIndex => "Vanilla: Mouse Text";

        public PotionSlot<PurificatingState> potionslot;

        public MaterialSlot<PurificatingState> material;

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
            if (purificatingState.Material.IsAir) return;
            TestPotion tes = AsPotion(purificatingState.Potion);
            tes.BuffDictionary.TryAdd(34, 300);
            tes.Purifying();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

}

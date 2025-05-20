using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PotionCraft.Content.UI
{
    public class PotionCraftState : AutoUIState
    {
        public override string Layers_FindIndex => "Vanilla: Interface Logic 2";

        private UIGrid iGrid; 
        public override void OnInitialize()
        {
            iGrid = new UIGrid();
            iGrid.Width.Set(200, 0);
            iGrid.HAlign = 0.5f;
            iGrid.VAlign = 0.5f;
            Append(iGrid);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.ItemSlotBackgroud, iGrid.GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class ItemSlot:UIElement
    {
        public ItemSlot()
        {
            Width.Set(120f, 0f);
            Height.Set(120f, 0f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.ItemSlotBackgroud, GetDimensions().ToRectangle(), Color.White);
        }
    }
}

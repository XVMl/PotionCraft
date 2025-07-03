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
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.UI
{
    /// <summary>
    /// 此类用于修改药水的提示栏
    /// </summary>
    public class TooltipUI : AutoUIState
    {
        public override bool IsLoaded() =>Show;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private bool Show;
        private UIElement Area;
        public override void OnInitialize()
        {
            Area = new();
            Area.Width.Set(300f, 0f);
            Area.Height.Set(300f, 0f);
            Append(Area);
        }

        public override void Update(GameTime gameTime)
        {
            Show = Main.HoverItem.type == ModContent.ItemType<TestPotion>();
            Vector2 pos = Main.MouseScreen;
            Area.Left.Set(pos.X, 0);
            Area.Top.Set(pos.Y, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Tooltip, Area.GetDimensions().ToRectangle(), Color.White);
        }
    }
}

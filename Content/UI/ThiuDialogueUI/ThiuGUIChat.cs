using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.NPCs;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.UI.ThouDialogueUI
{
    public class ThiuGUIChat : AutoUIState
    {
        private bool Active;
        public override bool IsLoaded() => Active;
        public override string Layers_FindIndex => "Vanilla: Mouse Text";

        private UIElement Area;
        public override void OnInitialize()
        {
            Area = new UIElement() {
                HAlign = 0.5f,
                VAlign = 0.8f,
            };
            Area.Width.Set(400f, 0);
            Area.Height.Set(300f, 0);
            Append(Area);
        }

        public override void Update(GameTime gameTime)
        {
            //Active = Main.LocalPlayer.talkNPC == ModContent.NPCType<Thou>();
            //Main.NewText(ModContent.NPCType<Thou>());
            if (Main.LocalPlayer.talkNPC > 0)
            {
                Active = Main.npc[Main.LocalPlayer.talkNPC].type == ModContent.NPCType<Thou>();
            }
            else
            {
                Active= false;
            }
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Tooltip, Area.GetDimensions().ToRectangle(), Color.White);
        }
    }
}

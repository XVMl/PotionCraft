using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PotionCraft.Content.UI.ThouDialogueUI
{
    public class ThiuGUIChat : AutoUIState
    {
        private bool Active;
        public override bool IsLoaded() => Active;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private UIElement Area;

        public UIImageButton PurifyingButton;

        private UIImageButton MashUpButton;


        public override void OnInitialize()
        {
            Area = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 0.9f,
            };
            Area.Width.Set(600f, 0);
            Area.Height.Set(300f, 0);
            Append(Area);
            //Area.Append(MashUpButton);
        }

        //public override void Update(GameTime gameTime)
        //{
        //    if (Main.LocalPlayer.talkNPC > 0)
        //    {
        //        Active = Main.npc[Main.LocalPlayer.talkNPC].type == ModContent.NPCType<Thiu>();
        //    }
        //    else
        //    {
        //        Active = false;
        //        CraftState = CraftUiState.Default;
        //    }
        //}

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Tooltip, Area.GetDimensions().ToRectangle(), Color.White);
         }

        private void PurifyingButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            CraftState = CraftUiState.Purification;
        }

        private void MashUpButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            CraftState = CraftUiState.MashUp;
        }

    }

}

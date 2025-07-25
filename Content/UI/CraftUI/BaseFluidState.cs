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
using PotionCraft.Content.System;
using Microsoft.Xna.Framework.Input;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
namespace PotionCraft.Content.UI.CraftUI
{
    public class BaseFluidState: AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.BaseFluid;

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BaseFluidState> Potionslot;

        private MaterialSlot<BaseFluidState> Materialslot;

        private CreatedPotionSlot<BaseFluidState> CreatedPotionSlot;

        private BaseFuildInput baseFuildInput;
        public override void OnInitialize()
        {
            Potionslot = new(this)
            {
                HAlign = 0.45f,
                VAlign = 0.5f,
            };
            Append(Potionslot);

            Materialslot = new(this)
            {
                HAlign = 0.55f,
                VAlign = 0.5f,
            };
            Append(Materialslot);

            CreatedPotionSlot = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.3f,
            };
            Append(CreatedPotionSlot);

            baseFuildInput = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.65f,
            };
            Append(baseFuildInput);

        }

        public override void Update(GameTime gameTime)
        {
            baseFuildInput.Update(gameTime);
        }

    }

    public class BaseFuildInput:PotionElement<BaseFluidState>
    {
        private string currentvalue="Test";

        private bool inputting;
        public BaseFuildInput(BaseFluidState baseFluidState)
        {   Width.Set(180f, 0f);
            Height.Set(50f, 0f);
        }

        private void InputText()
        {
            inputting = true;
            Main.blockInput= true;
        }

        private void HandleInputText()
        {
            if (Main.keyState.IsKeyDown(Keys.Escape))
            {
                EndInputText();
            }

            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            string value = Main.GetInputText(currentvalue);
            if (value !=currentvalue)
            {
                currentvalue = value;
            }
        }

        private void EndInputText()
        {
            Main.blockInput = false;
            inputting = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.mouseLeft && !IsMouseHovering)
            {
                EndInputText();
            }
            if (inputting)
            {
                HandleInputText();
            }
        }

        public override void CraftClick(UIMouseEvent evt)
        {
            InputText();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (inputting)
            {
                spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
                HandleInputText();
                Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
            }
            Utils.DrawBorderString(spriteBatch, currentvalue, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
        }

    }
    

}

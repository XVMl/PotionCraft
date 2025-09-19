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
using PotionCraft.Content.Items;
namespace PotionCraft.Content.UI.CraftUI
{
    public class BaseFluidState: AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.BaseFluid;

        public string currentvalue = "Test";

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BaseFluidState> Potionslot;

        private MaterialSlot<BaseFluidState> Materialslot;

        private CreatedPotionSlot<BaseFluidState> CreatedPotionSlot;

        private BaseFuildInput baseFuildInput;

        private BaseFuildButtun baseFuildButtun;

        private BaseFuildRenaemButtun baseFuildRenaemButtun;

        public bool inputting;
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

            baseFuildButtun = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.75f,
            };
            Append(baseFuildButtun);

            baseFuildRenaemButtun = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.85f,
            };
            Append(baseFuildRenaemButtun);

        }

        public override void Update(GameTime gameTime)
        {
            baseFuildInput.Update(gameTime);
        }

    }

    public class BaseFuildRenaemButtun : PotionElement<BaseFluidState>
    {
        public BaseFluidState baseFluidState;
        public BaseFuildRenaemButtun(BaseFluidState baseFluidState)
        {
            this.baseFluidState = baseFluidState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void RenameFluid()
        {
            baseFluidState.inputting = false;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            
            base.LeftClick(evt);
            RenameFluid();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class BaseFuildButtun: PotionElement<BaseFluidState>
    {

        private BaseFluidState baseFluidState;
        public BaseFuildButtun(BaseFluidState baseFluidState)
        {
            this.baseFluidState = baseFluidState;
            PotionCraftState = baseFluidState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void ChangBaseFluid(Item potion)
        {
            PotionCraftState.Potion = potion.Clone();
            BasePotion createdPotion = AsPotion(PotionCraftState.CreatedPotion);
            createdPotion.PotionName = baseFluidState.currentvalue;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
            ChangBaseFluid(PotionCraftState.Potion);
        }
    }

    public class BaseFuildInput:PotionElement<BaseFluidState>
    {

        private BaseFluidState baseFluidState;
        public BaseFuildInput(BaseFluidState baseFluidState)
        {   Width.Set(180f, 0f);
            Height.Set(50f, 0f);
            this.baseFluidState = baseFluidState;
        }

        private void InputText()
        {
            baseFluidState.inputting = true;
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

            string value = Main.GetInputText(baseFluidState.currentvalue);
            if (value != baseFluidState.currentvalue)
            {
                baseFluidState.currentvalue = value;
            }
        }

        private void EndInputText()
        {
            Main.blockInput = false;
            baseFluidState.inputting = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.mouseLeft && !IsMouseHovering)
            {
                EndInputText();
            }
            if (baseFluidState.inputting)
            {
                HandleInputText();
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            InputText();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (baseFluidState.inputting)
            {
                spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
                HandleInputText();
                Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
            }
            Utils.DrawBorderString(spriteBatch, baseFluidState.currentvalue, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
        }

    }
    

}

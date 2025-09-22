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
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BaseFluidState> Potionslot;

        private MaterialSlot<BaseFluidState> Materialslot;

        private CreatedPotionSlot<BaseFluidState> CreatedPotionSlot;

        public BaseFuildInput baseFuildInput;

        public BaseFuildInput SignaturesInput;

        private BaseFuildButtun baseFuildButtun;

        public override void OnInitialize()
        {
            baseFuildInput = new(PotionCraftState.AsPotion(CreatedPotion).PotionName)
            {
                HAlign = 0.5f,
                VAlign = 0.65f,
            };
            Append(baseFuildInput);

            SignaturesInput = new(PotionCraftState.AsPotion(CreatedPotion).Signatures)
            {
                HAlign = 1f,
                VAlign = 0.65f,
            };
            Append(SignaturesInput);
            
            Potionslot = new(this,UpdateText)
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
            
            baseFuildButtun = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.75f,
            };
            Append(baseFuildButtun);
            
        }

        private void UpdateText()
        {
            baseFuildInput.UpdateText(PotionCraftState.AsPotion(CreatedPotion).PotionName);
            SignaturesInput.UpdateText(PotionCraftState.AsPotion(CreatedPotion).Signatures);
        }

        public override void Update(GameTime gameTime)
        {
            baseFuildInput.Update(gameTime);
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
            createdPotion.PotionName = baseFluidState.baseFuildInput.Currentvalue;
            createdPotion.Signatures =  baseFluidState.SignaturesInput.Currentvalue;
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

        public bool Inputting;
        
        public string Currentvalue;
        public BaseFuildInput(string currentValue=null)
        {   
            Width.Set(180f, 0f);
            Height.Set(50f, 0f);
            Currentvalue = currentValue;
        }

        public void UpdateText(string currentValue)
        {
            Currentvalue = currentValue;
        }

        private void InputText()
        {
            Inputting = true;
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

            string value = Main.GetInputText(Currentvalue);
            if (value != Currentvalue)
            {
                Currentvalue = value;
            }
        }

        private void EndInputText()
        {
            Main.blockInput = false;
            Inputting = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.mouseLeft && !IsMouseHovering)
            {
                EndInputText();
            }
            if (Inputting)
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
            if (Inputting)
            {
                spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
                HandleInputText();
                Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
            }
            Utils.DrawBorderString(spriteBatch, Currentvalue, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
        }

    }
    

}

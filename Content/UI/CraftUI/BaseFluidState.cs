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
using ReLogic.Localization.IME;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent;
using static PotionCraft.Content.System.LanguageHelper;
using Luminance.Common.Utilities;
using static PotionCraft.Assets;

namespace PotionCraft.Content.UI.CraftUI
{
    public class BaseFluidState: AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.BaseFluid;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BaseFluidState> Potionslot;

        private MaterialSlot<BaseFluidState> Materialslot;

        public BaseFuildInput BaseFuildInput;

        public BaseFuildInput SignaturesInput;

        private BaseFuildButtun BaseFuildButtun;
        
        private BaseFuildSwitch AutoUseSwitch;
        
        private BaseFuildSwitch EditorSwitch;

        private UIElement Area;

        public override void OnInitialize()
        {
            HAlign = 0.5f;
            VAlign = 0.5f;
            Area = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };
            Area.Width.Set(712f, 0);
            Area.Height.Set(536f, 0);
            Append(Area);

            BaseFuildInput = new("")
            {
                HAlign = 0.5f,
                VAlign = 0.65f,
            };
            Append(BaseFuildInput);

            SignaturesInput = new("")
            {
                HAlign = 1f,
                VAlign = 0.65f,
            };
            Append(SignaturesInput);

            AutoUseSwitch = new (this,"AutoUse",()=>
            {
                if (Potion.ModItem is not BasePotion) return;
                AsPotion(Potion).AutoUse = AutoUseSwitch.Switchvalue;
            })
            {
                HAlign = 0.6f,
                VAlign = 0.65f,
            };
            Append(AutoUseSwitch);

            EditorSwitch = new BaseFuildSwitch(this,"Editor",()=>
            {
                if (Potion.ModItem is not BasePotion) return;
                AsPotion(Potion).EditorName = EditorSwitch.Switchvalue ? EditorSwitch.SwitchText : "";
            })
            {
                HAlign = 0.6f,
                VAlign = 0.60f,
            };
            Append(EditorSwitch);
            
            Potionslot = new(this, () => {
                if (Potion.ModItem is not BasePotion) return;
                BaseFuildInput.Recordvalue =ParseTextToList(AsPotion(Potion).PotionName);
                SignaturesInput.Recordvalue =ParseTextToList(AsPotion(Potion).Signatures);
            })
            {
                HAlign = 0.2f,
                VAlign = 0.45f,
                TexturePath="Pixel"
            };
            Area.Append(Potionslot);

            Materialslot = new(this, () => {
                if (Potion.ModItem is not BasePotion) return;
                BaseFuildInput.Recordvalue =ParseTextToList(AsPotion(Potion).PotionName); 
            })
            {
                HAlign = 0.55f,
                VAlign = 0.5f,
            };

            BaseFuildButtun = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.75f,
            };
            Append(BaseFuildButtun);
            
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsLoaded())
                return;
            if (Area.IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;   
            BaseFuildInput.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UITexture("BaseFluid").Value, Area.GetDimensions().ToRectangle().TopLeft(), Color.White);
            base.Draw(spriteBatch);
        }

    }

    public class BaseFuildButtun: PotionElement<BaseFluidState>
    {
        private BaseFluidState BaseFluidState;
        public BaseFuildButtun(BaseFluidState baseFluidState)
        {
            BaseFluidState = baseFluidState;
            PotionCraftState = baseFluidState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void ChangBaseFluid(Item potion)
        {
            BasePotion createdPotion = AsPotion(PotionCraftState.Potion);
            createdPotion.CustomName = "[c/AA1AAA:A<LDC#]";
            createdPotion.CanCustomName = true;
            createdPotion.Signatures =  BaseFluidState.SignaturesInput.Showstring;
            if (PotionCraftState.Material.IsAir) return;
            createdPotion.DrawPotionList.Clear();
            createdPotion.DrawCountList.Clear();
            createdPotion.DrawPotionList.Add(PotionCraftState.Material.type);
            createdPotion.DrawCountList.Add(1);
            createdPotion.BaseName =PotionCraftState.Material.Name;
            if (PotionCraftState.Material.ModItem is not BaseCustomMaterials) return;
            var materialData =AsMaterial(PotionCraftState.Material);
            createdPotion.PotionUseSounds =  (int)materialData.Materialdata.PotionUseSound;
            createdPotion.Item.UseSound =
                (SoundStyle)BasePotion.ItemSound.Invoke(null, [createdPotion.PotionUseSounds]);
            createdPotion.PotionUseStyle = materialData.Materialdata.UseStyle;
            createdPotion.Item.useStyle = createdPotion.PotionUseStyle;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
            ChangBaseFluid(PotionCraftState.Potion);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
            base.Draw(spriteBatch);
        }

    }

    public class BaseFuildInput:PotionElement<BaseFluidState>
    {

        public bool Inputting;
        
        public (string,string) Currentvalue;

        public List<(string,string)> Recordvalue;
        
        public string Showstring;

        private string SelectColor = Deafult_Hex;
        
        public Action Onchange;
        public BaseFuildInput(string currentValue=null)
        {   
            Width.Set(180f, 0f);
            Height.Set(50f, 0f);
            Recordvalue = ParseTextToList(currentValue);
        }

        public BaseFuildInput(Action onchange,string currentvalue=null )
        {
            Recordvalue = ParseTextToList(currentvalue);
            Onchange = onchange;
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
            if (Currentvalue.Item2.Length == 0)
                Currentvalue = StListText();
            Currentvalue = (Currentvalue.Item1,Main.GetInputText(Currentvalue.Item2));
        }

        private void EndInputText()
        {
            Main.blockInput = false;
            Inputting = false;
            Recordvalue.Add(Currentvalue);
            Showstring = string.Join("", Recordvalue.Select(s=>s.Item1.Insert(10,s.Item2)));
            Onchange?.Invoke();
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

        private (string,string) StListText()
        {
            if (Recordvalue.Count == 0)
                return ("","");
            var ss = Recordvalue.Last();
            Recordvalue.Remove(Recordvalue.Last());
            return ss;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            InputText();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Utils.DrawBorderString(spriteBatch, Showstring, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
            Vector2 vector2 = GetDimensions().Position() + 
                                FontAssets.MouseText.Value.MeasureString(string.Join("",Recordvalue.Select(s=>s.Item2))) ;
            if (Inputting)
            {
                spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
                Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
                Utils.DrawBorderString(spriteBatch, Currentvalue.Item1.Insert(10, Currentvalue.Item2), vector2,
                    Color.White, 0.75f, 0f, 0f, -1);
            }
            if (Main.GameUpdateCount % 20U >= 10U)
                return;
            Utils.DrawBorderString(spriteBatch, "|", vector2+FontAssets.MouseText.Value.MeasureString(Currentvalue.Item2), Color.White, 0.75f, 0.0f, 0.0f, -1);

        }

    }

    public class BaseFuildSwitch : PotionElement<BaseFluidState>
    {
        public Action OnClick;

        private BaseFluidState BaseFluidState;
        
        public string SwitchText;

        public bool Switchvalue;
        public BaseFuildSwitch(BaseFluidState baseFluidState, string switchtext,Action onClick)
        {
            OnClick = onClick;
            SwitchText = switchtext;
            BaseFluidState = baseFluidState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }
        public void ResetValue(bool value)
        {
            Switchvalue = value;
        }
        
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            OnClick?.Invoke();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Utils.DrawBorderString(spriteBatch, SwitchText, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
            
        }
        
    }
    

}

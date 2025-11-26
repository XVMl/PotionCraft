using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using static PotionCraft.Assets;

namespace PotionCraft.Content.UI.CraftUI
{
    public delegate void SlotChange();
    public class PotionCraftState : AutoUIState
    {
        public bool Active => ActiveState;

        public override string LayersFindIndex => "Vanilla: Interface Logic 3";

        private UIElement area;

        public override void OnInitialize()
        {
            area = new UIElement()
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };
            area.Width.Set(820f, 0);
            area.Height.Set(600f, 0);
            Append(area);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
                return;
            base.Draw(spriteBatch);
            
        }
    }

    public class PotionSlot<T> : PotionElement<T> where T : AutoUIState
    {
        public string TexturePath= "Slot_Back";
        
        private Action _onChange;
        public PotionSlot(T potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public PotionSlot(T potionCraftState,Action change)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
            _onChange = change;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!PotionCraftState.Active)   return;
            switch (Main.mouseItem.IsAir)
            {
                case false when PotionCraftState.Potion.IsAir:
                    PotionCraftState.Potion = Main.mouseItem.Clone();
                    Main.LocalPlayer.HeldItem.TurnToAir();
                    Main.mouseItem.TurnToAir();
                    return;
                case true when !PotionCraftState.Potion.IsAir:
                    Main.mouseItem = PotionCraftState.Potion.Clone();
                    PotionCraftState.Potion.TurnToAir();
                    break;
            }
            
            _onChange?.Invoke();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var tex = UITexture(TexturePath).Value;
            spriteBatch.Draw(tex, GetDimensions().ToRectangle().TopLeft(),Color.White);
            if (PotionCraftState.Potion.IsAir) return;
            Main.inventoryScale = 2.30f;
            ItemSlot.Draw(spriteBatch, ref PotionCraftState.Potion, 21, GetDimensions().Position());
            if (!IsMouseHovering) return;
            Main.LocalPlayer.mouseInterface = true;
            Main.HoverItem = PotionCraftState.Potion.Clone();
        }

    }

    public class MaterialSlot<T> : PotionElement<T> where T : AutoUIState
    {
        public string TexturePath = "Slot_Back";

        private Action _onChange;
        public MaterialSlot(T potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public MaterialSlot(T potionCraftState,Action onChange)
        {
            _onChange = onChange;
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!PotionCraftState.Active) return;
            switch (Main.mouseItem.IsAir)
            {
                case false when PotionCraftState.Material.IsAir:
                {
                    if (!IsMaterial(Main.mouseItem)&&Main.mouseItem.ModItem is not BasePotion)
                    {
                        return;
                    }
                    PotionCraftState.Material = Main.mouseItem.Clone();
                    Main.LocalPlayer.HeldItem.TurnToAir();
                    Main.mouseItem.TurnToAir();
                    return;
                }
                case true when !PotionCraftState.Material.IsAir:
                    Main.mouseItem = PotionCraftState.Material.Clone();
                    PotionCraftState.Material.TurnToAir();
                    break;
            }
            
            _onChange?.Invoke();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UITexture(TexturePath).Value, GetDimensions().ToRectangle(), Color.White);
            if (PotionCraftState.Material.IsAir) return;
            Main.inventoryScale = 2.3f;
            ItemSlot.Draw(spriteBatch, ref PotionCraftState.Material, 21, GetDimensions().Position());
            if (!IsMouseHovering) return;
            Main.LocalPlayer.mouseInterface = true;
            Main.HoverItem = PotionCraftState.Material.Clone();
        }

    }

    public class CreatedPotionSlot<T> : PotionElement<T> where T : AutoUIState
    {

        public CreatedPotionSlot(T potionCraftState)
        {
            PotionCraftState = potionCraftState;
            Width.Set(80f, 0f);
            Height.Set(80f, 0f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!PotionCraftState.Active) return;
            if (!Main.mouseItem.IsAir || PotionCraftState.CreatedPotion.IsAir) return;
            Main.mouseItem = PotionCraftState.CreatedPotion.Clone();
            PotionCraftState.CreatedPotion.TurnToAir();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Panel, GetDimensions().ToRectangle(), Color.White);
            if (PotionCraftState.CreatedPotion.IsAir) return;
            Main.inventoryScale = 2.3f;
            ItemSlot.Draw(spriteBatch, ref PotionCraftState.CreatedPotion, 21, GetDimensions().Position());
            if (!IsMouseHovering) return;
            Main.LocalPlayer.mouseInterface = true;
            Main.HoverItem = PotionCraftState.CreatedPotion.Clone();
        }

    }

}

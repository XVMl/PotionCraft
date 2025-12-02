using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using PotionCraft.Content.Items;
using static PotionCraft.Assets;
using static PotionCraft.Content.System.LanguageHelper;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria.UI;
using Terraria.ModLoader;
using System.Runtime.CompilerServices;
using PotionCraft.Content.System.AutoLoaderSystem;
using Terraria.GameContent.Creative;
using Newtonsoft.Json.Linq;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.ID;

namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSynopsis: PotionElement<BrewPotionState>
    {
        private BrewPotionState _brewPotionState;
        
        private Button<BrewPotionState> coloreelectorbutton;

        private Button<BrewPotionState> _coloreelectorbutton;

        private Button<BrewPotionState> _inputbutton;

        private Input _potionname;

        private Input _potionremarks;

        private ItemIcon<BrewPotionState> _potionicon;

        public PotionSynopsis(BrewPotionState brewPotionState)
        {
            _brewPotionState = brewPotionState;
            PotionCraftState = brewPotionState;
            Width.Set(342, 0);
            Height.Set(486, 0);

            _potionicon = new(brewPotionState, brewPotionState.PreviewPotion)
            {
                Name = "SynopsisIcon",
                ExtraInformation = true,
                OnClick = () =>
                {
                    PotionCraftUI.UIstate.TryGetValue(nameof(PotionBaseSelect), out var state);
                    state.Top.Set(300, 0);
                    state.A = 0;
                    state.Active = !state.Active;
                }
            };
            _potionicon.Top.Set(38, 0);
            _potionicon.Left.Set(124, 0);
            Append(_potionicon);

            coloreelectorbutton = new(Assets.UI.ColorSelector, Color.White,brewPotionState);
            coloreelectorbutton.Name = "MarksSelector";
            coloreelectorbutton.Width.Set(18, 0);
            coloreelectorbutton.Height.Set(18, 0);
            coloreelectorbutton.Top.Set(305, 0);
            coloreelectorbutton.Left.Set(26, 0);
            coloreelectorbutton.OnClike = () =>
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(ColorSelector), out var state);
                state.A = 0;
                state.Top.Set(270, 0);
                state.Active = !state.Active;
            };
            Append(coloreelectorbutton);

            _coloreelectorbutton = new(Assets.UI.ColorSelector, Color.White, brewPotionState)
            {
                Active = false,
                A = 0f,
                Name = "NameSelector",
                TransitionAnimation = () =>
                {
                    var top = MathHelper.Lerp(_coloreelectorbutton.Top.Pixels, _coloreelectorbutton.Active ? 216 : 186, .05f);
                    _coloreelectorbutton.A = MathHelper.Lerp(_coloreelectorbutton.A, _coloreelectorbutton.Active ? 1 : 0, .05f);
                    _coloreelectorbutton.Top.Set(top, 0);
                },
                OnClike = () =>
                {
                    if (!_coloreelectorbutton.Active) return;
                    PotionCraftUI.UIstate.TryGetValue(nameof(ColorSelector), out var state);
                    state.A = 0;
                    state.Top.Set(270, 0);
                    state.Active = !state.Active;
                }
            };
            _coloreelectorbutton.Width.Set(18, 0);
            _coloreelectorbutton.Height.Set(18, 0);
            _coloreelectorbutton.Top.Set(228, 0);
            _coloreelectorbutton.Left.Set(298, 0);
            Append(_coloreelectorbutton);

            _inputbutton = new(Assets.UI.Input, Color.White,brewPotionState);
            _inputbutton.Name = "EditName";
            _inputbutton.Width.Set(18, 0);
            _inputbutton.Height.Set(18, 0);
            _inputbutton.Top.Set(245, 0);
            _inputbutton.Left.Set(298, 0);
            
            _inputbutton.OnClike = () =>
            {
                _coloreelectorbutton.Active = !_coloreelectorbutton.Active;
            };
            Append(_inputbutton);

            _potionname = new(brewPotionState);
            _potionname.Name = "PotionName";
            _potionname.Canedit = false;
            _potionname.Width.Set(246, 0);
            _potionname.Height.Set(90, 0);
            _potionname.Onchange = () =>
            {
                _brewPotionState.CreatPotion.CustomName = _potionname.Showstring;
            };
            _potionname.Left.Set(46, 0);
            _potionname.Top.Set(170,0);
            Append(_potionname);

            _potionremarks = new(brewPotionState)
            {
                Name = "Marks",
                Canedit = true,
                Onchange = () =>
                {
                    _brewPotionState.CreatPotion.Signatures = _potionremarks.Showstring;
                }
            };
            _potionremarks.Width.Set(246, 0);
            _potionremarks.Height.Set(146, 0);
            _potionremarks.Left.Set(46, 0);
            _potionremarks.Top.Set(300, 0);
            _potionremarks.Onchange = () =>
            {
                _brewPotionState.CreatPotion.Signatures = _potionremarks.Showstring;
            };
            Append(_potionremarks);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _potionremarks?.Update(gameTime);
            _coloreelectorbutton?.Update(gameTime);
        }

        public void SynopsisUpdate()
        {
            var potion = _brewPotionState.PreviewPotion.ModItem as BasePotion;
            _potionname.Recordvalue = potion.CanCustomName ? ParseText(potion.CustomName) : ParseText(potion.PotionName);
            _potionremarks.Recordvalue = ParseText(potion.Signatures);
            _potionname.Refresh();
            _potionremarks.Refresh();
            _potionicon.Item = _brewPotionState.PreviewPotion;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            var tex = UITexture("UI1").Value;
            spriteBatch.Draw(tex, GetDimensions().ToRectangle().TopLeft(), Color.White);    
            base.Draw(spriteBatch);
        }

    }
    
    public class ItemIcon<T> : PotionElement<T> where T : AutoUIState
    {
        public Item Item;

        public bool ExtraInformation;

        public bool Slot;

        public Action OnClick;
        public ItemIcon(T state,Item item) 
        { 
            PotionCraftState = state;
            Width.Set(102f, 0f);
            Height.Set(104f, 0f);
            Item = item;
        }

        public ItemIcon(T state)
        {
            PotionCraftState = state;
            Width.Set(102f, 0f);
            Height.Set(104f, 0f);
        }

        public void AddItem()
        {
            if (!Slot) return;
            Item ??= new Item();
            switch (!Main.mouseItem.IsAir)
            {
                case true when Item.IsAir:
                    Item = Main.mouseItem.Clone();
                    Item.stack = 1;
                    Main.LocalPlayer.HeldItem.stack -= 1;
                    Main.mouseItem.stack -= 1;
                    return;
                case false when !Item.IsAir:
                    Main.mouseItem = Item.Clone();
                    Item.TurnToAir();
                    break;
            }
            if (ItemID.Sets.IsFood[Item.type])
            {
                PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
                BrewPotionState brewPotionState = (BrewPotionState)state;
                brewPotionState.CreatPotion.Item.useStyle = Item.useStyle;
                brewPotionState.CreatPotion.Item.UseSound = Item.UseSound;
                brewPotionState.CreatPotion.IconID = Item.type;
                brewPotionState.CreatPotion.PotionUseStyle = Item.useStyle;
                brewPotionState.CreatPotion.useAnimation = Item.useAnimation;
                brewPotionState.CreatPotion.useTime = Item.useTime;
                //brewPotionState.CreatPotion.Item.holdStyle = Item.holdStyle;
                //brewPotionState.CreatPotion.PotionUseSounds = Item.UseSound;
                Texture2D value = TextureAssets.Item[Item.type].Value;
                Rectangle frame = ((Main.itemAnimations[Item.type] == null) ? value.Frame() : Main.itemAnimations[Item.type].GetFrame(value));

                //if (count > 0)
                brewPotionState.CreatPotion.Frame = frame;
                brewPotionState.Refresh();
            }

            if (Item.ModItem is null)
                return;

            //Main.NewText(Item.ModItem.Name);

            if (JsonLoader.Materials.Contains(Item.ModItem.Name))
            {
                var item = new Item();
                ModContent.TryFind("PotionCraft", Item.ModItem.Name, out ModItem modItem);
                item.SetDefaults(modItem.Type);
                
                PotionCraftUI.UIstate.TryGetValue(nameof(BrewPotionState), out var state);
                BrewPotionState brewPotionState = (BrewPotionState)state;
                brewPotionState.CreatPotion.Item.useStyle = item.useStyle;
                brewPotionState.CreatPotion.Item.UseSound = item.UseSound;
                brewPotionState.CreatPotion.IconID = item.type;
                brewPotionState.CreatPotion.PotionUseStyle = item.useStyle;
                brewPotionState.CreatPotion.useAnimation = item.useAnimation;
                brewPotionState.CreatPotion.useTime = item.useTime;
                Texture2D value = TextureAssets.Item[item.type].Value;
                Rectangle frame = ((Main.itemAnimations[item.type] == null) ? value.Frame() : Main.itemAnimations[item.type].GetFrame(value));
                brewPotionState.CreatPotion.Frame = frame;
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            OnClick?.Invoke();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var tex = Assets.UI.ItemSlot.Value;
            if (IsMouseHovering)
                tex = Assets.UI.ItemSlotActive.Value;
            spriteBatch.Draw(tex, GetDimensions().ToRectangle(),Color.White);
            if(Item is null && Slot)
                spriteBatch.Draw(Assets.UI.ItemSlotAdd.Value, GetDimensions().ToRectangle(), Color.White);

            if (Item is null)
                return;
            Main.inventoryScale = 2.030f;
            ItemSlot.Draw(spriteBatch, ref Item, 21, GetDimensions().Position());
            if (!IsMouseHovering || !ExtraInformation) return;
            Main.LocalPlayer.mouseInterface = true;
            Main.HoverItem = Item;
        }
    }

    public class SelectIcon : PotionElement<BrewPotionState>
    {
        public SelectIcon()
        {
            Width.Set(342, 0);
            Height.Set(224, 0);
            
        }
    }

}

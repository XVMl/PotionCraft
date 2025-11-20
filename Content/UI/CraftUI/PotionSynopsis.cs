using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using PotionCraft.Content.Items;
using static PotionCraft.Assets;
using static PotionCraft.Content.System.LanguageHelper;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using System.Runtime.CompilerServices;

namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSynopsis: PotionElement<BrewPotionState>
    {
        private BrewPotionState _brewPotionState;
        
        private Button coloreelectorbutton;

        private Button _coloreelectorbutton;

        private Button _inputbutton;

        private Input _potionname;

        private Input _potionremarks;

        private ItemIcon _potionicon;

        public PotionSynopsis(BrewPotionState brewPotionState)
        {
            _brewPotionState = brewPotionState;
            PotionCraftState = brewPotionState;
            Width.Set(342, 0);
            Height.Set(414, 0);

            _potionicon = new(brewPotionState, brewPotionState.PreviewPotion);
            _potionicon.Top.Set(38, 0);
            _potionicon.Left.Set(124, 0);
            _potionicon.OnClick = () =>
            {
                Main.NewText("CD");
            };
            Append(_potionicon);

            coloreelectorbutton = new(Assets.UI.ColorSelector, Color.White,brewPotionState);
            coloreelectorbutton.Width.Set(18, 0);
            coloreelectorbutton.Height.Set(18, 0);
            coloreelectorbutton.Top.Set(290, 0);
            coloreelectorbutton.Left.Set(26, 0);
            coloreelectorbutton.OnClike = () =>
            {
                brewPotionState.colorSelector.Active = !brewPotionState.colorSelector.Active;
                brewPotionState?.colorSelector.TransitionAnimation?.Invoke();
            };
            Append(coloreelectorbutton);

            _coloreelectorbutton = new(Assets.UI.ColorSelector, Color.White,brewPotionState);
            _coloreelectorbutton.Width.Set(18, 0);
            _coloreelectorbutton.Height.Set(18, 0);
            _coloreelectorbutton.Top.Set(216, 0);
            _coloreelectorbutton.Left.Set(298, 0);
            _coloreelectorbutton.TransitionAnimation = () =>
            { 
                var top = MathHelper.Lerp(_coloreelectorbutton.Top.Pixels, _coloreelectorbutton.Active ? 216 : 186, .05f);
                _coloreelectorbutton.Top.Set(top, 0);
            };
            _coloreelectorbutton.OnClike = () =>
            {
                if (!_coloreelectorbutton.Active) return;
                brewPotionState.colorSelector.Active = !brewPotionState.colorSelector.Active;
                brewPotionState?.colorSelector.TransitionAnimation?.Invoke();
            };

            Append(_coloreelectorbutton);

            _inputbutton = new(Assets.UI.Input, Color.White,brewPotionState);
            _inputbutton.Width.Set(18, 0);
            _inputbutton.Height.Set(18, 0);
            _inputbutton.Top.Set(238, 0);
            _inputbutton.Left.Set(298, 0);
            
            _inputbutton.OnClike = () =>
            {
                _coloreelectorbutton.Active = !_coloreelectorbutton.Active;
            };
            Append(_inputbutton);

            _potionname = new(brewPotionState);
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
                Canedit = true,
                Onchange = () =>
                {
                    _brewPotionState.CreatPotion.Signatures = _potionremarks.Showstring;
                }
            };
            _potionremarks.Width.Set(246, 0);
            _potionremarks.Height.Set(106, 0);
            _potionremarks.Left.Set(46, 0);
            _potionremarks.Top.Set(276, 0);
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
    
    public class ItemIcon : PotionElement<BrewPotionState>
    {
        public Item Item;

        private BrewPotionState BrewPotionState;

        public Action OnClick;
        public ItemIcon(BrewPotionState brewPotionState,Item item) 
        { 
            PotionCraftState = brewPotionState;
            BrewPotionState = brewPotionState;
            Width.Set(102f, 0f);
            Height.Set(104f, 0f);
            Item = item;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            OnClick?.Invoke();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.UI1.Value, GetDimensions().ToRectangle(),new Rectangle(124,38,102,104), Color.White);
            if (Item is null)
                return;
            Main.inventoryScale = 2.30f;
            ItemSlot.Draw(spriteBatch, ref Item, 21, GetDimensions().Position());

            //spriteBatch.Draw(tex, GetDimensions().ToRectangle().TopLeft(), Color.White);
            //if (PotionCraftState.Potion.IsAir) return;
            //if (!IsMouseHovering) return;
            //Main.LocalPlayer.mouseInterface = true;
            //Main.HoverItem = PotionCraftState.Potion.Clone();
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

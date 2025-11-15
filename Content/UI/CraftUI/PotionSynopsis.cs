using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.System;
using Terraria;
using Microsoft.Xna.Framework;
using PotionCraft.Content.Items;
using static PotionCraft.Assets;
using static PotionCraft.Content.System.LanguageHelper;
using Terraria.GameContent.UI.Elements;

namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionSynopsis: PotionElement<BrewPotionState>
    {
        private BrewPotionState _brewPotionState;
        
        private Button coloreelectorbutton;

        private Input _potionname;

        private Input _potionremarks;

        public PotionSynopsis(BrewPotionState brewPotionState)
        {
            _brewPotionState = brewPotionState;
            PotionCraftState = brewPotionState;
            Width.Set(342, 0);
            Height.Set(414, 0);
            coloreelectorbutton = new(Assets.UI.ColorSelector, Color.White);
            coloreelectorbutton.Width.Set(18, 0);
            coloreelectorbutton.Height.Set(18, 0);
            coloreelectorbutton.Top.Set(290, 0);
            coloreelectorbutton.Left.Set(26, 0);
            coloreelectorbutton.OnClike = () =>
            {
                brewPotionState?.colorSelector.TransitionAnimation?.Invoke();
            };
            
            Append(coloreelectorbutton);
            _potionname = new(brewPotionState);
            _potionname.Onchange = () =>
            {
                _brewPotionState.CreatPotion.CustomName = _potionname.Showstring;
            };
            _potionname.Left.Set(30, 0);
            _potionname.Top.Set(100,0);


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
        }

        public void SynopsisUpdate()
        {
            var potion = _brewPotionState.PreviewPotion.ModItem as BasePotion;
            _potionname.Recordvalue = potion.CanCustomName ? ParseText(potion.CustomName) : ParseText(potion?.PotionName);
            _potionremarks.Recordvalue = ParseText(potion.Signatures);
            
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            var tex = UITexture("UI1").Value;
            spriteBatch.Draw(tex, GetDimensions().ToRectangle().TopLeft(), Color.White);    
            base.Draw(spriteBatch);
        }

    }
    
    
}

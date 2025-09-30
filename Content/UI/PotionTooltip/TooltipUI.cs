using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.UI.PotionTooltip
{
    /// <summary>
    /// 此类用于修改药水的提示栏
    /// </summary>
    public class TooltipUI : AutoUIState
    {
        public override bool IsLoaded() => Show;
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private bool Show;
        private UIElement Area;

        private static BasePotion ShowBasePotion= ModContent.GetInstance<BasePotion>();

        private PotionIngredients PotionIngredients;
        public override void OnInitialize()
        {
            Area = new();
            Area.Width.Set(300f, 0f);
            Area.Height.Set(300f, 0f);
            Append(Area);
            PotionIngredients = new(this) { 
                HAlign= 0.5f,
                VAlign = 1f,
            };
            PotionIngredients.Width.Set(300, 0);
            PotionIngredients.Height.Set(500, 0);
            Area.Append(PotionIngredients);
        }
        /// <summary>
        /// 检查两瓶药水是否完全相同，是则返回true
        /// </summary>
        /// <param name="oldPotion"></param>
        /// <param name="newPotion"></param>
        /// <returns></returns>
        public static bool CheckPotion(BasePotion oldPotion, BasePotion newPotion)
        {
            if (oldPotion.PotionName != newPotion.PotionName) return false;
           
            if(oldPotion.PotionDictionary.Count!=newPotion.PotionDictionary.Count) return false;

            return oldPotion.PotionDictionary.All(item => newPotion.PotionDictionary.Any( s => s.Value.BuffId == item.Value.BuffId && s.Value.BuffTime == item.Value.BuffTime));            
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = Main.MouseScreen;
            Area.Left.Set(pos.X, 0);
            Area.Top.Set(pos.Y, 0);
            Show = Main.HoverItem.type.Equals(ModContent.ItemType<BasePotion>());
            if (!Show) return;
            if (CheckPotion(ShowBasePotion, PotionElement<MashUpState>.AsPotion(Main.HoverItem))) return;
            PotionIngredients.UIgrid.Clear();
            ShowBasePotion = PotionElement<TooltipUI>.AsPotion(Main.HoverItem);
            PotionIngredients.SetPotionCraftState(this, Main.HoverItem);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Show) return;
            spriteBatch.Draw(Assets.UI.Tooltip, Area.GetDimensions().ToRectangle(), Color.White);
            
        }
    }
}

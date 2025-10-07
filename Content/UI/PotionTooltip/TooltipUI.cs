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
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

using static PotionCraft.Content.System.LanguageHelper;

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

        private UIElement NameArea;

        private static BasePotion ShowBasePotion= ModContent.GetInstance<BasePotion>();

        private PotionIngredients PotionIngredients;

        private UIText PotionName;
        public override void OnInitialize()
        {
            Area = new();
            NameArea = new();
            NameArea.Width.Set(363f, 0f);
            NameArea.Height.Set(123f, 0f);
            Append(NameArea);
            Area.Width.Set(363f, 0f);
            Area.Height.Set(510f, 0f);
            Area.Top.Set(140f, 0f);
            Append(Area);
            PotionIngredients = new(this);
            PotionIngredients.Top.Set(60, 0);
            PotionIngredients.Left.Set(20, 0);
            PotionIngredients.Width.Set(350, 0);
            PotionIngredients.Height.Set(500, 0);
            Area.Append(PotionIngredients);
            PotionName = new("");
            PotionName.Left.Set(35, 0);
            PotionName.Top.Set(40, 0);
            NameArea.Append(PotionName);
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

            return oldPotion.PotionDictionary.All(item => newPotion.PotionDictionary.Any(s => s.Value.BuffId == item.Value.BuffId && s.Value.BuffTime == item.Value.BuffTime));
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = Main.MouseScreen+new Vector2(20,20);
            NameArea.Left.Set(pos.X, 0);
            NameArea.Top.Set(pos.Y, 0);
            Area.Left.Set(pos.X, 0);
            Area.Top.Set(pos.Y + NameArea.Height.Pixels+20, 0);
            Show = Main.HoverItem.type.Equals(ModContent.ItemType<BasePotion>());
            if (!Show) return;
            if (CheckPotion(ShowBasePotion, PotionElement<MashUpState>.AsPotion(Main.HoverItem))) return;
            PotionIngredients.UIgrid.Clear();
            ShowBasePotion = PotionElement<TooltipUI>.AsPotion(Main.HoverItem);
            PotionIngredients.SetPotionCraftState(this, Main.HoverItem);
            CalculateHeight();
        }

        private void CalculateHeight( )
        {
            var data = WrapTextWithColors(ShowBasePotion.PotionName, 40);
            PotionName.SetText(data.Item1);
            //PotionIngredients.Top.Set(50+data.Item2*21, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Show) return;
            spriteBatch.Draw(Assets.UITexture("ToolName").Value, NameArea.GetDimensions().ToRectangle(), Color.White);
            if(!PotionCraftModPlayer.PotionCraftKeybind.Current) return;
            var AreaRectangle = Area.GetDimensions().ToRectangle();
            var height = (int)Area.Height.Pixels;
            for (;;)
            {
                switch (height)
                {
                    case > 440:
                        spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X,AreaRectangle.Y+40,363,440), new(0, 40, 363, 440),Color.White);
                        height -= 440;
                        break;
                    case > 0:
                        spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X,AreaRectangle.Y+40,363,height), new(0, 40, 363, height),Color.White);
                        height = 0;
                        break;
                }
                if (height==0) break;
            }
            spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X,AreaRectangle.Y,363,27), new(0, 483, 363, 27), Color.White);
        }
    }
}

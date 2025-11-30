using Microsoft.Build.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using ReLogic.Graphics;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social.Steam;
using Terraria.UI;
using static PotionCraft.Assets;
using static PotionCraft.Content.System.LanguageHelper;

namespace PotionCraft.Content.UI.PotionTooltip
{
    /// <summary>
    /// 此类用于修改药水的提示栏
    /// </summary>
    public class TooltipUI : AutoUIState
    {
        public override bool Isload()=> true;
        public override string LayersFindIndex => "Vanilla: Mouse Item / NPC Head";

        //private bool ShowToolTip;
        
        private UIElement Area;

        private UIElement NameArea;

        private static BasePotion ShowBasePotion= ModContent.GetInstance<BasePotion>();

        private PotionIngredients PotionIngredients;

        private UIText PotionName;

        private UIText PotionMarks;

        public override void OnInitialize()
        {
            Area = new();
            NameArea = new();
            NameArea.Width.Set(360f, 0f);
            NameArea.Height.Set(123f, 0f);
            Append(NameArea);
            Area.Width.Set(360f, 0f);
            Area.Height.Set(510f, 0f);
            Area.Top.Set(140f, 0f);
            Append(Area);
            PotionIngredients = new(this);
            PotionIngredients.Top.Set(35, 0);
            PotionIngredients.Left.Set(50, 0);
            PotionIngredients.Width.Set(350, 0);
            PotionIngredients.Height.Set(500, 0);
            Area.Append(PotionIngredients);
            PotionName = new("");
            PotionName.Left.Set(25, 0);
            PotionName.Top.Set(40, 0);
            NameArea.Append(PotionName);
            PotionMarks = new("");
            PotionMarks.Top.Set(50, 0);
            PotionMarks.Left.Set(25 , 0);
            NameArea.Append(PotionMarks);
        }
        /// <summary>
        /// 检查两瓶药水是否完全相同，是则返回true
        /// </summary>
        /// <param name="oldPotion"></param>
        /// <param name="newPotion"></param>
        /// <returns></returns>
        public static bool CheckPotion(BasePotion oldPotion, BasePotion newPotion)
        {
            if (oldPotion._Name != newPotion._Name) return false;

            if(oldPotion.CustomName !=newPotion.CustomName) return false;

            if (oldPotion.Signatures != newPotion.Signatures) return false;

            if(oldPotion.PotionDictionary.Count!=newPotion.PotionDictionary.Count) return false;

            return oldPotion.PotionDictionary.All(item => newPotion.PotionDictionary.Any(s => s.Value.BuffName == item.Value.BuffName && s.Value.BuffTime == item.Value.BuffTime));
        }

        public static bool CheckPotion_Item(Item oldPotion,Item newPotion)
        {
            return oldPotion.ModItem is BasePotion && newPotion.ModItem is BasePotion &&
                   CheckPotion(AsPotion(oldPotion), AsPotion(newPotion));
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 pos = Main.MouseScreen+new Vector2(20,20);
            NameArea.Left.Set(pos.X, 0);
            NameArea.Top.Set(pos.Y, 0);
            var h = Area.Height.Pixels + 20;
            var x = 0;
            var y = NameArea.Height.Pixels+20;
            if (pos.Y + y + h > Main.screenHeight)
            {
                x = 380;
                y = 0;
            }
            Area.Left.Set(pos.X +x, 0); 
            Area.Top.Set(pos.Y + y, 0);
            //Active = Main.HoverItem.type.Equals(ModContent.ItemType<BasePotion>());
            if (!Active) 
                return;

            if (CheckPotion(ShowBasePotion, AsPotion(Main.HoverItem)))
                return;
            PotionIngredients.UIgrid.Clear();
            NameArea.Height.Set(123, 0);
            ShowBasePotion = PotionElement<TooltipUI>.AsPotion(Main.HoverItem);
            CalculateHeight();
            PotionIngredients.SetPotionCraftState(this, Main.HoverItem);
        }

        private void CalculateHeight( )
        {
            var linetextnum = LanguageManager.Instance.ActiveCulture.Name switch
            {
                "zh-Hans" => 320,
                "en-US" => 300,
                _ => 300,
            };
            //var data = WrapTextWithColors(ShowBasePotion.PotionName, linetextnum);
            var data = WrapTextWithColors_ComPact(ShowBasePotion.PotionName, linetextnum);
            var marks= WrapTextWithColors_ComPact(ShowBasePotion.Signatures, linetextnum);
            PotionName.SetText(data.Item1);
            PotionMarks.SetText(marks.Item1);

            var buff = BuffID.Search.GetName(10);
            var textheight= FontAssets.MouseText.Value.MeasureString(data.Item1).Y;
            var marksheight = FontAssets.MouseText.Value.MeasureString(marks.Item1).Y;
            PotionMarks.Top.Set(textheight+56, 0);
            NameArea.Height.Set(textheight+marksheight + 75, 0);
            var count = ShowBasePotion.PotionDictionary.Count/2+1;
            Area.Height.Set(Math.Max(400, count * 50 + 90), 0);
            PotionIngredients.Height.Set(count*50+70, 0);
            PotionIngredients.UIgrid.Height.Set(count * 50 + 70, 0);
        }

        public static (string,bool) GetKeybind(ModKeybind key)
        {
            if (key == null || Main.dedServ) return("",false);
            var text = key.GetAssignedKeys(InputMode.Keyboard);
            if (text.Count==0) return ("[NONE]", false);
            StringBuilder sb = new(16);
            sb.Append(text[0]);
            for (int i = 1; i < text.Count; i++)
            {
                sb.Append(" / ").Append(text[i]);
            }
            return (sb.ToString(),true);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Active) return;
            var nametex = UITexture("ToolName").Value;
            var namearea = NameArea.GetDimensions().ToRectangle();
            var nameheight = namearea.Height-36;
            spriteBatch.Draw(nametex, new Rectangle(namearea.X, namearea.Y, 360, 18), new(0, 0, 360, 18), Color.White);
            for (; ; )
            {
                switch (nameheight)
                {
                    case > 120:
                        spriteBatch.Draw(nametex, new Rectangle(namearea.X, namearea.Y + 18 + namearea.Height - 36 - nameheight, 360, 120), new(0, 18, 360, 120), Color.White);
                        nameheight -= 120;
                        break;
                    case > 0:
                        spriteBatch.Draw(nametex, new Rectangle(namearea.X, namearea.Y + 18 + namearea.Height - 36 - nameheight, 360, nameheight), new(0, 18, 360, nameheight), Color.White);
                        nameheight = 0;
                        break;
                }
                if (nameheight <= 0) break;
            }

            spriteBatch.Draw(nametex, new Vector2(0, PotionMarks.Top.Pixels-20)+namearea.TopLeft(), new(0, 132, 360, 20), Color.White);
            spriteBatch.Draw(nametex, new Rectangle(namearea.X, namearea.Y + namearea.Height - 18, 360, 18), new(0, 176, 360, 18), Color.White);
            var data = GetKeybind(PotionCraftModPlayer.PotionCraftKeybind);


            if (!data.Item2)
                Utils.DrawBorderString(spriteBatch,$"{TryGetLanguagValue("KeybindsTips", data.Item1)}", namearea.TopLeft()+new Vector2(0, namearea.Height), Color.White);
            
            if (!PotionCraftModPlayer.PotionCraftKeybind.Current) 
                return;
            var AreaRectangle = Area.GetDimensions().ToRectangle();
            var height = AreaRectangle.Height-96;
            spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X, AreaRectangle.Y, 360, 48), new(0, 0, 360, 48), Color.White);
            for (; ; )
            {
                switch (height)
                {
                    case > 440:
                        spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X, AreaRectangle.Y + 48 + (int)Area.Height.Pixels - 96 - height, 360, 440), new(0, 48, 360, 440), Color.White);
                        height -= 440;
                        break;
                    case > 0:
                        spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X, AreaRectangle.Y + 48 + (int)Area.Height.Pixels - 96 - height, 360, height), new(0, 48, 360, height), Color.White);
                        height = 0;
                        break;
                }
                if (height == 0) break;
            }
            spriteBatch.Draw(Assets.UI.Tooltip, new Rectangle(AreaRectangle.X, AreaRectangle.Y + AreaRectangle.Height-48, 360, 48), new(0, 488, 360, 48), Color.White);
            
            var lock_rectangle= ShowBasePotion.CanEditor ? new Rectangle(62, 0, 18, 18) : new Rectangle(80, 0, 18, 18);
            spriteBatch.Draw(Assets.UI.Icon.Value, AreaRectangle.TopLeft()+new Vector2(60,AreaRectangle.Height-68), lock_rectangle, Deafult);

            var auto_rotation =  ShowBasePotion.AutoUse ? Main.time * .03f : 0;
            spriteBatch.Draw(Assets.UI.Icon.Value, AreaRectangle.TopLeft() + new Vector2(129, AreaRectangle.Height - 58), new Rectangle(40, 0, 18, 18), Deafult, (float)auto_rotation, new Vector2(18,18)/2, 1, SpriteEffects.None, 0);

            var packing_rectangle = ShowBasePotion.IsPackage ? new Rectangle(0, 0, 18, 18) : new Rectangle(22, 0, 18, 18); 
            spriteBatch.Draw(Assets.UI.Icon.Value, AreaRectangle.TopLeft() + new Vector2(90, AreaRectangle.Height-68), packing_rectangle, Deafult);

            
        }
    }
}

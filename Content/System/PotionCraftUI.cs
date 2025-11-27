using Luminance.Common.Utilities;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.UI;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.System
{
    public class PotionCraftUI : ModSystem
    {
        private static Type[] _UIstate = [];

        public static Dictionary<string, AutoUIState> UIstate = new();

        private static Dictionary<AutoUIState, UserInterface> keyValuePairs = new();

        private List<UserInterface> _UserInterface = new();

        public override void Load()
        {
            if (Main.dedServ)
            {
                return;
            }
            _UIstate = Mod.Code.GetTypes()
                .Where(x => x.BaseType == typeof(AutoUIState))
                .ToArray();

            foreach (var type in _UIstate)
            {
                AutoUIState _state = (AutoUIState)Activator.CreateInstance(type, null);
                UserInterface _userInterface = new();
                if (!_state.Isload())
                    continue;
                _userInterface.SetState(_state);
                _UserInterface.Add(_userInterface);
                UIstate.Add(type.Name, _state);
                keyValuePairs[_state] = _userInterface;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            foreach (UserInterface type in _UserInterface)
            {
                if (((AutoUIState)type.CurrentState).Active)
                    type.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            foreach (var item in keyValuePairs)
            {
                if (!item.Key.Active)
                {
                    continue;
                }
                var index1 = layers.FindIndex(layer => layer.Name.Equals(item.Key.LayersFindIndex));
                layers.Insert(index1, new LegacyGameInterfaceLayer(
                   "PotionCraft:" + item.Key,
                   delegate
                   {
                       item.Value.Draw(Main.spriteBatch, new GameTime());
                       return true;
                   },
                   InterfaceScaleType.UI)
               );
            }

        }

    }
    /// <summary>
    /// 包含药品界面常用方法，承载PotionState
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PotionElement<T> : UIElement where T : AutoUIState
    {
        public T PotionCraftState;

        public bool Active = true;

        public float A = 1f;

        private string _name="";
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value; 
            }
        }

        public static readonly int PurifyingCountMax = 12;

        public static readonly int MashUpCountMax = 14;

        public Vector2 SourcePotion = Vector2.Zero;

        public Vector2 Range = new(50, 50);

        public Action TransitionAnimation;

        public Action IdelAnimation;

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            if (PotionCraftState is null || !PotionCraftState.Active)
                return;
            AutoUIState.CurrentElement = Name;
            Main.NewText(AutoUIState.CurrentElement);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            if (PotionCraftState is null || !PotionCraftState.Active)
                return;
            AutoUIState.CurrentElement = string.Empty;
        }

        public bool IsPotion(Item item)
        {
            return item.ModItem is BasePotion;
        }

        public static bool IsMaterial(Item item)
        {
            if (item.consumable && item.buffType != 0)
            {
                return true;
            }
            return item.type == ModContent.ItemType<MagicPanacea>();
        }

        /// <summary>
        /// 转化为测试药剂
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static BasePotion AsPotion(Item item)
        {
            if (item.ModItem is BasePotion testPotion)
            {
                return testPotion;
            }
            Mod instance = ModContent.GetInstance<PotionCraft>();
            if (item.ModItem == null)
            {
                instance.Logger.Warn($"Item was erroneously casted to Potion");
            }
            return ModContent.GetInstance<BasePotion>();
        }

        public static BaseCustomMaterials AsMaterial(Item item)
        {
            if (item.ModItem is BaseCustomMaterials material)
            {
                return material;
            }
            return ModContent.GetInstance<BaseCustomMaterials>();
        }

        public override void Update(GameTime gameTime)
        {
            if (!PotionCraftState.Active)
                return;

            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;


            TransitionAnimation?.Invoke();
            IdelAnimation?.Invoke();

            if (SourcePotion.Equals(Vector2.Zero))
                return;


            var pos = GetDimensions().ToRectangle();
            var
            x = (float)(Utils.Lerp(pos.X, SourcePotion.X, 0.03d));
            var
            y = (float)(Utils.Lerp(pos.Y, SourcePotion.Y, 0.03d));

            if ((SourcePotion.X - Main.MouseScreen.X < Range.X && SourcePotion.Y - Main.MouseScreen.Y < Range.Y)
                && (Main.MouseScreen.X - SourcePotion.X - Width.Pixels < Range.X && Main.MouseScreen.Y - SourcePotion.Y - Height.Pixels < Range.Y))
            {
                var diff = Main.MouseScreen - new Vector2(Width.Pixels / 2, Height.Pixels / 2) - SourcePotion;
                var offset = Vector2.Divide(diff, new Vector2(Width.Pixels / 2, Height.Pixels / 2) + Range) * Range;
                x = (float)Utils.Lerp(pos.X, SourcePotion.X + offset.X, 0.03d);
                y = (float)Utils.Lerp(pos.Y, SourcePotion.Y + offset.Y, 0.03d);
            }

            Left.Set(x, 0);
            Top.Set(y, 0);
        }


    }
    /// <summary>
    /// 自动注册加载继承它的子类
    /// </summary>
    public abstract class AutoUIState : UIState
    {
        public virtual string TypeName { get; }

        public static bool ActiveState;

        public static string CurrentElement = "";

        public float A = 1f;

        public static CraftUiState CraftState;

        public Action TransitionAnimation;
        public virtual bool Isload() => false;

        public Item Potion = new();

        public Item Material = new();

        public Item CreatedPotion = new();
        /// <summary>
        /// 工艺界面的状态名称
        /// </summary>
        public enum CraftUiState
        {
            Default,
            Purification,
            MashUp,
            BaseFluid,
            BrewPotion,
        }

        public bool Active = false;

        public bool Deactive = false;

        public abstract string LayersFindIndex { get; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TransitionAnimation?.Invoke();
        }

        public static BasePotion AsPotion(Item item)
        {
            if (item.ModItem is BasePotion testPotion)
            {
                return testPotion;
            }
            Mod instance = ModContent.GetInstance<PotionCraft>();
            if (item.ModItem == null)
            {
                instance.Logger.Warn($"Item was erroneously casted to Potion");
            }
            return ModContent.GetInstance<BasePotion>();
        }

        public static bool CheckEditor(Item item, Player player)
        {
            return AsPotion(item).EditorName.Equals("") || AsPotion(item).EditorName.Equals(player.name);
        }

    }
}

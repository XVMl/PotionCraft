using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static PotionCraft.Assets;

namespace PotionCraft.Content.System
{
    public class PotionCraftUI:ModSystem
    {
        public static Type[] _UIstate = [];

        public static Dictionary<AutoUIState, UserInterface> keyValuePairs = new();

        public List<UserInterface> _UserInterface = new();

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
                _userInterface.SetState(_state);
                _UserInterface.Add(_userInterface);
                keyValuePairs[_state] = _userInterface;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            foreach (UserInterface type in _UserInterface)
            {
                UserInterface userInterface = type;
                userInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            foreach (var item in keyValuePairs)
            {
                if (!item.Key.IsLoaded())
                {
                    continue;
                }
                int Index1 = layers.FindIndex(layer => layer.Name.Equals(item.Key.Layers_FindIndex));
                layers.Insert(Index1, new LegacyGameInterfaceLayer(
                   "PotionCraft:" + item.Key.ToString(),
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

    public class PotionElement:UIElement
    {
        public bool IsPotion(Item item)
        {
            if (item.ModItem is TestPotion)
            {
                return true;
            }
            return false;
        }

        public bool IsMaterial(Item item)
        {
            if (item.consumable&&item.buffType!=0)
            {
                return true;
            }
            return false;
        }

        public void PurificatingMaterial()
        {

        }

        public TestPotion AsPotion(Item item)
        {
            if (item.ModItem is TestPotion testPotion)
            {
                return testPotion;
            }
            Mod instance = ModContent.GetInstance<PotionCraft>();
            if (item.ModItem == null)
            {
                instance.Logger.Warn($"Item was erroneously casted to Potion");
            }
            else
            {
                instance.Logger.Warn($"Item was erroneously casted to Potion");
            }
            return ModContent.GetInstance<TestPotion>();
        }

    }
    /// <summary>
    /// 自动注册加载继承它的子类
    /// </summary>
    public abstract class AutoUIState:UIState
    {
        public virtual string TypeName { get; }

        public Item Potion = new();

        public Item Material = new();

        public Item CreatedPotion=new();
        public enum CraftUIState
        {
            Purificating,
            MashUp,
            Boling
        }

        public virtual bool IsLoaded() => true;

        public abstract string Layers_FindIndex { get; }

    }
}

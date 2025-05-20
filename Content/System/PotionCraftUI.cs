using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

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

    public abstract class AutoUIState:UIState
    {
        public virtual string TypeName { get; }

        public virtual bool IsLoaded() => true;

        public abstract string Layers_FindIndex { get; }

        public static Asset<Texture2D> BaseTexture(string path) => ModContent.Request<Texture2D>("DuanWu/Content/UI/" + path);

    }
}

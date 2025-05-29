using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Luminance.Core.Hooking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace PotionCraft.Content.System
{
    
    public class PotionTooltipHook:ModSystem
    {
        public override void PostSetupContent()
        {
            LoadPotionTooltipBG();
        }
        private static void LoadPotionTooltipBG()
        {
            new ManagedILEdit("The Potion`s tooltip background", ModContent.GetInstance<PotionCraft>(), edit =>
            {
                IL_Main.MouseText_DrawItemTooltip += edit.SubscriptionWrapper;
            }, edit =>
            {
                IL_Main.MouseText_DrawItemTooltip -= edit.SubscriptionWrapper;
            }, PotionTooltipBG).Apply();
        }
        private static void PotionTooltipBG(ILContext context,ManagedILEdit edit)
        {
            ILCursor cursor = new ILCursor(context);
            MethodInfo drawInventoryBgMethod = typeof(Utils).GetMethod("DrawInvBG", new Type[]
            {
            typeof(SpriteBatch),
            typeof(Rectangle),
            typeof(Color)
            })!;
            if (!cursor.TryGotoNext(MoveType.Before, i => i.MatchCallOrCallvirt(drawInventoryBgMethod)))
            {
                edit.LogFailure("Could not find the DrawInvBG call.");
                return;
            }
            cursor.EmitDelegate(() => {
                return;
            });
            


        }

        public static void DrawInvBG(SpriteBatch sb, Rectangle R, Color c = default(Color))
        {
            DrawInvBG(sb, R.X, R.Y, R.Width, R.Height, c);
            Main.spriteBatch.Draw(Assets.UI.Button, R, Color.White);
        }
        public static void DrawInvBG(SpriteBatch sb, int x, int y, int w, int h, Color c = default(Color))
        {
            if (c == default(Color))
                c = new Color(63, 65, 151, 255) * 0.785f;

            Texture2D value = TextureAssets.InventoryBack13.Value;
            if (w < 20)
                w = 20;

            if (h < 20)
                h = 20;

            sb.Draw(value, new Rectangle(x, y, 10, 10), new Rectangle(0, 0, 10, 10), c);
            sb.Draw(value, new Rectangle(x + 10, y, w - 20, 10), new Rectangle(10, 0, 10, 10), c);
            sb.Draw(value, new Rectangle(x + w - 10, y, 10, 10), new Rectangle(value.Width - 10, 0, 10, 10), c);
            sb.Draw(value, new Rectangle(x, y + 10, 10, h - 20), new Rectangle(0, 10, 10, 10), c);
            sb.Draw(value, new Rectangle(x + 10, y + 10, w - 20, h - 20), new Rectangle(10, 10, 10, 10), c);
            sb.Draw(value, new Rectangle(x + w - 10, y + 10, 10, h - 20), new Rectangle(value.Width - 10, 10, 10, 10), c);
            sb.Draw(value, new Rectangle(x, y + h - 10, 10, 10), new Rectangle(0, value.Height - 10, 10, 10), c);
            sb.Draw(value, new Rectangle(x + 10, y + h - 10, w - 20, 10), new Rectangle(10, value.Height - 10, 10, 10), c);
            sb.Draw(value, new Rectangle(x + w - 10, y + h - 10, 10, 10), new Rectangle(value.Width - 10, value.Height - 10, 10, 10), c);
        }
    }
}

﻿using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Luminance.Core.Hooking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        static readonly FieldInfo X = typeof(Rectangle).GetField(nameof(Rectangle.X))!;

        static readonly FieldInfo Y = typeof(Rectangle).GetField(nameof(Rectangle.Y))!;

        static readonly FieldInfo W = typeof(Rectangle).GetField(nameof(Rectangle.Width))!;

        static readonly FieldInfo H = typeof(Rectangle).GetField(nameof(Rectangle.Height))!;

        static readonly FieldInfo SpriteBatch = typeof(Main).GetField(nameof(Main.spriteBatch))!;

        static readonly MethodInfo Assetsbuttion = typeof(Assets).GetMethod(nameof(Assets.UI.Button))!;

        static readonly MethodInfo White = typeof(Color).GetMethod("get_White")!;
        
        static readonly MethodInfo _DrawInvBG = typeof(Utils).GetMethod(nameof(Utils.DrawInvBG),BindingFlags.Public|BindingFlags.Static,[ 
            typeof(SpriteBatch),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(Color)
        ]);

        //static readonly MethodInfo Draw = typeof(Main).GetMethod(nameof(Main.spriteBatch.Draw), BindingFlags.Public, new Type[] {
        //    typeof(Texture2D),
        //    typeof(Vector2),
        //    typeof(Rectangle?),
        //    typeof(Color),
        //})!;

        private static void LoadPotionTooltipBG()
        {
            new ManagedILEdit("The Potion`s tooltip background", ModContent.GetInstance<PotionCraft>(), edit =>
            {
                IL_Utils.DrawInvBG_SpriteBatch_Rectangle_Color += edit.SubscriptionWrapper;
            }, edit =>
            {
                IL_Utils.DrawInvBG_SpriteBatch_Rectangle_Color -= edit.SubscriptionWrapper;
            }, PotionTooltipBG).Apply();
        }
        private static void PotionTooltipBG(ILContext context, ManagedILEdit edit)
        {
            ILCursor cursor = new ILCursor(context);
            //MethodInfo drawInventoryBgMethod = typeof(Utils).GetMethod("DrawInvBG", new Type[]
            //{
            //typeof(SpriteBatch),
            //typeof(Rectangle),
            //typeof(Color)
            //})!;
            if (!cursor.TryGotoNext(MoveType.AfterLabel,
                    i => i.MatchRet()
                ))
            {
                edit.LogFailure("Could not find the DrawInvBG call.");
                return;
            }
            //cursor.EmitLdcI4(5);
            //cursor.EmitDelegate((int x) =>
            //{
            //    Main.NewText(x);
            //});
            
            cursor.EmitLdarg0();
            cursor.EmitLdarg1();
            cursor.EmitLdfld(X);
            cursor.EmitLdarg1();
            cursor.EmitLdfld(Y);
            cursor.EmitLdarg1();
            cursor.EmitLdfld(W);
            cursor.EmitLdarg1();
            cursor.EmitLdfld(H);
            cursor.EmitCall(White);
            cursor.EmitCall(_DrawInvBG);
            cursor.EmitRet();


            //cursor.EmitNop();
            //cursor.EmitLdfld(SpriteBatch);
            //cursor.EmitCall(Assetsbuttion);
            //cursor.EmitLdarg1();
            //cursor.EmitCall(White);
            //cursor.EmitCallvirt(Draw);
            //cursor.EmitNop();
            //cursor.EmitRet();
        }


        public static void DrawInvBG(SpriteBatch sb, Rectangle R, Color c = default(Color))
        {
            DrawInvBG(sb, R.X, R.Y, R.Width, R.Height, c);
            if (!(Main.HoverItem.type == ModContent.ItemType<TestPotion>()))
            {
                return;
            }
        }
        public static void DrawInvBG(SpriteBatch sb, int x, int y, int w, int h, Color c = default(Color))
        {
            Main.NewText(x);
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

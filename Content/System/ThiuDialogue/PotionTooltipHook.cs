using Luminance.Common.Utilities;
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

namespace PotionCraft.Content.System.ThiuDialogue
{
    /// <summary>
    /// 此Hook用于修改物品提示栏背景
    /// </summary>
    public class PotionTooltipHook : ModSystem
    {
        public override void PostSetupContent()
        {
            LoadPotionTooltipBG();
        }

        static readonly FieldInfo HoverItem = typeof(Main).GetField(nameof(Main.HoverItem))!;

        static readonly FieldInfo type = typeof(Item).GetField(nameof(Item.type))!;

        static readonly FieldInfo X = typeof(Rectangle).GetField(nameof(Rectangle.X))!;

        static readonly FieldInfo Y = typeof(Rectangle).GetField(nameof(Rectangle.Y))!;

        static readonly FieldInfo W = typeof(Rectangle).GetField(nameof(Rectangle.Width))!;

        static readonly FieldInfo H = typeof(Rectangle).GetField(nameof(Rectangle.Height))!;

        public static readonly FieldInfo SpriteBatch = typeof(Main).GetField(nameof(Main.spriteBatch))!;

        static readonly MethodInfo Assetsbuttion = typeof(Assets).GetMethod(nameof(Assets.UI.Button))!;

        static readonly MethodInfo DarkSlateBlue = typeof(Color).GetMethod("get_DarkSlateBlue")!;

        static readonly MethodInfo _DrawInvBG = typeof(Utils).GetMethod(nameof(Utils.DrawInvBG), BindingFlags.Public | BindingFlags.Static, [
            typeof(SpriteBatch),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(Color)
        ]);


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
            ILCursor il= new ILCursor(context);
            if (!il.TryGotoNext(MoveType.AfterLabel,
                    i => i.MatchLdarg0(),
                    i => i.MatchLdarg1()
                ))
            {
                edit.LogFailure("Could not find the DrawInvBG call.");
                return;
            }
            if (!cursor.TryGotoNext(MoveType.AfterLabel,
                    i => i.MatchRet()
                ))
            {
                edit.LogFailure("Could not find the DrawInvBG call.");
                return;
            }
            il.EmitLdsfld(HoverItem);
            il.EmitLdfld(type);
            il.EmitLdcI4(ModContent.ItemType<BasePotion>());
            il.EmitCeq();
            il.Emit(Mono.Cecil.Cil.OpCodes.Brtrue_S, context.Instrs[cursor.Index]);

            cursor.EmitLdsfld(HoverItem);
            cursor.EmitLdfld(type);
            cursor.EmitLdcI4(ModContent.ItemType<MagicPanacea>());
            cursor.EmitCeq();
            cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
            cursor.EmitLdarg0();
            cursor.EmitLdarg1();
            cursor.EmitLdfld(X);
            cursor.EmitLdarg1();
            cursor.EmitLdfld(Y);
            cursor.EmitLdarg1();
            cursor.EmitLdfld(W);
            cursor.EmitLdarg1();
            cursor.EmitLdfld(H);
            cursor.EmitCall(DarkSlateBlue);
            cursor.EmitCall(_DrawInvBG);
        }

    }
}

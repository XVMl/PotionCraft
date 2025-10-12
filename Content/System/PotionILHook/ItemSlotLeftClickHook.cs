using System.Reflection;
using Luminance.Core.Hooking;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MonoMod.Cil;
using PotionCraft.Content.UI.PotionTooltip;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.System.PotionILHook;
/// <summary>
/// 这个钩子用于修改交换不同药剂
/// </summary>
public class ItemSlotLeftClickHook:ModSystem
{
    public override void PostSetupContent()
    {
        LoadItemSlotLeftClick();
    }

    private static void LoadItemSlotLeftClick()
    {
        new ManagedILEdit("ItemSlotLeftClick Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_ItemSlot.LeftClick_ItemArray_int_int += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_ItemSlot.LeftClick_ItemArray_int_int -= edit.SubscriptionWrapper;
        }, ItemSlotLeftClick).Apply();
    }

    private static void ItemSlotLeftClick(ILContext context, ManagedILEdit edit)
    {
        FieldInfo mouseItem = typeof(Main).GetField(nameof(Main.mouseItem))!;
        
        FieldInfo maxStack = typeof(Item).GetField(nameof(Item.maxStack))!;

        FieldInfo stack = typeof(Item).GetField(nameof(Item.stack))!;

        MethodInfo checkpotion = typeof(TooltipUI).GetMethod(nameof(TooltipUI.CheckPotion_Item))!;

        TypeInfo item = typeof(Item).GetTypeInfo();

        ILCursor cursor = new ILCursor(context);
        ILCursor il = new ILCursor(cursor);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdsfld(mouseItem),
                i => i.MatchLdfld(maxStack),
                i => i.MatchLdcI4(1)
            ) || 
            !il.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdarg0(),
                i => i.MatchLdarg2(),
                i => i.MatchLdelema(item),
                i => i.MatchLdsflda(mouseItem)
            ))
        {
            edit.LogFailure("Find ItemSlotLeftClick ERROR!");
            return;
        }

        cursor.EmitLdarg0();
        cursor.EmitLdarg2();
        cursor.EmitLdelemRef();
        cursor.EmitLdsfld(mouseItem);
        cursor.EmitCall(checkpotion);
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[il.Index]);

    }
}
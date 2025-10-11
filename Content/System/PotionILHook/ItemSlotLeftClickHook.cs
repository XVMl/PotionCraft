using System.Reflection;
using Luminance.Core.Hooking;
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
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(stack),
                i => i.MatchLdcI4(0)
            ))
        {
            edit.LogFailure("Find ItemSlotLeftClick ERROR!");
            return;
        }

        cursor.EmitLdsfld(mouseItem);
        cursor.EmitLdarg0();
        cursor.EmitLdarg2();
        cursor.EmitLdelemRef();
        cursor.EmitCallvirt(checkpotion);
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[il.Index]);

    }
}
using System;
using System.Reflection;
using Terraria.ModLoader;
using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.UI.CraftUI;
using PotionCraft.Content.UI.PotionTooltip;
using Terraria;
using PotionCraft.Content.Items;

namespace PotionCraft.Content.System.PotionILHook;

public class CanStackHook : ModSystem
{

    public override void PostSetupContent()
    {
        LoadCanStack();
    }

    private void LoadCanStack()
    {
        MethodInfo IL_CanStack = typeof(ItemLoader).GetMethod(nameof(ItemLoader.CanStack));

        try
        {
            MonoModHooks.Modify(IL_CanStack, CanStack);
            Mod.Logger.Error("Load CanStackHool Successs .");

        }
        catch (Exception e)
        {
            Mod.Logger.Error(e.ToString());
        }
    }

    private void CanStack(ILContext context)
    {

        MethodInfo AsPotion = typeof(AutoUIState).GetMethod(nameof(AutoUIState.AsPotion));

        MethodInfo CheckPotion = typeof(TooltipUI).GetMethod(nameof(TooltipUI.CheckPotion));

        MethodInfo ModItem = typeof(Item).GetMethod("get_ModItem")!;

        TypeInfo BasePotion = typeof(BasePotion).GetTypeInfo();

        ILCursor il = new ILCursor(context);

        if (!il.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdarg0()
            ))
        {
            Mod.Logger.Error("Could not find.");
            return;
        }
        il.EmitLdarg0();
        il.EmitCallvirt(ModItem);
        il.EmitIsinst(BasePotion);
        il.EmitLdnull();
        il.EmitCgtUn();
        il.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[il.Index]);
        il.EmitLdarg0();
        il.EmitCall(AsPotion);
        il.EmitLdarg1();
        il.EmitCall(AsPotion);
        il.EmitCall(CheckPotion);
        il.Emit(Mono.Cecil.Cil.OpCodes.Brtrue_S, context.Instrs[il.Index]);
        il.EmitLdcI4(0);
        il.EmitRet();

    }
}
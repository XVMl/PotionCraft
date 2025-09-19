using System;
using System.Reflection;
using Terraria.ModLoader;
using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.UI.CraftUI;
using PotionCraft.Content.UI.PotionTooltip;
using Terraria;

namespace PotionCraft.Content.System.PotionILHook;

public class CanStackHook:ModSystem
{
    public static MethodInfo AsPotion = typeof(PotionElement<>).GetMethod(nameof(PotionElement<MashUpState>.AsPotion));
   
    public static MethodInfo IL_CanStack = typeof(ItemLoader).GetMethod(nameof(ItemLoader.CanStack));
    
    public static MethodInfo CheckPotion = typeof(TooltipUI).GetMethod(nameof(TooltipUI.CheckPotion));
    
    public override void PostSetupContent()
    {
        LoadCanStack();
    }

    private void LoadCanStack()
    {
        try
        {
            MonoModHooks.Modify(IL_CanStack,CanStack);
        }
        catch (Exception e)
        {
            Mod.Logger.Error(e.ToString());
        }
    }

    private void CanStack(ILContext context)
    {
        ILCursor il= new ILCursor(context);

        if (!il.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdarg0()
            ))
        {
            Mod.Logger.Error("Could not find.");
            return;
        }

        il.EmitLdarg0();
        il.EmitCall(AsPotion);
        il.EmitLdarg1();
        il.EmitCall(AsPotion);
        il.EmitCall(CheckPotion);
        il.EmitStloc0();
        il.EmitLdloc0();
        il.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[il.Index]);
        il.EmitLdcI4(1);
        il.EmitRet();

    }
}
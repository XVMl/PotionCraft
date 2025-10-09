using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.PotionILHook;
/// <summary>
/// 这个钩子用于修改光标浮于物品上方时物品的名称文本
/// </summary>
internal class DrawMouseOverHook : ModSystem
{
    public override void PostSetupContent()
    {
        LoadDrawMouseOver();
    }

    private static void LoadDrawMouseOver()
    {
        new ManagedILEdit("DrawMouseOver Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_Main.DrawMouseOver += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_Main.DrawMouseOver -= edit.SubscriptionWrapper;
        }, DrawMouseOver).Apply();
    }

    private static void DrawMouseOver(ILContext context, ManagedILEdit edit)
    {
        FieldInfo stack = typeof(Item).GetField(nameof(Item.stack));
        
        FieldInfo type = typeof(Item).GetField(nameof(Item.type));

        FieldInfo potionname = typeof(BasePotion).GetField(nameof(BasePotion.PotionName));

        MethodInfo AsPotion = typeof(AutoUIState).GetMethod(nameof(AutoUIState.AsPotion));

        FieldInfo item = typeof(Main).GetField(nameof(Main.item));

        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdsfld(item),
                i => i.MatchLdloc1(),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(stack),
                i => i.MatchLdcI4(1)
                ))
        {
            edit.LogFailure("Find T ERROR!");
            return;
        }
        cursor.EmitLdsfld(item);
        cursor.EmitLdloc1();
        cursor.EmitLdelemRef();
        cursor.EmitLdfld(type);
        cursor.EmitDelegate(ModContent.ItemType<BasePotion>);
        cursor.EmitCeq();
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        cursor.EmitLdsfld(item);
        cursor.EmitLdloc1();
        cursor.EmitLdelemRef();
        cursor.EmitCall(AsPotion);
        cursor.EmitLdfld(potionname);
        cursor.EmitStloc(5);
    }
}


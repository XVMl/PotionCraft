using FullSerializer.Internal;
using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.PotionILHook;
public class NewTextHook:ModSystem
{
    public override void PostSetupContent()
    {
        LoadNewText();
    }

    private static void LoadNewText()
    {
        new ManagedILEdit("NewText Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_PopupText.NewText_PopupTextContext_Item_int_bool_bool += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_PopupText.NewText_PopupTextContext_Item_int_bool_bool -= edit.SubscriptionWrapper;
        }, NewText).Apply();
    }

    private static void NewText(ILContext context, ManagedILEdit edit)
    {
       
        FieldInfo stack = typeof(PopupText).GetField(nameof(PopupText.stack));

        FieldInfo name = typeof(PopupText).GetField(nameof(PopupText.name));

        MethodInfo ModItem = typeof(Item).GetMethod("get_ModItem")!;

        TypeInfo BasePotion = typeof(BasePotion).GetTypeInfo();

        MethodInfo AsPotion = typeof(AutoUIState).GetMethod(nameof(AutoUIState.AsPotion));

        FieldInfo PotionName = typeof(BasePotion).GetField(nameof(Items.BasePotion.PotionName));

        MethodInfo DeleteTextColor = typeof(LanguageHelper).GetMethod(nameof(LanguageHelper.DeleteTextColor));

        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdloc(11),
                i => i.MatchLdarg2(),
                i => i.MatchConvI8(),
                i => i.MatchStfld(stack),
                i => i.MatchLdloc(11)
                ))
        {
            edit.LogFailure("Find NewText ERROR!");
            return;
        }

        cursor.EmitLdarg1();
        cursor.EmitCallvirt(ModItem);
        cursor.EmitIsinst(BasePotion);
        cursor.EmitLdnull();
        cursor.EmitCgtUn();
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        cursor.EmitLdloc(11);
        cursor.EmitLdarg1();
        cursor.EmitCall(AsPotion);
        cursor.EmitLdfld(PotionName);
        cursor.EmitCall(DeleteTextColor);
        cursor.EmitStfld(name);

    }

}

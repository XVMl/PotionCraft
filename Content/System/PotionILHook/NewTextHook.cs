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
        FieldInfo MouseText = typeof(FontAssets).GetField(nameof(FontAssets.MouseText))!;

        MethodInfo get_Value = typeof(ReLogic.Content.Asset<>).MakeGenericType(typeof(DynamicSpriteFont)).GetProperty("Value").GetGetMethod();

        FieldInfo stack = typeof(PopupText).GetField(nameof(PopupText.stack));

        FieldInfo name = typeof(PopupText).GetField(nameof(PopupText.name));

        MethodInfo ModItem = typeof(Item).GetMethod("get_ModItem")!;

        TypeInfo BasePotion = typeof(BasePotion).GetTypeInfo();

        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i =>i.MatchLdloc(11),
                i =>i.MatchLdarg2(),
                i =>i.MatchConvI8(),
                i => i.MatchStfld(stack)
                //i => i.MatchLdsfld(MouseText),
                //i => i.MatchCallvirt(get_Value),
                //i => i.MatchLdloc2()
                ))
        {
            edit.LogFailure("Find NewText ERROR!");
            return;
        }
        //cursor.EmitLdarg1();
        //cursor.EmitCallvirt(ModItem);
        //cursor.EmitIsinst(BasePotion);
        //cursor.EmitLdnull();
        //cursor.EmitCgtUn();
        //cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        //cursor.EmitDelegate(() =>
        //{
        //    return Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == ModContent.ItemType<BasePotion>();
        //});
        //cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        //cursor.EmitDelegate(() =>
        //{
        //    return AutoUIState.AsPotion(Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem]).HotbarPotionName;
        //});
        cursor.EmitDelegate(() =>
        {
            return "AAA";
        });
        cursor.EmitLdloc(11);
        cursor.EmitStfld(name);
        cursor.EmitDelegate(() =>
        {
            Main.NewText("!!!");
        });

    }

}

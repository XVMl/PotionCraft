using Luminance.Core.Hooking;
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

namespace PotionCraft.Content.System.PotionILHook;
public class GetItemHook:ModSystem
{
    public override void PostSetupContent()
    {
        LoadGetItem();
    }

    private static void LoadGetItem()
    {
        new ManagedILEdit("GetItem Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_Player.GetItem += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_Player.GetItem -= edit.SubscriptionWrapper;
        }, GetItem).Apply();
    }

    private static void GetItem(ILContext context, ManagedILEdit edit)
    {
        FieldInfo LongText = typeof(GetItemSettings).GetField(nameof(GetItemSettings.LongText))!;
        FieldInfo CanGoIntoVoidVault = typeof(GetItemSettings).GetField(nameof(GetItemSettings.CanGoIntoVoidVault))!;
        FieldInfo StepAfterHandlingSlotNormally = typeof(GetItemSettings).GetField(nameof(GetItemSettings.StepAfterHandlingSlotNormally))!;

        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdarg2()
                ))
        {
            edit.LogFailure("Find GetItem ERROR!");
            return;
        }
        cursor.EmitLdarg3();
        cursor.EmitLdfld(LongText);
        cursor.EmitLdcI4(0);
        cursor.EmitLdarg3();
        cursor.EmitLdfld(CanGoIntoVoidVault);
        cursor.EmitLdarg3();
        cursor.EmitLdfld(StepAfterHandlingSlotNormally);
        cursor.EmitStloc0();
    }

}


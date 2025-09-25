using Luminance.Core.Hooking;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using System.Numerics;
using PotionCraft.Content.System.ThiuDialogue;
using Terraria.GameContent;
using PotionCraft.Content.UI.CraftUI;
using PotionCraft.Content.Items;

namespace PotionCraft.Content.System.PotionILHook;
/// <summary>
/// 这个钩子用于修改物品栏上方的当前选择物品的名称文本
/// </summary>
public class GUIHotbarDrawInnerHook : ModSystem
{
    public override void PostSetupContent()
    {
        LoadGUIHotbarDrawInner();
    }

    private static void LoadGUIHotbarDrawInner()
    {
        new ManagedILEdit("GUIHotbarDrawInner Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_Main.GUIHotbarDrawInner += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_Main.GUIHotbarDrawInner -= edit.SubscriptionWrapper;
        }, GUIHotbarDrawInner).Apply();
    }

    private static void GUIHotbarDrawInner(ILContext context, ManagedILEdit edit)
    {
        FieldInfo MouseText = typeof(FontAssets).GetField(nameof(FontAssets.MouseText))!;

        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdsfld(PotionTooltipHook.SpriteBatch),
                i => i.MatchLdsfld(MouseText)
                ))
        {
            edit.LogFailure("Find GUIHotbarDrawInner ERROR!");
            return;
        }
        cursor.EmitDelegate(() =>
        {
            return Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == ModContent.ItemType<BasePotion>();
        });
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        cursor.EmitDelegate(() =>
        {
            return AutoUIState.AsPotion(Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem]).HotbarPotionName;
        });
        cursor.EmitStloc0();

    }
}



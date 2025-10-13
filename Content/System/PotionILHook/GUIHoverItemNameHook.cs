using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.PotionILHook;
/// <summary>
/// 此部分用于修改在未打开物品栏时光标悬浮在物品上时的文本
/// </summary>
public partial class GUIHotbarDrawInnerHook:ModSystem
{
    private static partial void GUIHoverItemName(ILContext context, ManagedILEdit edit) {
        
        FieldInfo player = typeof(Main).GetField(nameof(Main.player));

        FieldInfo myplayer = typeof(Main).GetField(nameof(Main.myPlayer));

        FieldInfo inventory = typeof(Player).GetField(nameof(Player.inventory));

        FieldInfo stack = typeof(Item).GetField(nameof(Item.stack));

        FieldInfo hoveritem = typeof(Main).GetField(nameof(Main.hoverItemName));

        FieldInfo type = typeof(Item).GetField(nameof(Item.type));

        MethodInfo potionname = typeof(BasePotion).GetProperty(nameof(BasePotion.PotionName)).GetGetMethod()!;

        MethodInfo AsPotion = typeof(AutoUIState).GetMethod(nameof(AutoUIState.AsPotion));

        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdsfld(player),
                i => i.MatchLdsfld(myplayer),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(inventory),
                i => i.MatchLdloc(5),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(stack),
                i => i.MatchLdcI4(1)
                ))
        {
            edit.LogFailure("Find HoverItemName ERROR!");
            return;
        }

        cursor.EmitLdsfld(player);
        cursor.EmitLdsfld(myplayer);
        cursor.EmitLdelemRef();
        cursor.EmitLdfld(inventory);
        cursor.EmitLdloc(5);
        cursor.EmitLdelemRef();
        cursor.EmitLdfld(type);
        cursor.EmitDelegate(ModContent.ItemType<BasePotion>);
        cursor.EmitCeq();
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        cursor.EmitLdsfld(player);
        cursor.EmitLdsfld(myplayer);
        cursor.EmitLdelemRef();
        cursor.EmitLdfld(inventory);
        cursor.EmitLdloc(5);
        cursor.EmitLdelemRef();
        cursor.EmitCall(AsPotion);
        cursor.EmitCall(potionname);
        cursor.EmitStsfld(hoveritem);

    }

}

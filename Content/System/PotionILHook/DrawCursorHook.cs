using System.Reflection;
using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using Terraria;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.PotionILHook;

public class DrawCursorHook:ModSystem
{
    public override void PostSetupContent()
    {
        LoadDrawCursor();
    }

    static readonly FieldInfo player = typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).GetField(nameof(Main.player))!;
        
    static readonly FieldInfo myplayer = typeof(Main).GetField(nameof(Main.myPlayer))!;

    private static void LoadDrawCursor()
    {
        new ManagedILEdit("Hide Chat Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_Main.DrawInterface_36_Cursor += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_Main.DrawInterface_36_Cursor -= edit.SubscriptionWrapper;
        },DrawCursor).Apply();
    }

    private static void DrawCursor(ILContext context, ManagedILEdit edit)
    {
        ILCursor cursor = new ILCursor(context);
        if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdsfld(player),
                i => i.MatchLdsfld(myplayer)))
        {
            edit.LogFailure("Find ERROR!");
            return;
        }

        cursor.EmitDelegate(() => Main.HoverItem.type.Equals(ModContent.ItemType<BasePotion>()));
        
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        cursor.EmitRet();
        
    }
    
}
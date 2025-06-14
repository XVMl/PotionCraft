using Luminance.Core.Hooking;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using System.Reflection;
using PotionCraft.Content.NPCs;
namespace PotionCraft.Content.System.ThiuDialogue
{
    public class HideChatHook:ModSystem
    {
        public override void PostSetupContent()
        {
            LoadHideChatUI();
        }

        static readonly FieldInfo player = typeof(Main).GetField(nameof(Main.player))!;
        
        static readonly FieldInfo myplayer = typeof(Main).GetField(nameof(Main.myPlayer))!;

        private static void LoadHideChatUI()
        {
            new ManagedILEdit("Hide Chat Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
            {
                IL_Main.GUIChatDrawInner += edit.SubscriptionWrapper;
            }, edit =>
            {
                IL_Main.GUIChatDrawInner -= edit.SubscriptionWrapper;
            },HideChatUI ).Apply();
        }

        private static void HideChatUI(ILContext context, ManagedILEdit edit) 
        {
            ILCursor cursor = new ILCursor(context);
            if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchLdsfld(player),
                i => i.MatchLdsfld(myplayer)))
            {
                edit.LogFailure("Find ERROR!");
                return;
            }
            cursor.EmitDelegate(() =>
            {
                if (Main.LocalPlayer.talkNPC > 0)
                {
                    return Main.npc[Main.LocalPlayer.talkNPC].type == ModContent.NPCType<Thou>() ? 1 : 0;
                }
                return 0;
            });
            cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
            cursor.EmitRet();

        }

    }
}

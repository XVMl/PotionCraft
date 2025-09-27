using Luminance.Core.Hooking;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using rail;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace PotionCraft.Content.System.PotionILHook
{
    internal class THook:ModSystem
    {
        public override void PostSetupContent()
        {
            LoadT();
        }

        private static void LoadT()
        {
            new ManagedILEdit("T Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
            {
                IL_Main.GUIHotbarDrawInner += edit.SubscriptionWrapper;
            }, edit =>
            {
                IL_Main.GUIHotbarDrawInner -= edit.SubscriptionWrapper;
            }, T).Apply();
        }

        private static void T(ILContext context, ManagedILEdit edit)
        {
            FieldInfo player = typeof(Main).GetField(nameof(Main.player));

            FieldInfo myplayer = typeof(Main).GetField(nameof(Main.myPlayer));

            FieldInfo inventory = typeof(Player).GetField(nameof(Player.inventory));

            FieldInfo stack = typeof(Item).GetField(nameof(Item.stack));

            FieldInfo hoveritem = typeof(Main).GetField(nameof(Main.hoverItemName));

            FieldInfo type = typeof(Item).GetField(nameof(Item.type));

            FieldInfo potionname = typeof(BasePotion).GetField(nameof(BasePotion.PotionName));
            
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
                edit.LogFailure("Find T ERROR!");
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
            cursor.EmitLdfld(potionname);
            cursor.EmitStsfld(hoveritem);

        }

    }
}

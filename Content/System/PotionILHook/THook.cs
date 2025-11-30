using Luminance.Core.Hooking;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using PotionCraft.Content.Items;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;

namespace PotionCraft.Content.System.PotionILHook
{
    internal class THook:ModSystem
    {
        public override void PostSetupContent()
        {
            //LoadT();
        }

        static readonly FieldInfo player = typeof(Main).GetField(nameof(Main.player))!;

        static readonly FieldInfo myplayer = typeof(Main).GetField(nameof(Main.myPlayer))!;

        private void LoadT()
        {
            MethodInfo IL_HeldItem = typeof(Player).GetProperty("HeldItem").GetGetMethod();

            try
            {
                MonoModHooks.Modify(IL_HeldItem, T);
                Mod.Logger.Error("Load T Successs .");
            }
            catch (Exception e)
            {
                Mod.Logger.Error(e.ToString());
            }
        }

        private void T(ILContext context)
        {
            var item = typeof(TextureAssets).GetField(nameof(TextureAssets.Item));

            var get_value = typeof(Asset<Texture2D>).GetMethod("get_Value", BindingFlags.Instance | BindingFlags.Public)!;

            var AsPotion = typeof(AutoUIState).GetMethod(nameof(AutoUIState.AsPotion));

            var icon = typeof(BasePotion).GetField(nameof(BasePotion.IconID));

            //var held = typeof(PlayerDrawSet).GetField(nameof(PlayerDrawSet.T));

            var inventory = typeof(Player).GetField(nameof(Player.inventory));
            
            var selectedItem = typeof(Player).GetField(nameof(Player.selectedItem));

            var type = typeof(Item).GetField(nameof(Item.type));

            ILCursor cursor = new ILCursor(context);
            if (!cursor.TryGotoNext(MoveType.AfterLabel,
                i => i.MatchRet()
            ))
            {
                Mod.Logger.Error("Could not find.");
                return;
            }

            //cursor.EmitDelegate(() =>
            //{
            //    Main.NewText("!!!!");
            //});

            //cursor.EmitLdarg0();
            //cursor.EmitLdfld(inventory);
            //cursor.EmitLdarg0();
            //cursor.EmitLdfld(selectedItem);
            //cursor.EmitLdelemRef();
            //cursor.EmitLdfld(type);
            //cursor.EmitDelegate(ModContent.ItemType<BasePotion>);
            //cursor.EmitCeq();
            //cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
            //cursor.EmitCall(AsPotion);
            //cursor.EmitLdfld(icon);
            //cursor.EmitDelegate((int t) =>
            //{
            //    var item = new Item();
            //    item.SetDefaults(t);
            //    return item;
            //});

        }
    }
}

using FullSerializer.Internal;
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
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.PotionILHook;
internal class HeldItemHook:ModSystem
{
    public override void PostSetupContent()
    {
        LoadHeldItem();
    }

    static readonly FieldInfo player = typeof(Main).GetField(nameof(Main.player))!;

    static readonly FieldInfo myplayer = typeof(Main).GetField(nameof(Main.myPlayer))!;

    private static void LoadHeldItem()
    {
        new ManagedILEdit("Hide Chat Hook ERROR!", ModContent.GetInstance<PotionCraft>(), edit =>
        {
            IL_PlayerDrawLayers.DrawPlayer_27_HeldItem += edit.SubscriptionWrapper;
        }, edit =>
        {
            IL_PlayerDrawLayers.DrawPlayer_27_HeldItem -= edit.SubscriptionWrapper;
        }, HeldItem).Apply();
    }

    private void FUN(ref PlayerDrawSet drawSet)
    {
        if(drawSet.heldItem.type == ModContent.ItemType<BasePotion>())
        {
            var item = new Item();
            item.SetDefaults(AutoUIState.AsPotion(drawSet.heldItem).IconID);
            drawSet.heldItem =  item;
        }
        Item heldItem = drawSet.heldItem;
        
    }

    private static void HeldItem(ILContext context, ManagedILEdit edit)
    {
        var item = typeof(TextureAssets).GetField(nameof(TextureAssets.Item));

        var get_value = typeof(Asset<Texture2D>).GetMethod("get_Value",BindingFlags.Instance | BindingFlags.Public)!;
        
        var AsPotion = typeof(AutoUIState).GetMethod(nameof(AutoUIState.AsPotion));

        var icon = typeof(BasePotion).GetField(nameof(BasePotion.IconID));

        var held = typeof(PlayerDrawSet).GetField(nameof(PlayerDrawSet.heldItem));

        var type = typeof(Item).GetField(nameof(Item.type));

        var drawplayer = typeof(PlayerDrawSet).GetField(nameof(PlayerDrawSet.drawPlayer));

        ILCursor cursor = new ILCursor(context);

        if (!cursor.TryGotoNext(MoveType.AfterLabel,
            i => i.MatchLdarg0(),
            i => i.MatchLdfld(drawplayer),
            i => i.MatchLdloc0()
            ))
        {
            edit.LogFailure("Find ERROR!");
            return;
        }

        cursor.EmitLdloc0();
        cursor.EmitLdfld(type);
        cursor.EmitDelegate(ModContent.ItemType<BasePotion>);
        cursor.EmitCeq();
        cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);


        cursor.EmitLdloc0();
        cursor.EmitDelegate((Item i) =>
        {
           return AutoUIState.AsPotion(i).IconID;
        });
        cursor.EmitStloc1();


        //if (!cursor.TryGotoNext(MoveType.AfterLabel,
        //    i => i.MatchLdarg0(),
        //    i => i.MatchLdfld(held)
        //    ))
        //{
        //    edit.LogFailure("Find ERROR!");
        //    return;
        //}

        //cursor.EmitLdarg0();
        //cursor.EmitLdfld(held);
        //cursor.EmitLdfld(type);
        //cursor.EmitDelegate(ModContent.ItemType<BasePotion>);
        //cursor.EmitCeq();
        //cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);

        //cursor.EmitLdarg0();
        //cursor.EmitLdarg0();
        //cursor.EmitLdfld(held);
        //cursor.EmitCall(AsPotion);
        //cursor.EmitLdfld(icon);
        //cursor.EmitDelegate((int t) =>
        //{
        //    var item = new Item();
        //    item.SetDefaults(t);
        //    return item;
        //});
        //cursor.EmitStfld(held);

        //cursor.EmitLdloc0();
        //cursor.EmitDelegate((Item item) =>
        //{
        //    return item.type == ModContent.ItemType<BasePotion>();
        //});
        //cursor.Emit(Mono.Cecil.Cil.OpCodes.Brfalse_S, context.Instrs[cursor.Index]);
        //cursor.EmitLdloc0();
        //cursor.EmitDelegate((Item item1) =>
        //{
        //    var potion = AutoUIState.AsPotion(item1);
        //    var icon = new Item();
        //    icon.SetDefaults(potion.IconID);
        //    return icon;
        //});
        //cursor.EmitStloc0();


    }
}


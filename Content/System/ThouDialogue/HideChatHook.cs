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
namespace PotionCraft.Content.System.ThiuDialogue
{
    public class HideChatHook:ModSystem
    {
        public override void PostSetupContent()
        {
            Loadprojectile();
        }
        private static void Loadprojectile()
        {
            new ManagedILEdit("The Potion`s tooltip background", ModContent.GetInstance<PotionCraft>(), edit =>
            {
                IL_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += edit.SubscriptionWrapper;
            }, edit =>
            {
                IL_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float -= edit.SubscriptionWrapper;
            }, NewProjectile).Apply();
        }

        private static void NewProjectile(ILContext context,ManagedILEdit edit)
        {
            ILCursor cursor = new ILCursor(context);
            if (!cursor.TryGotoNext(MoveType.AfterLabel,
                    i => i.MatchLdloc0(),
                    i => i.MatchRet()
                ))
            {
                edit.LogFailure("Could not find the NewProjectile call.");
                return;
            }
            cursor.EmitLdarg(5);
            cursor.EmitDelegate((int x) =>
            {
                Main.NewText(x);
            });
  
        }
    }
}

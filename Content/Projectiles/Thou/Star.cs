using Luminance.Assets;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Luminance.Common.Utilities.Utilities;
using static Microsoft.Xna.Framework.MathHelper;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PotionCraft.Content.Projectiles.Thou
{
    public class Star:ModProjectile
    {
        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

        public static Color LensFlareColor => new(255, 174, 147);

        private int Time;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 8000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.localNPCHitCooldown = 1;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            Time++;
        }

        public override bool PreDraw(ref Color lightColor)
        {

            float theMagicFactorThatMakesEveryElectricShineEffectSoMuchBetter = Cos01(Main.GlobalTimeWrappedHourly * 85f);
            float shineIntensity = InverseLerp(0f, 12f, Time) * InverseLerp(0f, 7f, Projectile.timeLeft) * Lerp(1f, 1.2f, theMagicFactorThatMakesEveryElectricShineEffectSoMuchBetter) * 0.45f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Texture2D glow = Assets.NPCs.BloomCircleSmall;
            Texture2D flare = MiscTexturesRegistry.ShineFlareTexture.Value;

            for (int i = 0; i < 3; i++)
                Main.spriteBatch.Draw(flare, drawPosition, null, Projectile.GetAlpha(LensFlareColor with { A = 0 }), 0f, flare.Size() * 0.5f, shineIntensity * 2f, 0, 0f);
            return false;
        }
    }
}

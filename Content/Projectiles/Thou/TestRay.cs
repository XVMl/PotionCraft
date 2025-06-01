using Luminance.Assets;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Luminance.Common.Utilities.Utilities;
using static Microsoft.Xna.Framework.MathHelper;
using static System.MathF;
namespace PotionCraft.Content.Projectiles.Thou
{
    public class TestRay:ModProjectile
    {
        public int SolynIndex => (int)Projectile.ai[0];

        /// <summary>
        /// The owner of this laserbeam.
        /// </summary>
        public Player Owner => Main.player[Projectile.owner];

        /// <summary>
        /// How long this laserbeam has existed, in frames.
        /// </summary>
        public ref float Time => ref Projectile.ai[1];

        /// <summary>
        /// How long this laserbeam currently is.
        /// </summary>
        public ref float LaserbeamLength => ref Projectile.ai[2];

        /// <summary>
        /// How long this laserbeam should exist for, in frames.
        /// </summary>
        public static int Lifetime => Utilities.SecondsToFrames(3.75f);

        /// <summary>
        /// The maximum length of this laserbeam.
        /// </summary>
        public static float MaxLaserbeamLength => 5600f;

        /// <summary>
        /// The color of the lens flare on this laserbeam.
        /// </summary>
        public static Color LensFlareColor => new(255, 174, 147);

        /// <summary>
        /// The speed at which this laserbeam aims towards the mouse.
        /// </summary>
        public static float MouseAimSpeedInterpolant => 1.01f;

        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

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
            Projectile.timeLeft = Lifetime;
            Projectile.localNPCHitCooldown = 1;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            

            AimTowardsMouse();

            NPC solyn = Main.npc[SolynIndex];
            Vector2 offsetBetweenOwners = Owner.Center - Owner.Center;
            Projectile.Center = Owner.Center + offsetBetweenOwners * 0.5f + Projectile.velocity * 100f;

            LaserbeamLength = Clamp(LaserbeamLength + 175f, 0f, MaxLaserbeamLength);

            ScreenShakeSystem.StartShake(Utilities.InverseLerp(0f, 20f, Time) * 2f);

            CreateOuterParticles();

            Time++;
        }

        /// <summary>
        /// Makes this beam slowly aim towards the user's mouse.
        /// </summary>
        public void AimTowardsMouse()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 oldVelocity = Projectile.velocity;
            float idealRotation = Projectile.AngleTo(Main.MouseWorld);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleLerp(idealRotation, MouseAimSpeedInterpolant).ToRotationVector2();

            if (Projectile.velocity != oldVelocity)
            {
                Projectile.netUpdate = true;
                Projectile.netSpam = 0;
            }
        }

        /// <summary>
        /// Creates particles along the deathray's outer boundaries.
        /// </summary>
        public void CreateOuterParticles()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 perpendicular = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2);
            for (int i = 0; i < 6; i++)
            {
                int arcLifetime = Main.rand.Next(6, 14);
                float energyLengthInterpolant = Main.rand.NextFloat();
                float perpendicularDirection = Main.rand.NextFromList(-1f, 1f);
                float arcReachInterpolant = Main.rand.NextFloat();
                Vector2 energySpawnPosition = Projectile.Center + Projectile.velocity * energyLengthInterpolant * LaserbeamLength + perpendicular * LaserWidthFunction(0.5f) * perpendicularDirection * 0.9f;
                Vector2 arcOffset = perpendicular.RotatedBy(1.04f) * Lerp(40f, 320f, Pow(arcReachInterpolant, 4f)) * perpendicularDirection;
                //Utilities.NewProjectileBetter(Projectile.GetSource_FromAI(), energySpawnPosition, arcOffset, ModContent.ProjectileType<SmallTeslaArc>(), 0, 0f, -1, arcLifetime, 1f);
            }
        }

        public float LaserWidthFunction(float completionRatio)
        {
            
            float initialBulge = Convert01To010(Utilities.InverseLerp(0.15f, 0.85f, LaserbeamLength / MaxLaserbeamLength)) * Utilities.InverseLerp(0f, 0.05f, completionRatio) * 32f;
            float idealWidth = initialBulge + Cos(Main.GlobalTimeWrappedHourly * 90f) * 6f + Projectile.width;
            float closureInterpolant = Utilities.InverseLerp(0f, 8f, Lifetime - Time);

            float circularStartInterpolant = Utilities.InverseLerp(0.05f, 0.012f, completionRatio);
            float circularStart = Sqrt(1.001f - circularStartInterpolant.Squared());

            return Utils.Remap(LaserbeamLength, 0f, MaxLaserbeamLength, 4f, idealWidth) * closureInterpolant * circularStart;
        }

        public float BloomWidthFunction(float completionRatio) => LaserWidthFunction(completionRatio) * 1.9f;

        public Color LaserColorFunction(float completionRatio)
        {
            float lengthOpacity = Utilities.InverseLerp(0f, 0.45f, LaserbeamLength / MaxLaserbeamLength);
            float startOpacity = Utilities.InverseLerp(0f, 0.032f, completionRatio);
            float endOpacity = Utilities.InverseLerp(0.95f, 0.81f, completionRatio);
            float opacity = lengthOpacity * startOpacity * endOpacity;
            Color startingColor = Projectile.GetAlpha(new(255, 45, 123));
            return startingColor * opacity;
        }

        public static Color BloomColorFunction(float completionRatio) => new Color(255, 10, 150) * Utilities.InverseLerpBump(0.02f, 0.05f, 0.81f, 0.95f, completionRatio) * 0.34f;

        public override bool PreDraw(ref Color lightColor)
        {
            float theMagicFactorThatMakesEveryElectricShineEffectSoMuchBetter = Utilities.Cos01(Main.GlobalTimeWrappedHourly * 85f);

            List<Vector2> laserPositions = Projectile.GetLaserControlPoints(12, LaserbeamLength);
            laserPositions[0] -= Projectile.velocity * 10f;

            // Draw bloom.
            ManagedShader shader = ShaderManager.GetShader("PotionCraft.PrimitiveBloomShader");
            shader.TrySetParameter("innerGlowIntensity", 0.45f);
            PrimitiveSettings bloomSettings = new PrimitiveSettings(BloomWidthFunction, BloomColorFunction, Shader: shader, UseUnscaledMatrix: true);
            PrimitiveRenderer.RenderTrail(laserPositions, bloomSettings, 46);

            // Draw the beam.
            ManagedShader deathrayShader = ShaderManager.GetShader("PotionCraft.SolynTagTeamBeamShader");
            deathrayShader.TrySetParameter("secondaryColor", new Color(255, 196, 36).ToVector4());
            deathrayShader.TrySetParameter("lensFlareColor", LensFlareColor.ToVector4());
            deathrayShader.SetTexture(Assets.NPCs.DendriticNoiseZoomedOut, 1, SamplerState.LinearWrap);

            PrimitiveSettings laserSettings = new PrimitiveSettings(LaserWidthFunction, LaserColorFunction, Shader: deathrayShader, UseUnscaledMatrix: true);
            PrimitiveRenderer.RenderTrail(laserPositions, laserSettings, 75);

            // Draw a superheated lens flare and bloom instance at the center of the beam.
            float shineIntensity = Utilities.InverseLerp(0f, 12f, Time) * Utilities.InverseLerp(0f, 7f, Projectile.timeLeft) * Lerp(1f, 1.2f, theMagicFactorThatMakesEveryElectricShineEffectSoMuchBetter) * 0.45f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Texture2D glow =Assets.NPCs.BloomCircleSmall;
            Texture2D flare = MiscTexturesRegistry.ShineFlareTexture.Value;

            for (int i = 0; i < 3; i++)
                Main.spriteBatch.Draw(flare, drawPosition, null, Projectile.GetAlpha(LensFlareColor with { A = 0 }), 0f, flare.Size() * 0.5f, shineIntensity * 2f, 0, 0f);
            for (int i = 0; i < 2; i++)
                Main.spriteBatch.Draw(glow, drawPosition, null, Projectile.GetAlpha(LensFlareColor with { A = 0 }), 0f, glow.Size() * 0.5f, shineIntensity * 2f, 0, 0f);

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0f;
            float laserWidth = LaserWidthFunction(0.25f) * 1.8f;
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity.SafeNormalize(Vector2.Zero) * LaserbeamLength * 0.95f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, laserWidth, ref _);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //if (target.ModNPC is MarsBody mars)
            //    mars.RegisterHitByTeamBeam(Projectile);
        }

        public override bool ShouldUpdatePosition() => false;

    }
}

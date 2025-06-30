using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Luminance;
using Luminance.Assets;
using Terraria.ID;
using Terraria;
using static Luminance.Common.Utilities.Utilities;
using static Microsoft.Xna.Framework.MathHelper;
using Microsoft.Xna.Framework;
namespace PotionCraft.Content.Projectiles.Thiu
{
    public class EnergyTrial : ModProjectile, IPixelatedPrimitiveRenderer
    {

        public static int Lifetime => SecondsToFrames(3f);

        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 100;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
        }

        public override void SetDefaults()
        {
            Projectile.width = Main.rand?.Next(36, 100) ?? 100;
            Projectile.height = Projectile.width;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.hide = true;
            Projectile.timeLeft = Lifetime;
            Projectile.Opacity = 0f;

            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public float EnergyWidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.width;
            return baseWidth * Projectile.scale;
        }

        public Color EnergyColorFunction(float completionRatio)
        {
            Color energyColor = Color.Lerp(Color.PaleTurquoise, Color.Cyan, Projectile.identity / 10f % 1f);
            energyColor.A = 0;

            return energyColor * InverseLerpBump(0f, 0.4f, 0.6f, 0.9f, completionRatio) * Projectile.Opacity * Convert01To010(completionRatio);
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader trailShader = ShaderManager.GetShader("PotionCraft.EnergyTrial");
            //trailShader.SetTexture(PerlinNoise.Value, 1, SamplerState.LinearWrap);
            trailShader.Apply();

            PrimitiveSettings settings = new(EnergyWidthFunction, EnergyColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: trailShader);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings,45);
        }
    }
}

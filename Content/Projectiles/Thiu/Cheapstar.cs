using Luminance.Assets;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace PotionCraft.Content.Projectiles.Thiu
{
    internal class Cheapstar:ModProjectile
    {
        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

        public static Color LensFlareColor => new(255, 174, 147);

        private int TimeLeft = 60;

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
            Projectile.timeLeft = TimeLeft * 999;
            Projectile.localNPCHitCooldown = 1;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hide = false;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D laser = Assets.UI.BackGround;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null);

            var starshader = ShaderManager.GetShader("PotionCraft.Cheapstar");
            starshader.TrySetParameter("time", (float)(Projectile.timeLeft / TimeLeft));
            starshader.SetTexture(Assets.NPCs.LaserChannel, 1, SamplerState.LinearWrap);
            starshader.Apply();

            Vector2 pos = Projectile.Center - Main.screenPosition;
            Rectangle rectangle = new Rectangle((int)pos.X, (int)pos.Y, 180, 180);

            //var shader = ShaderManager.GetShader("PotionCraft.Color");
            //shader.Apply();

            Main.spriteBatch.Draw(laser, rectangle, null, Color.White * 1, 0, laser.Size() / 2, 0, 0);

            return false;
        }
    }
}

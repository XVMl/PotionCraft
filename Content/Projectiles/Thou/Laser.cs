using Luminance.Assets;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Luminance.Common.Utilities.Utilities;
using static Microsoft.Xna.Framework.MathHelper;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Luminance.Core.Graphics;


namespace PotionCraft.Content.Projectiles.Thou
{
    public class Laser : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Luminance/Assets/InvisiblePixel";
            }
        }
        public static Color LensFlareColor => new(255, 174, 147);

        private int Time;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 8000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1000;
            Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 99000;
            Projectile.localNPCHitCooldown = 1;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hide = false;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            Time++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Texture2D laser = Assets.NPCs.LaserChannel;
            Texture2D laser = Assets.UI.BackGround;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null);

            var lasershader = ShaderManager.GetShader("PotionCraft.Laser");
            lasershader.SetTexture(Assets.NPCs.LaserChannel, 1, SamplerState.LinearWrap);
            lasershader.Apply();
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Rectangle rectangle = new Rectangle((int)pos.X, (int)pos.Y, 1500, 500);

            Main.spriteBatch.Draw(laser, rectangle, null, Color.White * 1, 0, laser.Size() / 2, 0, 0);

            return false;
        }
    }
}

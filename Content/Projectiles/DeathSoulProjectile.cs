using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Projectiles
{
    public class DeathSoulProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // 告诉游戏这张贴图有 6 帧
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;

            // 不碰地形，才能飘
            Projectile.tileCollide = false;

            // 关掉原版 AI，用自己的
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            // --- 帧动画 ---
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            // --- 飘浮逻辑 ---
            Projectile.velocity *= 0.98f;
            Projectile.velocity.Y += 0.05f;
            Projectile.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);

            // 发光粒子
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Pixie, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            }
        }
    }
}
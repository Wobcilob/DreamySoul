using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DreamySoul.Content.Projectiles
{
    public class 合金短刀刺击 : ModProjectile
    {
        // PNG 的 (1, 30) 是刀把与人物中心的对齐点，刀身指向 (30, 1)。
        private static readonly Vector2 TextureHandle = new(1f, 30f);
        private static readonly Vector2 TextureTip = new(30f, 1f);

        private Vector2 handlePosition;
        private Vector2 stabDirection;

        public override string Texture => "DreamySoul/Content/Items/合金短刀";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        // 弹幕始终由玩家中心定位，不让引擎按 velocity 移动它。
        public override bool ShouldUpdatePosition() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // 第一次更新时记录完整攻击时长，之后由弹幕自身计时，确保收回阶段不会被提前截断。
            if (Projectile.ai[1] <= 0f)
                Projectile.ai[1] = System.Math.Max(1, player.HeldItem.useAnimation);

            float animationLength = Projectile.ai[1];
            float progress = MathHelper.Clamp(Projectile.ai[0] / animationLength, 0f, 1f);

            stabDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX * player.direction);
            player.ChangeDir(stabDirection.X >= 0f ? 1 : -1);
            player.heldProj = Projectile.whoAmI;

            // 正弦曲线保证起点和终点都在人物中心，中途达到最大刺出距离。
            float extension = (float)System.Math.Sin(progress * MathHelper.Pi) * 12f;
            handlePosition = player.MountedCenter + stabDirection * extension;

            float bladeLength = Vector2.Distance(TextureHandle, TextureTip) * Projectile.scale;
            Projectile.Center = handlePosition + stabDirection * bladeLength * 0.5f;
            // 贴图刀身从 (1,30) 指向 (30,1)，自身角度为 -45°；补偿 +45° 后对准刺击方向。
            Projectile.rotation = stabDirection.ToRotation() + MathHelper.PiOver4;
            Projectile.timeLeft = 2;

            Projectile.ai[0]++;
            if (Projectile.ai[0] > animationLength)
                Projectile.Kill();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float bladeLength = Vector2.Distance(TextureHandle, TextureTip) * Projectile.scale;
            Vector2 bladeTip = handlePosition + stabDirection * bladeLength;
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                handlePosition,
                bladeTip,
                4f * Projectile.scale,
                ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Main.EntitySpriteDraw(
                texture,
                handlePosition - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                TextureHandle,
                Projectile.scale,
                SpriteEffects.None);

            return false;
        }
    }
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Projectiles
{
    public class 硅孔雀石钩爪弹幕 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookDiamond);
            AIType = ProjectileID.GemHookDiamond;
        }

        public override bool? CanUseGrapple(Player player)
        {
            return player.ownedProjectileCounts[Type] < 1;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        public override float GrappleRange()
        {
            return 466f;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 12.5f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 12.5f;
        }

        public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
        {
            Vector2 offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            grappleX += offset.X;
            grappleY += offset.Y;
        }
    }
}

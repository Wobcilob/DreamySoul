using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Projectiles
{
    public class 硅孔雀石法杖弹幕 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DiamondBolt);
            AIType = ProjectileID.DiamondBolt;
        }
    }
}

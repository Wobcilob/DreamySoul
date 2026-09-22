using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Projectiles
{
    public class 合金短刀刺击 : ModProjectile
    {
        // 刺击时继续使用合金短刀本身的贴图。
        public override string Texture => "DreamySoul/Content/Items/合金短刀";

        public override void SetDefaults()
        {
            // 复制原版铜短剑弹幕的尺寸、碰撞与刺击行为。
            Projectile.CloneDefaults(ProjectileID.CopperShortswordStab);
            AIType = ProjectileID.CopperShortswordStab;
            Projectile.DamageType = DamageClass.Melee;
        }
    }
}

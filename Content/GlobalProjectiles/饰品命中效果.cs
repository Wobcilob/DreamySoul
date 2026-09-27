using DreamySoul.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalProjectiles
{
    public class 饰品命中效果 : GlobalProjectile
    {
        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (!projectile.friendly || projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player player = Main.player[projectile.owner];
            if (player.active && player.GetModPlayer<饰品效果玩家>().启用暗影鲨牙项链)
                target.AddBuff(BuffID.ShadowFlame, 180);
        }
    }
}

using DreamySoul.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalItems
{
    public class 饰品命中效果 : GlobalItem
    {
        public override void OnHitNPC(
            Item item,
            Player player,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (player.GetModPlayer<饰品效果玩家>().启用暗影鲨牙项链)
                target.AddBuff(BuffID.ShadowFlame, 180);
        }
    }
}

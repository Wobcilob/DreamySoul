using DreamySoul.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace DreamySoul.Content.Buffs
{
    public class 结晶再生 : ModBuff
    {
        public override string Texture => "DreamySoul/Content/Items/复生冰晶";

        public override void Update(Player player, ref int buffIndex)
        {
            // lifeRegen 以半点生命/秒计数，因此 +18 等于每秒恢复 9 点生命。
            player.lifeRegen += 18;
            player.statDefense += 10;

            饰品效果玩家 effects = player.GetModPlayer<饰品效果玩家>();
            effects.结晶再生激活 = true;
            effects.元素伤害承受倍率 *= 0.5f;
        }
    }
}

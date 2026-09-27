using Terraria;
using Terraria.ModLoader;

namespace DreamySoul.Content.Buffs
{
    public class 光合作用 : ModBuff
    {
        public override string Texture => "DreamySoul/Content/Items/叶绿精华";

        public override void Update(Player player, ref int buffIndex)
        {
            // lifeRegen 以半点生命/秒计数，因此 +10 等于每秒恢复 5 点生命。
            player.lifeRegen += 10;
        }
    }
}

using DreamySoul.Content.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalItems
{
    public class 再生徽章宝匣掉落 : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FloatingIslandFishingCrate
                || item.type == ItemID.FloatingIslandFishingCrateHard)
            {
                // 作为额外掉落独立判定，不替换宝匣原有战利品。
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<再生徽章>(), 5));
            }
        }
    }
}

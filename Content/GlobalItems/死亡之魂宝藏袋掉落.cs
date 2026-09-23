using DreamySoul.Content.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalItems
{
    public class 死亡之魂宝藏袋掉落 : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.PlanteraBossBag)
            {
                // 专家和大师模式下，每位玩家从自己的世纪之花宝藏袋中获得一份。
                itemLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<死亡之魂>(),
                    chanceDenominator: 1,
                    minimumDropped: 15,
                    maximumDropped: 20));
                itemLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<叶绿精华>(),
                    chanceDenominator: 1,
                    minimumDropped: 3,
                    maximumDropped: 5));
            }
        }
    }
}

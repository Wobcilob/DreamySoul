using DreamySoul.Content.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalNPCs
{
    public class 死亡之魂掉落 : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Plantera)
            {
                // 普通模式由世纪之花本体掉落；专家和大师模式改由宝藏袋掉落。
                LeadingConditionRule normalMode = new(new Conditions.NotExpert());
                normalMode.OnSuccess(ItemDropRule.Common(
                    ModContent.ItemType<死亡之魂>(),
                    chanceDenominator: 1,
                    minimumDropped: 15,
                    maximumDropped: 20));
                npcLoot.Add(normalMode);
            }
        }
    }
}

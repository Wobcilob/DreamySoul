using DreamySoul.Content.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalNPCs
{
    public class 生物材料掉落 : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BloodNautilus)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<血晶>(),
                    chanceDenominator: 1,
                    minimumDropped: 1,
                    maximumDropped: 1));
            }

            if (IsBloodMoonFishingEnemy(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<血晶碎片>(),
                    chanceDenominator: 1,
                    minimumDropped: 1,
                    maximumDropped: 1));
            }

            if (npc.type == NPCID.FireImp)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<赤炎碎片>(),
                    chanceDenominator: 1,
                    minimumDropped: 1,
                    maximumDropped: 1));
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<改良型火焰喷管>(),
                    chanceDenominator: 1,
                    minimumDropped: 1,
                    maximumDropped: 1));
            }

            if (IsHornetOrSpider(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<毒腺>(),
                    chanceDenominator: 1,
                    minimumDropped: 1,
                    maximumDropped: 3));
            }

            if (npc.type == NPCID.WallCreeper || npc.type == NPCID.WallCreeperWall)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<外骨骼>(),
                    chanceDenominator: 1,
                    minimumDropped: 1,
                    maximumDropped: 3));
            }
        }

        private static bool IsHornetOrSpider(int npcType)
        {
            return npcType == NPCID.Hornet
                || npcType == NPCID.HornetFatty
                || npcType == NPCID.HornetHoney
                || npcType == NPCID.HornetLeafy
                || npcType == NPCID.HornetSpikey
                || npcType == NPCID.HornetStingy
                || npcType == NPCID.MossHornet
                || npcType == NPCID.WallCreeper
                || npcType == NPCID.WallCreeperWall
                || npcType == NPCID.JungleCreeper
                || npcType == NPCID.JungleCreeperWall
                || npcType == NPCID.BlackRecluse
                || npcType == NPCID.BlackRecluseWall;
        }

        private static bool IsBloodMoonFishingEnemy(int npcType)
        {
            // 恐惧鹦鹉螺（BloodNautilus）按要求排除；血鳗仅由头部结算掉落。
            return npcType == NPCID.ZombieMerman
                || npcType == NPCID.EyeballFlyingFish
                || npcType == NPCID.GoblinShark
                || npcType == NPCID.BloodEelHead;
        }
    }
}

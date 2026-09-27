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
    }
}

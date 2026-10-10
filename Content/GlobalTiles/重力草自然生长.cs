using DreamySoul.Content.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.GlobalTiles
{
    public class 重力草自然生长 : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && type == TileID.Grass
                && WorldGen.genRand.NextBool(100) && 重力草世界生成.IsSkyIslandGrass(i, j))
                重力草世界生成.TryGrow(i, j);
        }
    }
}

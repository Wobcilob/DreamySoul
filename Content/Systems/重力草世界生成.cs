using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Systems
{
    public class 重力草世界生成 : ModSystem
    {
        public override void PostWorldGen()
        {
            for (int x = 10; x < Main.maxTilesX - 10; x++)
            {
                for (int y = 10; y < (int)Main.worldSurface; y++)
                {
                    if (WorldGen.genRand.NextBool(10) && IsSkyIslandGrass(x, y))
                        TryGrow(x, y);
                }
            }
        }

        // 空岛草地下方有云块；不把普通地面或高空人工草平台当成空岛。
        internal static bool IsSkyIslandGrass(int x, int y)
        {
            if (!WorldGen.InWorld(x, y, 10) || y >= Main.worldSurface)
                return false;

            Tile grass = Main.tile[x, y];
            if (!grass.HasTile || grass.TileType != TileID.Grass
                || grass.IsActuated || grass.IsHalfBlock || grass.Slope != 0)
                return false;

            for (int cloudX = x - 8; cloudX <= x + 8; cloudX++)
            {
                for (int cloudY = y + 1; cloudY <= y + 30 && cloudY < Main.maxTilesY - 1; cloudY++)
                {
                    Tile cloud = Main.tile[cloudX, cloudY];
                    if (cloud.HasTile && (cloud.TileType == TileID.Cloud
                        || cloud.TileType == TileID.RainCloud || cloud.TileType == TileID.SnowCloud))
                        return true;
                }
            }

            return false;
        }

        internal static void TryGrow(int x, int y)
        {
            Tile target = Main.tile[x, y - 1];
            if (target.HasTile || target.LiquidAmount > 0)
                return;

            int herbType = ModContent.TileType<Tiles.重力草>();
            // 留出间距，避免旧世界逐渐长满整片空岛。
            for (int nearbyX = x - 3; nearbyX <= x + 3; nearbyX++)
            {
                Tile nearby = Main.tile[nearbyX, y - 1];
                if (nearby.HasTile && nearby.TileType == herbType)
                    return;
            }

            WorldGen.PlaceTile(x, y - 1, herbType, mute: true);
            if (Main.netMode == NetmodeID.Server && target.HasTile && target.TileType == herbType)
                NetMessage.SendTileSquare(-1, x, y - 1, 1);
        }
    }
}

using System.Collections.Generic;
using DreamySoul.Content.Tiles;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace DreamySoul.Content.Systems
{
    public class 硅孔雀石世界生成 : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int gemsIndex = tasks.FindIndex(pass => pass.Name == "Gems");
            int insertionIndex = gemsIndex >= 0 ? gemsIndex + 1 : tasks.Count;
            tasks.Insert(insertionIndex, new PassLegacy("硅孔雀石矿", GenerateSiliconMalachite));

            int gemCavesIndex = tasks.FindIndex(pass => pass.Name == "Gem Caves");
            int gemCavesInsertionIndex = gemCavesIndex >= 0 ? gemCavesIndex + 1 : tasks.Count;
            tasks.Insert(gemCavesInsertionIndex, new PassLegacy("宝石洞穴中的硅孔雀石", GenerateInGemCaves));
        }

        private static void GenerateSiliconMalachite(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "正在生成硅孔雀石矿";

            int tileType = ModContent.TileType<硅孔雀石矿>();
            double attempts = Main.maxTilesX * 0.01;

            for (int attempt = 0; attempt < attempts; attempt++)
            {
                int x;
                int y;

                do
                {
                    x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    y = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
                }
                while (Main.tile[x, y].TileType != TileID.Stone);

                WorldGen.TileRunner(
                    x,
                    y,
                    WorldGen.genRand.Next(2, 6),
                    WorldGen.genRand.Next(3, 7),
                    tileType,
                    false,
                    0.0,
                    0.0,
                    false,
                    true,
                    -1);
            }
        }

        private static void GenerateInGemCaves(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "正在向宝石洞穴中生成硅孔雀石";

            ushort tileType = (ushort)ModContent.TileType<硅孔雀石矿>();
            int minimumY = (int)Main.worldSurface;

            for (int x = 1; x < Main.maxTilesX - 1; x++)
            {
                for (int y = minimumY; y < Main.maxTilesY - 1; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (!tile.HasTile || tile.TileType != TileID.Stone || !TouchesGemCaveWall(x, y))
                        continue;

                    // 原版宝石洞穴中钻石占所有候选洞壁石块的平均概率为 1/120。
                    if (WorldGen.genRand.NextBool(120))
                        tile.TileType = tileType;
                }
            }
        }

        private static bool TouchesGemCaveWall(int x, int y)
        {
            return IsGemCaveWall(Main.tile[x, y].WallType)
                || IsGemCaveWall(Main.tile[x - 1, y].WallType)
                || IsGemCaveWall(Main.tile[x + 1, y].WallType)
                || IsGemCaveWall(Main.tile[x, y - 1].WallType)
                || IsGemCaveWall(Main.tile[x, y + 1].WallType);
        }

        private static bool IsGemCaveWall(ushort wallType)
        {
            return wallType >= WallID.AmethystUnsafe && wallType <= WallID.DiamondUnsafe;
        }
    }
}

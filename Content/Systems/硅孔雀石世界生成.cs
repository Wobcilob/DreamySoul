using System.Collections.Generic;
using DreamySoul.Content.Tiles;
using DreamySoul.Content.Walls;
using Microsoft.Xna.Framework;
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
            progress.Message = "正在寻找全宝石洞穴";

            ushort oreTileType = (ushort)ModContent.TileType<硅孔雀石矿>();
            ushort crystalTileType = (ushort)ModContent.TileType<硅孔雀石晶体>();
            ushort caveWallType = (ushort)ModContent.WallType<硅孔雀石洞墙>();
            int minimumY = (int)Main.worldSurface;
            bool[,] visited = new bool[Main.maxTilesX, Main.maxTilesY];
            Queue<Point> pending = new Queue<Point>();
            List<Point> caveTiles = new List<Point>();

            for (int x = 1; x < Main.maxTilesX - 1; x++)
            {
                progress.Set((double)x / Main.maxTilesX);

                for (int y = minimumY; y < Main.maxTilesY - 1; y++)
                {
                    if (visited[x, y] || !IsGemCaveWall(Main.tile[x, y].WallType))
                        continue;

                    int gemWallMask = CollectGemCave(x, y, visited, pending, caveTiles);
                    if (gemWallMask == 0b11_1111)
                        PopulateAllGemCave(caveTiles, oreTileType, crystalTileType, caveWallType);
                }
            }
        }

        private static int CollectGemCave(
            int startX,
            int startY,
            bool[,] visited,
            Queue<Point> pending,
            List<Point> caveTiles)
        {
            pending.Clear();
            caveTiles.Clear();
            pending.Enqueue(new Point(startX, startY));
            visited[startX, startY] = true;
            int gemWallMask = 0;

            while (pending.Count > 0)
            {
                Point point = pending.Dequeue();
                caveTiles.Add(point);

                ushort wallType = Main.tile[point.X, point.Y].WallType;
                gemWallMask |= 1 << (wallType - WallID.AmethystUnsafe);

                TryQueueGemWall(point.X - 1, point.Y, visited, pending);
                TryQueueGemWall(point.X + 1, point.Y, visited, pending);
                TryQueueGemWall(point.X, point.Y - 1, visited, pending);
                TryQueueGemWall(point.X, point.Y + 1, visited, pending);
            }

            return gemWallMask;
        }

        private static void TryQueueGemWall(int x, int y, bool[,] visited, Queue<Point> pending)
        {
            if (x <= 0 || x >= Main.maxTilesX - 1 || y <= 0 || y >= Main.maxTilesY - 1)
                return;

            if (visited[x, y] || !IsGemCaveWall(Main.tile[x, y].WallType))
                return;

            visited[x, y] = true;
            pending.Enqueue(new Point(x, y));
        }

        private static void PopulateAllGemCave(
            List<Point> caveTiles,
            ushort oreTileType,
            ushort crystalTileType,
            ushort caveWallType)
        {
            HashSet<Point> oreCandidates = new HashSet<Point>();

            foreach (Point point in caveTiles)
            {
                Tile tile = Main.tile[point.X, point.Y];

                // 全宝石洞中七种宝石墙等份：将原有六种墙的 1/7
                // 替换为硅孔雀石墙后，每种墙的最终平均占比均为 1/7。
                if (WorldGen.genRand.NextBool(7))
                    tile.WallType = caveWallType;

                // 原版已占用约一半可放置位置；在剩余空位中使用 1/6，
                // 使硅孔雀石晶体的最终份额与单种原版宝石晶体一致。
                if (!tile.HasTile && WorldGen.genRand.NextBool(6))
                    TryPlaceCrystal(point.X, point.Y, crystalTileType);

                AddStoneCandidate(point.X - 1, point.Y, oreCandidates);
                AddStoneCandidate(point.X + 1, point.Y, oreCandidates);
                AddStoneCandidate(point.X, point.Y - 1, oreCandidates);
                AddStoneCandidate(point.X, point.Y + 1, oreCandidates);
            }

            foreach (Point point in oreCandidates)
            {
                // 原版宝石洞穴中钻石占所有候选洞壁石块的平均概率为 1/120。
                if (WorldGen.genRand.NextBool(120))
                    Main.tile[point.X, point.Y].TileType = oreTileType;
            }
        }

        private static void AddStoneCandidate(int x, int y, HashSet<Point> oreCandidates)
        {
            Tile tile = Main.tile[x, y];
            if (tile.HasTile && tile.TileType == TileID.Stone)
                oreCandidates.Add(new Point(x, y));
        }

        private static void TryPlaceCrystal(int x, int y, ushort crystalTileType)
        {
            int[] orientations = new int[4];
            int orientationCount = 0;

            if (WorldGen.SolidTile(x, y + 1))
                orientations[orientationCount++] = 0;
            if (WorldGen.SolidTile(x, y - 1))
                orientations[orientationCount++] = 1;
            if (WorldGen.SolidTile(x - 1, y))
                orientations[orientationCount++] = 2;
            if (WorldGen.SolidTile(x + 1, y))
                orientations[orientationCount++] = 3;

            if (orientationCount == 0)
                return;

            int orientation = orientations[WorldGen.genRand.Next(orientationCount)];
            int variant = WorldGen.genRand.Next(3);
            Tile tile = Main.tile[x, y];
            tile.HasTile = true;
            tile.TileType = crystalTileType;
            tile.TileFrameX = 0;
            tile.TileFrameY = (short)((orientation * 3 + variant) * 18);
        }

        private static bool IsGemCaveWall(ushort wallType)
        {
            return wallType >= WallID.AmethystUnsafe && wallType <= WallID.DiamondUnsafe;
        }
    }
}

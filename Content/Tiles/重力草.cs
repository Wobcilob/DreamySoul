using System;
using System.Collections.Generic;
using DreamySoul.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DreamySoul.Content.Tiles
{
    public class 重力草 : ModTile
    {
        private const int FrameWidth = 18;
        private const int MatureStage = 2;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileID.Sets.BreakableWhenPlacing[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.AnchorValidTiles = new int[]
            {
                TileID.Grass,
                TileID.ClayPot,
                TileID.PlanterBox
            };
            TileObjectData.addTile(Type);

            DustType = DustID.Grass;
            HitSound = SoundID.Grass;
            AddMapEntry(new Color(92, 190, 72), CreateMapEntryName());
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int stage = tile.TileFrameX / FrameWidth;

            if (stage == 0)
            {
                SetStage(i, j, 1);
            }
            else if (stage == 1 && CanFullyMature(i, j))
            {
                SetStage(i, j, MatureStage);
            }
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            int stage = Main.tile[i, j].TileFrameX / FrameWidth;

            if (stage >= MatureStage)
            {
                yield return CreateDrop(ModContent.ItemType<重力果>(), Main.rand.Next(2, 4));
                yield return CreateDrop(ModContent.ItemType<重力草种子>(), 1);
            }
            else if (stage == 1 && IsUsingStaffOfRegrowth(i, j))
            {
                yield return CreateDrop(ModContent.ItemType<重力果>(), 1);
                yield return CreateDrop(ModContent.ItemType<重力草种子>(), 1);
            }
        }

        private static bool CanFullyMature(int i, int j)
        {
            Tile support = Main.tile[i, j + 1];
            if (!support.HasTile)
                return false;

            if (support.TileType == TileID.ClayPot || support.TileType == TileID.PlanterBox)
                return true;

            bool inSpaceLayer = j < Main.worldSurface * 0.35;
            return inSpaceLayer && support.TileType == TileID.Grass;
        }

        private static bool IsUsingStaffOfRegrowth(int i, int j)
        {
            Vector2 tileCenter = new Vector2(i * 16f + 8f, j * 16f + 8f);

            for (int playerIndex = 0; playerIndex < Main.maxPlayers; playerIndex++)
            {
                Player player = Main.player[playerIndex];
                if (!player.active || player.dead || player.HeldItem.type != ItemID.StaffofRegrowth)
                    continue;

                if (Vector2.DistanceSquared(player.Center, tileCenter) <= 12f * 16f * 12f * 16f)
                    return true;
            }

            return false;
        }

        private static Item CreateDrop(int itemType, int stack)
        {
            Item item = new Item();
            item.SetDefaults(itemType);
            item.stack = stack;
            return item;
        }

        private static void SetStage(int i, int j, int stage)
        {
            Main.tile[i, j].TileFrameX = (short)(stage * FrameWidth);
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, i, j, 1);
        }
    }
}

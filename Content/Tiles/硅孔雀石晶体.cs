using DreamySoul.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Tiles
{
    public class 硅孔雀石晶体 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;

            MineResist = 0.5f;
            MinPick = 0;
            HitSound = SoundID.Shatter;
            DustType = DustID.GemEmerald;

            AddMapEntry(new Color(109, 255, 224), CreateMapEntryName());
            RegisterItemDrop(ModContent.ItemType<硅孔雀石>());
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Main.tile[i, j];
            int orientation = tile.TileFrameY / 54;

            bool hasAnchor = orientation switch
            {
                0 => WorldGen.SolidTile(i, j + 1),
                1 => WorldGen.SolidTile(i, j - 1),
                2 => WorldGen.SolidTile(i - 1, j),
                3 => WorldGen.SolidTile(i + 1, j),
                _ => false
            };

            if (!hasAnchor)
                WorldGen.KillTile(i, j);

            // 帧由放置方向与随机外观决定，阻止普通 1x1 方块重设帧。
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.08f;
            g = 0.28f;
            b = 0.2f;
        }
    }
}

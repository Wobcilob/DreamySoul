using DreamySoul.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Tiles
{
    public class 硅孔雀石矿 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1000;
            Main.tileOreFinderPriority[Type] = 350;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;

            TileID.Sets.Ore[Type] = true;
            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;

            MineResist = 1f;
            MinPick = 0;
            HitSound = SoundID.Tink;
            DustType = DustID.GemEmerald;

            AddMapEntry(new Color(109, 255, 224), CreateMapEntryName());
            RegisterItemDrop(ModContent.ItemType<硅孔雀石>());
        }
    }
}

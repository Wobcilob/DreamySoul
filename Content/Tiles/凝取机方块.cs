using DreamySoul.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DreamySoul.Content.Tiles
{
    public class 凝取机方块 : ModTile
    {
        // 48×32 贴图对应横向 3 格、纵向 2 格。
        public override string Texture => "DreamySoul/Content/Items/凝取机";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(1, 1);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);

            HitSound = SoundID.Tink;
            DustType = DustID.Iron;
            AddMapEntry(new Microsoft.Xna.Framework.Color(120, 120, 135));
            RegisterItemDrop(ModContent.ItemType<凝取机>());
        }
    }
}

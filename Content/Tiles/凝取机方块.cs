using DreamySoul.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DreamySoul.Content.Tiles
{
    public class 凝取机方块 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(1, 2);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            // 每个动画帧高 52 像素，共 6 帧纵向排列。
            AnimationFrameHeight = 52;

            HitSound = SoundID.Tink;
            DustType = DustID.Iron;
            AddMapEntry(new Microsoft.Xna.Framework.Color(120, 120, 135));
            RegisterItemDrop(ModContent.ItemType<凝取机>());
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 8)
            {
                frameCounter = 0;
                frame = (frame + 1) % 6;
            }
        }
    }
}

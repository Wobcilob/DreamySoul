using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Walls
{
    public class 硅孔雀石洞墙 : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            WallID.Sets.Conversion.Stone[Type] = true;

            DustType = DustID.GemEmerald;
            HitSound = SoundID.Tink;

            AddMapEntry(new Color(58, 136, 115), CreateMapEntryName());
        }
    }
}

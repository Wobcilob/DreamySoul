using Terraria;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 重力草种子 : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<global::DreamySoul.Content.Tiles.重力草>());
            Item.width = 22;
            Item.height = 18;
            Item.maxStack = 9999;
        }
    }
}

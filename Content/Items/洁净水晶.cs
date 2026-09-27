using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 洁净水晶 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 原始贴图为四张 32×32 竖排帧。
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
        }
    }
}

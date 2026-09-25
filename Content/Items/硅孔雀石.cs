using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 硅孔雀石 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Green;
        }
    }
}

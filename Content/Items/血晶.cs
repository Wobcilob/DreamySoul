using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 血晶 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient<血晶碎片>(6)
                .AddIngredient(ItemID.SoulofSight, 2)
                .AddIngredient(ItemID.SoulofFright, 2)
                .AddIngredient(ItemID.SoulofMight, 2)
                .AddTile(ModContent.TileType<global::DreamySoul.Content.Tiles.凝取机方块>())
                .Register();
        }
    }
}

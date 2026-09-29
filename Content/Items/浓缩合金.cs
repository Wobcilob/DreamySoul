using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 浓缩合金 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.consumable = false;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 3);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddRecipeGroup("DreamySoul:AnyCopperTin", 5);
            recipe.AddRecipeGroup("DreamySoul:AnyIronLead", 5);
            recipe.AddRecipeGroup("DreamySoul:AnySilverTungsten", 5);
            recipe.AddRecipeGroup("DreamySoul:AnyGoldPlatinum", 5);
            recipe.AddRecipeGroup("DreamySoul:AnyCobaltPalladium", 5);
            recipe.AddRecipeGroup("DreamySoul:AnyMythrilOrichalcum", 5);
            recipe.AddRecipeGroup("DreamySoul:AnyAdamantiteTitanium", 5);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}

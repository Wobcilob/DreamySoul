using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
namespace DreamySoul.Content.Items
{
    public class Test : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = false;
            Item.maxStack = 9999;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useTurn = true;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 5f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 114514);
            recipe.AddTile(TileID.WorkBenches);//10木材做的工作台
            recipe.Register();
        }
    }
}
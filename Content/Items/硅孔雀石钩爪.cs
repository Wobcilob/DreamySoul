using DreamySoul.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 硅孔雀石钩爪 : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DiamondHook);
            Item.shoot = ModContent.ProjectileType<硅孔雀石钩爪弹幕>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<硅孔雀石>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

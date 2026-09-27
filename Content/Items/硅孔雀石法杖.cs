using DreamySoul.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 硅孔雀石法杖 : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DiamondStaff);
            Item.shoot = ModContent.ProjectileType<硅孔雀石法杖弹幕>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 10)
                .AddIngredient<硅孔雀石>(8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

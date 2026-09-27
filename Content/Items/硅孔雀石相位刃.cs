using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 硅孔雀石相位刃 : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WhitePhaseblade);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 15)
                .AddIngredient<硅孔雀石>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

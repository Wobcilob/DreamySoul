using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 硅孔雀石相位剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WhitePhasesaber);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<硅孔雀石相位刃>()
                .AddIngredient(ItemID.CrystalShard, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

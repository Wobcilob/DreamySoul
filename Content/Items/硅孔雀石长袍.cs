using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    [AutoloadEquip(EquipType.Body)]
    public class 硅孔雀石长袍 : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DiamondRobe);
            Item.bodySlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 80;
            player.manaCost -= 0.15f;
            player.hasGemRobe = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Robe)
                .AddIngredient<硅孔雀石>(10)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}

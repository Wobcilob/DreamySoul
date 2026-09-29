using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 暗影焰 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            RegisterWeaponConversion(ItemID.ShadowFlameHexDoll);
            RegisterWeaponConversion(ItemID.ShadowFlameBow);
            RegisterWeaponConversion(ItemID.ShadowFlameKnife);

            // “影徒锁链”不是当前版本的原版物品；加入同名模组物品后自动启用。
            if (Mod.TryFind("影徒锁链", out ModItem shadowDiscipleChain))
                RegisterWeaponConversion(shadowDiscipleChain.Type);
        }

        private void RegisterWeaponConversion(int weaponType)
        {
            CreateRecipe(5)
                .AddIngredient(weaponType)
                .AddTile(ModContent.TileType<global::DreamySoul.Content.Tiles.起源水晶球方块>())
                .Register();
        }
    }
}

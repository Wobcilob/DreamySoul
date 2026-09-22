using DreamySoul.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 合金短刀 : ModItem
    {
        public override void SetDefaults()
        {
            // 先完整复制原版铜短剑，使攻速、刺击动画和基础使用逻辑保持一致。
            Item.CloneDefaults(ItemID.CopperShortsword);

            // 只覆盖合金短刀自身的属性与外观弹幕。
            Item.width = 30;
            Item.height = 30;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 60);
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<合金短刀刺击>();
            Item.DamageType = DamageClass.Melee;
            Item.crit = 3;
            Item.damage = 59;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.GetInstance<浓缩合金>(), 2);
            recipe.Register();
        }
    }
}

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 死亡之魂 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 8 帧竖排动画，每 5 Tick 切换一帧。
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));

            // 使用原版灵魂在世界中的绘制效果，并像光明之魂一样悬浮。
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemIconPulse[Type] = true;
            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 50, 2);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.SoulofLight);
            recipe.AddIngredient(ItemID.SoulofNight);
            recipe.AddIngredient(ItemID.SoulofFlight);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddTile(ModContent.TileType<global::DreamySoul.Content.Tiles.凝取机方块>());
            recipe.Register();
        }
    }
}

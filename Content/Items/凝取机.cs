using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace DreamySoul.Content.Items
{
    public class 凝取机 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.consumable = false;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddIngredient(ItemID.Extractinator);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.Register();
        }
    }
}

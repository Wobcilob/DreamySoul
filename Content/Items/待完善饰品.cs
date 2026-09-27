using DreamySoul.Content.Players;
using DreamySoul.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    /// <summary>
    /// 仅提供饰品的基础装备属性；具体效果、稀有度、售价、配方与来源后续再补。
    /// </summary>
    public abstract class 待完善饰品 : ModItem
    {
        protected abstract int ItemWidth { get; }
        protected abstract int ItemHeight { get; }

        public override void SetDefaults()
        {
            Item.width = ItemWidth;
            Item.height = ItemHeight;
            Item.maxStack = 1;
            Item.accessory = true;
        }
    }

    [LegacyName("暗影鲨齿项链")]
    public class 暗影鲨牙项链 : 待完善饰品
    {
        protected override int ItemWidth => 32;
        protected override int ItemHeight => 40;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 10;
            player.GetModPlayer<饰品效果玩家>().启用暗影鲨牙项链 = true;
        }

        public override void AddRecipes()
        {
            // “暗影焰”并非原版材料；在该模组物品加入后自动启用完整配方。
            if (!Mod.TryFind("暗影焰", out ModItem shadowFlameMaterial))
                return;

            CreateRecipe()
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddIngredient(shadowFlameMaterial.Type, 5)
                .AddIngredient(ItemID.SoulofNight, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class 复生冰晶 : 待完善饰品
    {
        protected override int ItemWidth => 30;
        protected override int ItemHeight => 38;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<饰品效果玩家>().启用复生冰晶 = true;
        }

        public override void AddRecipes()
        {
            if (!Mod.TryFind("再生符文", out ModItem regenerationRune)
                || !Mod.TryFind("元素水晶", out ModItem elementalCrystal)
                || !Mod.TryFind("神话作坊", out ModTile mythicWorkshop))
            {
                return;
            }

            int pureCrystalType = ModContent.ItemType<洁净水晶>();
            if (Mod.TryFind("纯洁冰晶", out ModItem pureCrystal))
                pureCrystalType = pureCrystal.Type;

            CreateRecipe()
                .AddIngredient(regenerationRune.Type)
                .AddIngredient(elementalCrystal.Type)
                .AddIngredient(pureCrystalType, 20)
                .AddTile(mythicWorkshop.Type)
                .Register();
        }
    }

    [LegacyName("日耀徽章")]
    public class 日耀纹章 : 待完善饰品
    {
        protected override int ItemWidth => 28;
        protected override int ItemHeight => 28;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 使用乘算修正，使最终近战伤害严格乘以 1.15。
            player.GetDamage(DamageClass.Melee) *= 1.15f;
            player.GetCritChance(DamageClass.Melee) += 8f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.20f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WarriorEmblem)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddIngredient(ItemID.FragmentSolar, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    [LegacyName("星尘徽章")]
    public class 星尘纹章 : 待完善饰品
    {
        protected override int ItemWidth => 28;
        protected override int ItemHeight => 28;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 使用乘算修正，使最终召唤伤害严格乘以 1.15。
            player.GetDamage(DamageClass.Summon) *= 1.15f;
            player.GetCritChance(DamageClass.Summon) += 5f;
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SummonerEmblem)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddIngredient(ItemID.FragmentStardust, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    [LegacyName("星璇徽章")]
    public class 星璇纹章 : 待完善饰品
    {
        protected override int ItemWidth => 28;
        protected override int ItemHeight => 28;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 使用乘算修正，使最终远程伤害严格乘以 1.15。
            player.GetDamage(DamageClass.Ranged) *= 1.15f;
            player.GetCritChance(DamageClass.Ranged) += 12f;
            player.ammoCost75 = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RangerEmblem)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddIngredient(ItemID.FragmentVortex, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    [LegacyName("星云徽章")]
    public class 星云纹章 : 待完善饰品
    {
        protected override int ItemWidth => 28;
        protected override int ItemHeight => 28;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 使用乘算修正，使最终魔法伤害严格乘以 1.15。
            player.GetDamage(DamageClass.Magic) *= 1.15f;
            player.GetCritChance(DamageClass.Magic) += 8f;
            player.manaCost -= 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SorcererEmblem)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddIngredient(ItemID.FragmentNebula, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class 再生护符 : 待完善饰品
    {
        protected override int ItemWidth => 24;
        protected override int ItemHeight => 34;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<饰品效果玩家>().启用再生护符 = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<再生徽章>()
                .AddIngredient(ItemID.CrossNecklace)
                .AddIngredient<叶绿精华>(10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class 再生徽章 : 待完善饰品
    {
        protected override int ItemWidth => 28;
        protected override int ItemHeight => 28;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<饰品效果玩家>().启用再生徽章 = true;
        }
    }
}

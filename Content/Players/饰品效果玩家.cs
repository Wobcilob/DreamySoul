using System;
using System.Collections.Generic;
using DreamySoul.Content.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DreamySoul.Content.Players
{
    public class 饰品效果玩家 : ModPlayer
    {
        private const int 复生冷却总时长 = 60 * 60 * 3 + 60 * 30;
        private const int 再生护符冷却总时长 = 60 * 60 * 3;
        private const int 再生徽章冷却总时长 = 60 * 60 * 3;
        private static readonly HashSet<int> 可用治疗药水类型 = new HashSet<int>
        {
            ItemID.LesserHealingPotion,
            ItemID.HealingPotion,
            ItemID.GreaterHealingPotion,
            ItemID.SuperHealingPotion
        };

        public bool 启用暗影鲨牙项链;
        public bool 启用复生冰晶;
        public bool 启用再生护符;
        public bool 启用再生徽章;
        public bool 结晶再生激活;
        public float 元素伤害承受倍率 = 1f;
        public int 复生冰晶冷却;
        public int 再生护符冷却;
        public int 再生徽章冷却;

        private int 致命伤前生命;
        private int 最后受伤冷却槽 = ImmunityCooldownID.General;

        public override void ResetEffects()
        {
            启用暗影鲨牙项链 = false;
            启用复生冰晶 = false;
            启用再生护符 = false;
            启用再生徽章 = false;
            结晶再生激活 = false;
            元素伤害承受倍率 = 1f;
        }

        public override void PostUpdate()
        {
            if (复生冰晶冷却 > 0)
                复生冰晶冷却--;
            if (再生护符冷却 > 0)
                再生护符冷却--;
            if (再生徽章冷却 > 0)
                再生徽章冷却--;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            致命伤前生命 = Player.statLife;
            最后受伤冷却槽 = info.CooldownCounter;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            int extraImmuneTime = 0;
            if (启用复生冰晶)
                extraImmuneTime += 60;
            if (启用再生护符)
                extraImmuneTime += 40;

            if (extraImmuneTime > 0)
                Player.AddImmuneTime(info.CooldownCounter, extraImmuneTime);
        }

        public override bool PreKill(
            double damage,
            int hitDirection,
            bool pvp,
            ref bool playSound,
            ref bool genDust,
            ref PlayerDeathReason damageSource)
        {
            int potionSlot = FindAllowedHealingPotion();
            if (potionSlot < 0)
                return true;

            Item potion = Player.inventory[potionSlot];
            if (启用复生冰晶 && 复生冰晶冷却 <= 0)
            {
                int lifeBeforeFatalHit = Math.Clamp(致命伤前生命, 0, Player.statLifeMax2);
                int restoredLife = potion.healLife + (int)(lifeBeforeFatalHit * 0.3f);
                复生冰晶冷却 = 复生冷却总时长;
                ReviveWithPotion(
                    potion,
                    restoredLife,
                    ModContent.BuffType<结晶再生>(),
                    60 * 7,
                    60,
                    1.5f);
                playSound = false;
                genDust = false;
                return false;
            }

            if (启用再生护符 && 再生护符冷却 <= 0)
            {
                再生护符冷却 = 再生护符冷却总时长;
                ReviveWithPotion(
                    potion,
                    potion.healLife,
                    ModContent.BuffType<光合作用>(),
                    60 * 5,
                    40,
                    1.5f);
                playSound = false;
                genDust = false;
                return false;
            }

            if (启用再生徽章 && 再生徽章冷却 <= 0)
            {
                再生徽章冷却 = 再生徽章冷却总时长;
                ReviveWithPotion(
                    potion,
                    potion.healLife,
                    0,
                    0,
                    0,
                    1.66f,
                    1.33f);
                playSound = false;
                genDust = false;
                return false;
            }

            return true;
        }

        public override void SaveData(TagCompound tag)
        {
            if (复生冰晶冷却 > 0)
                tag["复生冰晶冷却"] = 复生冰晶冷却;
            if (再生护符冷却 > 0)
                tag["再生护符冷却"] = 再生护符冷却;
            if (再生徽章冷却 > 0)
                tag["再生徽章冷却"] = 再生徽章冷却;
        }

        public override void LoadData(TagCompound tag)
        {
            复生冰晶冷却 = tag.GetInt("复生冰晶冷却");
            再生护符冷却 = tag.GetInt("再生护符冷却");
            再生徽章冷却 = tag.GetInt("再生徽章冷却");
        }

        private void ReviveWithPotion(
            Item potion,
            int restoredLife,
            int recoveryBuffType,
            int recoveryBuffTime,
            int extraImmuneTime,
            float newPotionDelayMultiplier,
            float existingPotionDelayMultiplier = 0f)
        {
            int basePotionDelay = Item.potionDelay;
            potion.ModItem?.ModifyPotionDelay(Player, ref basePotionDelay);
            int actualPotionDelay = (int)MathF.Round(Player.PotionDelayModifier.ApplyTo(basePotionDelay));
            int existingPotionDelay = Player.potionDelay;
            int potionSickness = (int)MathF.Round(
                actualPotionDelay * newPotionDelayMultiplier
                + existingPotionDelay * existingPotionDelayMultiplier);

            ConsumeOne(potion);

            Player.statLife = Math.Clamp(restoredLife, 1, Player.statLifeMax2);
            Player.dead = false;
            Player.ghost = false;
            Player.HealEffect(Player.statLife, true);

            Player.ClearBuff(BuffID.PotionSickness);
            Player.potionDelay = potionSickness;
            Player.AddBuff(BuffID.PotionSickness, potionSickness);
            if (recoveryBuffType > 0 && recoveryBuffTime > 0)
                Player.AddBuff(recoveryBuffType, recoveryBuffTime);
            if (extraImmuneTime > 0)
                Player.AddImmuneTime(最后受伤冷却槽, extraImmuneTime);

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.PlayerLifeMana, -1, -1, null, Player.whoAmI);
        }

        private int FindAllowedHealingPotion()
        {
            for (int slot = 0; slot < 58 && slot < Player.inventory.Length; slot++)
            {
                Item item = Player.inventory[slot];
                if (item.IsAir || item.stack <= 0 || item.healLife <= 0 || !item.potion)
                    continue;

                if (可用治疗药水类型.Contains(item.type))
                    return slot;
            }

            return -1;
        }

        private static void ConsumeOne(Item item)
        {
            item.stack--;
            if (item.stack <= 0)
                item.TurnToAir();
        }
    }
}

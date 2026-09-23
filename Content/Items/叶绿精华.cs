using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DreamySoul.Content.Items
{
    public class 叶绿精华 : ModItem
    {
        // 单次闪烁使用阶梯亮度，不做平滑插值。
        private static readonly float[] FlashSteps =
        {
            0.55f, 0.8f, 1.15f, 1.45f, 1.15f, 0.8f, 0.55f
        };

        public override void SetStaticDefaults()
        {
            // 单帧贴图不能启用 AnimatesAsSoul，否则世界物品绘制时会读取不存在的动画帧。
            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override void SetDefaults()
        {
            // 基础属性与原版光明之魂保持一致。
            Item.CloneDefaults(ItemID.SoulofLight);
            Item.width = 32;
            Item.height = 32;
        }

        public override void PostUpdate()
        {
            float intensity = GetGlowIntensity();
            Lighting.AddLight(Item.Center, new Vector3(0.18f, 0.7f, 0.14f) * intensity);
        }

        public override bool PreDrawInInventory(
            SpriteBatch spriteBatch,
            Vector2 position,
            Rectangle frame,
            Color drawColor,
            Color itemColor,
            Vector2 origin,
            float scale)
        {
            DrawSelfGlow(
                spriteBatch,
                TextureAssets.Item[Type].Value,
                position,
                frame,
                origin,
                0f,
                scale);

            return true;
        }

        public override bool PreDrawInWorld(
            SpriteBatch spriteBatch,
            Color lightColor,
            Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI)
        {
            Main.GetItemDrawFrame(Type, out Texture2D texture, out Rectangle frame);
            Vector2 origin = frame.Size() / 2f;
            Vector2 position = Item.Bottom - Main.screenPosition - new Vector2(0f, origin.Y);

            DrawSelfGlow(spriteBatch, texture, position, frame, origin, rotation, scale);
            return true;
        }

        private static float GetGlowIntensity()
        {
            // 固定 180 Tick（3 秒）为一个周期；前 28 Tick 播放阶梯式闪烁。
            ulong cycleTick = Main.GameUpdateCount % 180UL;
            int flashIndex = (int)(cycleTick / 4UL);
            if (flashIndex < FlashSteps.Length)
                return FlashSteps[flashIndex];

            return 0.45f;
        }

        private static void DrawSelfGlow(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Vector2 position,
            Rectangle frame,
            Vector2 origin,
            float rotation,
            float scale)
        {
            float intensity = GetGlowIntensity();
            Color glowColor = new Color(125, 255, 145, 0) * intensity;

            // 只在贴图自身的有色像素范围内叠加发光，不再向外扩散。
            spriteBatch.Draw(
                texture,
                position,
                frame,
                glowColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f);
        }
    }
}

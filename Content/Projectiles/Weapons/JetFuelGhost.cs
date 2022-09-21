using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraMica.Content.Items.Weapons;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using TerraMica.Common;

namespace TerraMica.Content.Projectiles.Weapons
{
    public class JetFuelGhost : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jet Fuel Ghost");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.MonkStaffT2Ghast);
            AIType = ProjectileID.MonkStaffT2Ghast;
            Projectile.penetrate = 3;
            Projectile.DamageType = ModContent.GetInstance<PiercingDamageClass>(); // Set the damage to piercing damage.
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 0, 0) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = 20f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraMica.Content.Items.Weapons;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using TerraMica.Common;
using Terraria.Utilities;
using ReLogic.Utilities;
using TerraMica.Content.Buffs.Misc;

namespace TerraMica.Content.Projectiles.Weapons
{
    public class JetFuelGhost : ModProjectile
    {
        public const float lifeTime = 85f;
        ref float ProjLife => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jet Fuel Ghost");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {

            Projectile.netImportant = true; // Sync this projectile if a player joins mid game.

            // The width and height do not affect the collision of the Jousting Lance because we calculate that separately (see Colliding() below)
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            //Projectile.CloneDefaults(ProjectileID.MonkStaffT2Ghast);
            //AIType = ProjectileID.MonkStaffT2Ghast;
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
            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);

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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.buffImmune[BuffID.Oiled])
            {
                target.AddBuff(ModContent.BuffType<BetterOiled>(), Main.rand.Next(8, 18) * 30);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (!target.buffImmune[BuffID.Oiled])
            {
                target.AddBuff(ModContent.BuffType<BetterOiled>(), Main.rand.Next(8, 18) * 30);
            }
        }

        public override void AI()
        {
			SlotId val;
			if (ProjLife == 0f)
			{
				float[] array = Projectile.localAI;
				val = SoundEngine.PlaySound(in SoundID.DD2_GhastlyGlaiveImpactGhost, Projectile.Center);
				array[1] = ((SlotId)(val)).ToFloat();
			}
            if (SoundEngine.TryGetActiveSound(SlotId.FromFloat(Projectile.localAI[1]), out var sound))
            {
                // use "sound" here
                if (sound == null)
                {
                    float[] array2 = Projectile.localAI;
                    val = SlotId.Invalid;
                    array2[1] = ((SlotId)(val)).ToFloat();
                }
                else
                {
                    sound.Position = Projectile.Center;
                }
            }


            ProjLife += 1f;
			if (ProjLife > lifeTime * 0.85f) //if projLife is greater than about 3/4ths of lifeTime...
			{
				Projectile.alpha += 25;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
				}
			}
			else //begin fading
			{
				Projectile.alpha -= ((int)lifeTime * 3 / 2);
				if (Projectile.alpha < 255)
				{
					Projectile.alpha = 127;
				}
			}
			Projectile.velocity *= 0.98f;
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}
			if (ProjLife >= lifeTime)//if Life is equal to or greater than lifeTime than execute contents.
			{
				Projectile.Kill();
			}
			Projectile.direction = (Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1)));
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += (float)Math.PI;
			}
			if (ProjLife >= 3 && ProjLife < (lifeTime * 0.8f))//if projectile has existed for three frames but hasnt already existed for about 3/4th's of lifeTime
			{
				Vector2 vector = Projectile.velocity.SafeNormalize(Vector2.UnitY);
				float num = ProjLife / lifeTime;
				float num2 = 2f;
				for (int i = 0; (float)i < num2; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, DustID.GoldFlame, 0f, 0f, 110);//228 here appears to be a DustID
					dust.velocity = vector * 2f;
					dust.position = Projectile.Center + vector.RotatedBy(num * ((float)Math.PI * 2f) * 2f + (float)i / num2 * ((float)Math.PI * 2f)) * 7f;
					dust.scale = 1f + 0.6f * Main.rand.NextFloat();
					dust.velocity += vector * 3f;
					dust.noGravity = true;
				}
			}
		}
	}
}
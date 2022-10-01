using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace TerraMica.Content.Projectiles.NPCs
{
    public class Flamethrower : ModProjectile
    {
        ref float ProjLife => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flamethrower");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 50;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<FlamethrowerBlast>(), damage, Projectile.knockBack, Projectile.owner);
            target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(8, 16));
            Projectile.Kill();
        }

        public override void AI()
        {
            if (ProjLife == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                ProjLife += 1f;
            }
            bool flag15 = false;
            bool flag16 = false;
            if (Projectile.velocity.X < 0f && Projectile.position.X < Projectile.ai[0])
            {
                flag15 = true;
            }
            if (Projectile.velocity.X > 0f && Projectile.position.X > Projectile.ai[0])
            {
                flag15 = true;
            }
            if (Projectile.velocity.Y < 0f && Projectile.position.Y < Projectile.ai[1])
            {
                flag16 = true;
            }
            if (Projectile.velocity.Y > 0f && Projectile.position.Y > Projectile.ai[1])
            {
                flag16 = true;
            }
            if (flag15 && flag16)
            {
                Projectile.Kill();
            }
            for (int num378 = 0; num378 < 10; num378++)
            {
                int num379 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.InfernoFork, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num379].noGravity = true;
                Dust dust2 = Main.dust[num379];
                dust2.velocity *= 0.5f;
                dust2 = Main.dust[num379];
                dust2.velocity += Projectile.velocity * 0.1f;
            }
            return;
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace TerraMica.Content.Projectiles.NPCs
{
    public class FlamethrowerBlast : ModProjectile
    {
        //ref float ProjLife => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flamethrower");
        }

        public override void SetDefaults()
        {
            Projectile.width = 130;
            Projectile.height = 130;
            Projectile.aiStyle = 50;
            Projectile.damage = 25;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(8, 16));
        }
    }
}

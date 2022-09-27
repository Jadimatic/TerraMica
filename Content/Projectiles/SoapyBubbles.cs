using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Content.Projectiles
{
    public class SoapyBubbles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soapy Bubbles");
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.aiStyle = 27;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30;
        }

        public override bool PreAI()
        {
            Projectile.tileCollide = false;
            if (Main.rand.NextBool(15))
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BubbleBlock, 0f, 0f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].noGravity = true;
            }
            return false;
        }
    }
}
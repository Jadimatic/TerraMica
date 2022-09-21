using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerraMica.Content.Projectiles.Weapons
{
    internal class ExampleSwordProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            
            Projectile.aiStyle = ProjAIStyleID.ShortSword;
        }

        public override string Texture => "TerraMica/Content/Items/Weapons/ExampleSword";
        public override void AI()
        {
            //This function makes it so the projectile reflects the sprite accurately
            base.AI();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

            int halfProjWidth = Projectile.width / 2;
            int halfProjHeight = Projectile.width / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -((32 / 2) - halfProjWidth);
            DrawOriginOffsetY = -((32 / 2) - halfProjHeight);
        }
    }
}

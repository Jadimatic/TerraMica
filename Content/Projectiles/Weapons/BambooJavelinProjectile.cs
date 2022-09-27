using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraMica.Content.Items.Weapons;
using Terraria.Audio;
using TerraMica.Common;
using System;

namespace TerraMica.Content.Projectiles.Weapons
{
    public class BambooJavelinProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bamboo Javelin");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.JavelinFriendly);
            AIType = ProjectileID.JavelinFriendly;
            Projectile.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Projectile.penetrate = 3;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.rand.Next(0, 4) == 0)
                Item.NewItem(Projectile.GetSource_DropAsItem(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<BambooJavelin>(), 1, false, 0, false, false);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }
    }
}
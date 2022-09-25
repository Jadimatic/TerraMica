using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TerraMica.Common;
using TerraMica.Content.Projectiles.Weapons;
using Terraria.Enums;

namespace TerraMica.Common
{
    public class TerraMicaProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void ModifyDamageScaling(Projectile projectile, ref float damageScale)
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<TerraMicaPlayer>().aeroGel)
            {
                if (projectile.DamageType == ModContent.GetInstance<PiercingDamageClass>())
                {
                    damageScale *= 0.1f + Main.player[projectile.owner].velocity.Length() / 7.25f * 1f;
                }
            }
        }
        public override void SetDefaults(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.type == ProjectileID.JoustingLance) //|| projectile.type == ProjectileID.JoustingLance && player.GetModPlayer<TerraMicaPlayer>().stickyFingers)
            {
                projectile.DamageType = ModContent.GetInstance<PiercingDamageClass>();
                //player.GetModPlayer<TerraMicaPlayer>().vanillaLance = true;
            }
            if (projectile.type == ProjectileID.ShadowJoustingLance)
            {
                projectile.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
            if (projectile.type == ProjectileID.HallowJoustingLance)
            {
                projectile.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
        }
    }
}
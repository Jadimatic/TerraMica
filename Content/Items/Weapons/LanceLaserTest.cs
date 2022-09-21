using Microsoft.Xna.Framework;
using System;
using TerraMica.Common;
using TerraMica.Content.Projectiles.Weapons;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Content.Items.Weapons
{
    public class LanceLaserTest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lance Laser Test");
            Tooltip.SetDefault("Tests the Laser Lance Laser projectile");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.mana = 0;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = new SoundStyle?((SoundStyle)SoundID.Item117);
            Item.autoReuse = true;
            Item.shootSpeed = 12.0f;
            Item.shoot = ModContent.ProjectileType<LaserLanceLaser>();
        }

        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-10f, 0.0f));

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 3; ++index)
                Projectile.NewProjectile((IEntitySource)source, position, velocity, type, damage, knockback, (int)((Entity)player).whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}

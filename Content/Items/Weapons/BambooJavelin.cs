using TerraMica.Content.Projectiles.Weapons;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TerraMica.Common;

namespace TerraMica.Content.Items.Weapons
{
    public class BambooJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bamboo Javelin");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Javelin);
            Item.shoot = ModContent.ProjectileType<BambooJavelinProjectile>();
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.damage = 5;
        }
    }
}
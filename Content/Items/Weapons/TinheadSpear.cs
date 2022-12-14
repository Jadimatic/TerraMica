using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using TerraMica;
using TerraMica.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using TerraMica.Content.Buffs.Misc;


namespace TerraMica.Content.Items.Weapons
{
    public class TinheadSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            // The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
            Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance"));

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // The number of sacrifices that is required to research the item in Journey Mode.
        }

        public override void SetDefaults()
        {
            //Item.CloneDefaults(ItemID.ShadowJoustingLance);
            Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Weapons.TinheadSpearProjectile>(), 1f, 24);
            //Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.SetWeaponValues(15, 6f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.
            Item.SetShopValues(ItemRarityColor.White0, Item.buyPrice(10, 0)); // A special method that sets the rarity and value.
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // If the player has increased melee speed, it will effect the shootSpeed of the Jousting Lance which will cause the projectile to spawn further away than it is supposed to.
            // This ensures that the velocity of the projectile is always the shootSpeed.
            float inverseMeleeSpeed = 1f / player.GetTotalAttackSpeed(DamageClass.Melee);
            velocity *= inverseMeleeSpeed;
            if (!player.buffImmune[ModContent.BuffType<StickyFingersBuff>()])
            {
                Item.InterruptChannelOnHurt = true;
                Item.StopAnimationOnHurt = true;
            }
            else
            {
                Item.InterruptChannelOnHurt = false;
                Item.StopAnimationOnHurt = false;
            }
        }

        public override bool MeleePrefix()
        {
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TinBar, 3); //add 12 wood to this recipe later, also add leather for the grips, this will also make it harder to obtain
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
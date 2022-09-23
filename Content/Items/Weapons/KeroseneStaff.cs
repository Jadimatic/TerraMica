using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using TerraMica.Common;
using Terraria.DataStructures;
using TerraMica.Content.Projectiles.Weapons;

namespace TerraMica.Content.Items.Weapons
{
    public class KeroseneStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
            Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance") + "\nUnleashes powerful jet fuel ghosts");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // The number of sacrifices that is required to research the item in Journey Mode.
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.JoustingLance);

            Item.DefaultToSpear(ModContent.ProjectileType<KeroseneStaffProjectile>(), 1f, 24);
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.SetWeaponValues(40, 11f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.
            //Item.shootSpeed = 0.5f;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(35, 0)); // A special method that sets the rarity and value.
            Item.channel = true; // Channel is important for our projectile.

            


            // This will make sure our projectile completely disappears on hurt.
            // It's not enough just to stop the channel, as the lance can still deal damage while being stowed
            // If two players charge at each other, the first one to hit should cancel the other's lance
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // If the player has increased melee speed, it will effect the shootSpeed of the Jousting Lance which will cause the projectile to spawn further away than it is supposed to.
            // This ensures that the velocity of the projectile is always the shootSpeed.
            float inverseMeleeSpeed = 1f / player.GetTotalAttackSpeed(DamageClass.Melee);
            velocity *= inverseMeleeSpeed;
        }

        // This will allow our Jousting Lance to receive the same modifiers as melee weapons.
        public override bool MeleePrefix()
        {
            return true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ItemID.Amber, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
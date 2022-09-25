using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using TerraMica;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using TerraMica.Common;
using Terraria.DataStructures;
using TerraMica.Content.Projectiles.Weapons;
using TerraMica.Content.Buffs.Misc;

namespace TerraMica.Content.Items.Weapons
{
    public class LaserLanceCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
            Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance") + "\nFires rapid bursts of laser in the opposite direction of where the lance is pointed");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // The number of sacrifices that is required to research the item in Journey Mode.
        }

        public override void SetDefaults()
        {

            // A special method that sets a variety of item parameters that make the item act like a spear weapon.
            // To see everything DefaultToSpear() does, right click the method in Visual Studios and choose "Go To Definition" (or press F12).
            // The shoot speed will affect how far away the projectile spawns from the player's hand.
            // If you are using the custom AI in your projectile (and not aiStyle 19 and AIType = ProjectileID.JoustingLance), the standard value is 1f.
            // If you are using aiStyle 19 and AIType = ProjectileID.JoustingLance, then multiply the value by about 3.5f.
            Item.DefaultToSpear(ModContent.ProjectileType<LaserLanceCannonProjectile>(), 1f, 24);

            //Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.SetWeaponValues(14, 6f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.
            Item.shootSpeed = 0.5f;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(35, 0)); // A special method that sets the rarity and value.
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

        // This will allow our Jousting Lance to receive the same modifiers as melee weapons.
        public override bool MeleePrefix()
        {
            return true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 18); //add 12 wood to this recipe later, also add leather for the grips, this will also make it harder to obtain
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
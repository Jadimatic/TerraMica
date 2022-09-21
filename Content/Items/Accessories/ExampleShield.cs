using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria;
using TerraMica.Content.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using TerraMica.Common;

namespace TerraMica.Content.Items.Accessories
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class ExampleShield : ModItem
	{
        public const int Base_Damage = 8;

        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee damage by 20%\nAllows the player to dash into the enemy\nDouble tap a direction");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(20, 0));
            Item.accessory = true;

			Item.defense = 1;
			//Item.lifeRegen = 10;

			Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
			Item.damage = Base_Damage;
			Item.knockBack = 9f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.dashType = 2;
			player.GetDamage(ModContent.GetInstance<PiercingDamageClass>()) += 0.2f; // Increase ALL player damage by 20%
			player.endurance = 1f - (0.1f * (1f - player.endurance));  // The percentage of damage reduction // Damage reduction is not stated in the tooltip. Remove?
			player.GetModPlayer<TerraMicaPlayer>().DashAccessoryEquipped = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
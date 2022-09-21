using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TerraMica.Content.Projectiles.Weapons;
using TerraMica.Common;

namespace TerraMica.Content.Items.Weapons
{
	public class ExampleSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ExampleSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("We wanna turn this into a lance if possible");
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 4f;
			Item.value = Item.buyPrice(gold: 20);
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.noUseGraphic = true;
			Item.noMelee = true;

			Item.shootSpeed = 4.2f;
			Item.shoot = ModContent.ProjectileType<ExampleSwordProjectile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
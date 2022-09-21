using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TerraMica.Content
{
	// This class contains thoughtful examples of item recipe creation.
	public class ExampleRecipes : ModSystem
	{
		// A place to store the recipe group so we can easily use it later
		/*public static RecipeGroup MicaRecipeGroup;

		public override void Unload()
		{
			MicaRecipeGroup = null;
		}

		public override void AddRecipeGroups()
		{
			// Create a recipe group and store it
			// Language.GetTextValue("LegacyMisc.37") is the word "Any" in english, and the corresponding word in other languages
			MicaRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<Items.ExampleItem>())}",
				ModContent.ItemType<Items.ExampleItem>(), ModContent.ItemType<Items.ExampleDataItem>());

			// To avoid name collisions, when a modded items is the iconic or 1st item in a recipe group, name the recipe group: ModName:ItemName
			RecipeGroup.RegisterGroup("ExampleMod:ExampleItem", MicaRecipeGroup);

			// Add an item to an existing Terraria recipeGroup
			//RecipeGroup.recipeGroups[RecipeGroupID.Snails].ValidItems.Add(ModContent.ItemType<Items.ExampleCritter>());

			// While an "IronBar" group exists, "SilverBar" does not. tModLoader will merge recipe groups registered with the same name, so if you are registering a recipe group with a vanilla item as the 1st item, you can register it using just the internal item name if you anticipate other mods wanting to use this recipe group for the same concept. By doing this, multiple mods can add to the same group without extra effort. In this case we are adding a SilverBar group. Don't store the RecipeGroup instance, it might not be used, use the same nameof(ItemID.ItemName) or RecipeGroupID returned from RegisterGroup when using Recipe.AddRecipeGroup instead.
			RecipeGroup SilverBarRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}",
			ItemID.SilverBar, ItemID.TungstenBar, ModContent.ItemType<Items.Placeable.ExampleBar>());
			RecipeGroup.RegisterGroup(nameof(ItemID.SilverBar), SilverBarRecipeGroup);
		}*/

		public override void AddRecipes()
		{
			////////////////////////////////////////////////////////////////////////////////////
			// The following recipes make 1 bar out of 4 coins. //
			////////////////////////////////////////////////////////////////////////////////////

			Recipe goldBarRecipe = Recipe.Create(ItemID.GoldBar, 1);
			goldBarRecipe.AddIngredient(ItemID.GoldCoin, 4);
			goldBarRecipe.Register();

			Recipe copperBarRecipe = Recipe.Create(ItemID.CopperBar, 1);
			copperBarRecipe.AddIngredient(ItemID.CopperCoin, 4);
			copperBarRecipe.Register();

			Recipe silverBarRecipe = Recipe.Create(ItemID.SilverBar, 1);
			silverBarRecipe.AddIngredient(ItemID.SilverCoin, 4);
			silverBarRecipe.Register();

			Recipe platinumBarRecipe = Recipe.Create(ItemID.PlatinumBar, 1);
			platinumBarRecipe.AddIngredient(ItemID.PlatinumCoin, 4);
			platinumBarRecipe.Register();

			////////////////////////////////////////////////////////////////////////////////////
			// The following recipe makes a JoustingLance out of 18 IronBars. //
			////////////////////////////////////////////////////////////////////////////////////

			Recipe shadowJoustingLanceRecipe = Recipe.Create(ItemID.ShadowJoustingLance, 1);
			shadowJoustingLanceRecipe.AddIngredient(ItemID.DemoniteBar, 12);
			shadowJoustingLanceRecipe.AddIngredient(ItemID.SoulofNight, 10);
			shadowJoustingLanceRecipe.AddTile(TileID.Anvils);
			shadowJoustingLanceRecipe.Register();

			// Source for ItemIDs: https://github.com/tModLoader/tModLoader/wiki/Vanilla-Item-IDs
		}

		public override void PostAddRecipes()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];

				// All recipes that require wood will now need 100% more
				if (recipe.TryGetIngredient(ItemID.Wood, out Item ingredient))
				{
					ingredient.stack *= 2;
				}
			}
		}
	}
}
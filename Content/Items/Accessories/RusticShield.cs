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
	public class RusticShield : ModItem
	{
        public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Rustic Shield");
            Tooltip.SetDefault("Allows the player to dash into the enemy\nDouble tap a direction");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(20, 0));
            Item.accessory = true;
			Item.defense = 1;
			Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
			Item.damage = 8;
			Item.knockBack = 9f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<TerraMicaPlayer>().rusticShield = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
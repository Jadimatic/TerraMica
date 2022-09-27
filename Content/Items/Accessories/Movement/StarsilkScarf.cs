using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using TerraMica.Common;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Neck)]
    public class StarsilkScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starsilk Scarf");
            Tooltip.SetDefault("Allows the player to magically float upwards\nWhile floating, you are granted temporary immunity\nDouble tap UP to float");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(45));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TerraMicaPlayer>().starSilk = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
}
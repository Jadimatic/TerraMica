using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Enums;

namespace TerraMica.Content.Items.Materials
{
    public class MeteorDust : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Dust");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.SetShopValues(ItemRarityColor.White0, Item.buyPrice(5));
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ItemID.MeteoriteBar, 1)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}
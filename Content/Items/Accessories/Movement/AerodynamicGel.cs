using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using TerraMica.Common;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shoes)]
    public class AerodynamicGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sudsy Soap");
            Tooltip.SetDefault("Increases piercing damage done while moving\n3% increased movement speed\nMakes your feet soapy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(45));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TerraMicaPlayer>().aeroGel = true;
            player.maxRunSpeed *= 1.03f;
            player.slippy = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.AddIngredient(ItemID.Cloud, 3);
            recipe.AddTile(TileID.Solidifier);
            recipe.AddCondition(Recipe.Condition.NearWater);
            recipe.Register();
        }
    }
}
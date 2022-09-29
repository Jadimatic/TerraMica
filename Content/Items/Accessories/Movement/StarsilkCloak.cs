using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Back)]
    public class StarsilkCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starsilk Cloak");
            Tooltip.SetDefault("Allows the player to magically float upwards\nWhile floating, you are granted temporary immunity\nCauses stars to fall after taking damage\nDouble tap UP to float");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(2, 0, 75));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StarSilkPlayer>().starSilk = true;
            player.starCloakItem = Item;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StarsilkScarf>());
            recipe.AddIngredient(ItemID.StarCloak);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
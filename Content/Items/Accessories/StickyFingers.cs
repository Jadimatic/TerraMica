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
    [AutoloadEquip(EquipType.HandsOn)]
    public class StickyFingers : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers");
            Tooltip.SetDefault("Prevents you from getting staggered while using a lance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(20, 0));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TerraMicaPlayer>().stickyFingers = true;
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
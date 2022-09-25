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
using System.Drawing.Drawing2D;

namespace TerraMica.Content.Items.Accessories
{
    public class AerodynamicGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sudsy Soap");
            Tooltip.SetDefault("Increases piercing damage done while moving\n3% increased movement speed");
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
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
using System;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using TerraMica.Common;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shoes)]
    public class ManTreaders : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Man Treaders");
            Tooltip.SetDefault("Allows the player to stomp enemies\nDouble tap DOWN to stomp");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(10);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.damage = 25;
            Item.knockBack = 1f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TerraMicaPlayer>().manTreads = true;
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
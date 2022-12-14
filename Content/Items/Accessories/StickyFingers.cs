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
using TerraMica.Content.Buffs.Misc;

namespace TerraMica.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
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
            player.buffImmune[ModContent.BuffType<StickyFingersBuff>()] = true;
        }
    }
}
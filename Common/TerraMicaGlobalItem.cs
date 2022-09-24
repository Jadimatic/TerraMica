using System;
using System.Collections.Generic;
using TerraMica.Content.Projectiles.Weapons;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Common
{
    public class TerraMicaGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.EoCShield)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
            if (item.type == ItemID.JoustingLance)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
            if (item.type == ItemID.ShadowJoustingLance)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
                item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(2, 40, 0));
            }
            if (item.type == ItemID.HallowJoustingLance)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
        }
    }
}
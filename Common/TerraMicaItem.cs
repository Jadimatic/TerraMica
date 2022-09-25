using System;
using System.Collections.Generic;
using System.Threading.Channels;
using TerraMica.Content.Items.Accessories;
using TerraMica.Content.Items.Weapons;
using TerraMica.Content.Projectiles.Weapons;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Common
{
    public class TerraMicaItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            Player player = Main.player[1];
            if (item.type == ItemID.EoCShield)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
            if (item.type == ItemID.JoustingLance)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
                item.DefaultToSpear(ModContent.ProjectileType<JoustingLanceNewProjectile>(), 3.5f, 24);
                item.SetWeaponValues(56, 12f);
                item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(6));
                item.channel = true;
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
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
using TerraMica.Content.Buffs.Misc;
using TerraMica.Content.Items.Accessories;
using TerraMica.Content.Items.Weapons;
using TerraMica.Content.Projectiles.Weapons;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace TerraMica.Common
{
    public class TerraMicaItem : GlobalItem
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
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.type == ItemID.HallowJoustingLance)
            {
                if (!player.buffImmune[ModContent.BuffType<StickyFingersBuff>()])
                {
                    item.InterruptChannelOnHurt = true;
                    item.StopAnimationOnHurt = true;
                }
                else
                {
                    item.InterruptChannelOnHurt = false;
                    item.StopAnimationOnHurt = false;
                }
            }
            if (item.type == ItemID.ShadowJoustingLance)
            {
                if (!player.buffImmune[ModContent.BuffType<StickyFingersBuff>()])
                {
                    item.InterruptChannelOnHurt = true;
                    item.StopAnimationOnHurt = true;
                }
                else
                {
                    item.InterruptChannelOnHurt = false;
                    item.StopAnimationOnHurt = false;
                }
            }
            if (item.type == ItemID.JoustingLance)
            {
                if (!player.buffImmune[ModContent.BuffType<StickyFingersBuff>()])
                {
                    item.InterruptChannelOnHurt = true;
                    item.StopAnimationOnHurt = true;
                }
                else
                {
                    item.InterruptChannelOnHurt = false;
                    item.StopAnimationOnHurt = false;
                }
            }
        }
    }
}
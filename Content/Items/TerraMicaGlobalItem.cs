using System;
using System.Collections.Generic;
using TerraMica.Common;
using TerraMica.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Items
{
    // This is another part of the ExampleShiftClickSlotPlayer.cs that adds a tooltip line to the gel
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
            }
            if (item.type == ItemID.HallowJoustingLance)
            {
                item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            }
        }
    }
    /*public override void PostUpdate(Item item)
    {
        List<int> moddedLances = TerraMicaLists.ModdedLanceItems;
        // ISSUE: explicit non-virtual call
        if ((moddedLances != null ? (moddedLances.Contains((int)item.type) ? 1 : 0) : 0) == 0)
            return;
        CalamityUtils.ForceItemIntoWorld(item);
    }*/
}
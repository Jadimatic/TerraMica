using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TerraMica.Content.Items;
using TerraMica.Content.Items.Weapons;
using Terraria.ID;

namespace TerraMica.Common
{
    public class TerraMicaLists
    {
        public static List<int> ModdedLanceItems;

        public static void LoadLists()
        {
            ModdedLanceItems = new List<int>()
            {
                ModContent.ItemType<BloodstainedJoustingLance>(),
                ModContent.ItemType<CopperheadSpear>(),
                ModContent.ItemType<LaserLanceCannon>(),
            };
        }

        public static void UnloadLists()
        {
            ModdedLanceItems = null;
        }
    }
}

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraMica.Common;
using IL.Terraria.DataStructures;

namespace TerraMica.Content.Buffs.Misc
{
    public class StickyFingersBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers Check");
            Description.SetDefault("You shouldn't be seeing this!");
            Main.debuff[Type] = true;
        }
    }
}
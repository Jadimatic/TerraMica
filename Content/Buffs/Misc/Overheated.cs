using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraMica.Common;
using IL.Terraria.DataStructures;

namespace TerraMica.Content.Buffs.Misc
{
    public class Overheated : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overheated");
            Description.SetDefault("Your weapon has overheated\nand needs to cool off");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerraMicaPlayer modPlayer = player.GetModPlayer<TerraMicaPlayer>();
            modPlayer.overHeated = true;
        }
    }
}
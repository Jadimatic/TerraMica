using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraMica.Common;
using IL.Terraria.DataStructures;

namespace TerraMica.Content.Buffs.Misc
{
    public class HellishRebuke : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellish Rebuke");
            Description.SetDefault("You have been judged by the stars");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerraMicaPlayer modPlayer = player.GetModPlayer<TerraMicaPlayer>();
            modPlayer.hellishRebuke = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            TerraMicaNPC modNPC = npc.GetGlobalNPC<TerraMicaNPC>();
            modNPC.hellishRebuke = true;
        }
    }
}
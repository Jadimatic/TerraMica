using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraMica.Common;

namespace TerraMica.Content.Buffs.Misc
{
    public class BetterOiled : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oiled");
            Description.SetDefault("Taking more damage from being on fire");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerraMicaPlayer modPlayer = player.GetModPlayer<TerraMicaPlayer>();
            modPlayer.betterOiled = true;
            player.buffImmune[BuffID.Oiled] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.oiled = true;
        }
    }
}
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraMica.Common;

namespace TerraMica.Content.Buffs.Misc
{
    public class AstralDislocation1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Dislocation");
            Description.SetDefault("You don't feel so good...\nDefense reduced by 10%");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //BuffID.Sets.LongerExpertDebuff[Type] = true; // Uncomment this if you want the debuff to last longer in Expert/Master Mode
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.statDefense = (int)(player.statDefense * 0.9f);
            player.buffImmune[ModContent.BuffType<AstralDislocationCheck>()] = true;
        }
    }
}
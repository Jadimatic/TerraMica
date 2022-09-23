using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Common
{
    public class TerraMicaNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        //public bool betterOiled = false;

        /*public override void ResetEffects(NPC npc)
        {
            betterOiled = false;
        }*/

        /*public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (betterOiled)
            {
                npc.oiled = true;
            }
        }*/
    }
}

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using TerraMica.Content.Items.Accessories;
using TerraMica.Content.Items.Materials;

namespace TerraMica.Common
{
    public class TerraMicaNPCLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.MeteorHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeteorDust>(), 1, 1, 3));
            }
        }
    }
}
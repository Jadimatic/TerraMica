using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using TerraMica.Content.Items.Accessories;

namespace TerraMica.Common
{
    public class TerraMicaNPCLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.GiantCursedSkull)
            {
                npcLoot.RemoveWhere(
                    rule => rule is ItemDropWithConditionRule drop
                        && drop.itemId == ItemID.ShadowJoustingLance
                );
            }
        }
    }
}
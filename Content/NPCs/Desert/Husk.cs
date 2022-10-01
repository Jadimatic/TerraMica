using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;

namespace TerraMica.Content.NPCs.Desert
{
    public class Husk : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Husk");

            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DesertGhoul];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 44;
            NPC.aiStyle = 3;
            NPC.damage = 25;
            NPC.defense = 13;
            NPC.lifeMax = 90;
            NPC.HitSound = SoundID.NPCHit37;
            NPC.DeathSound = SoundID.NPCDeath40;
            NPC.knockBackResist = 0.6f;
            NPC.value = 75f;
            NPC.npcSlots = 0.5f;
            AIType = NPCID.DesertGhoul;
            AnimationType = NPCID.DesertGhoul;
        }

        public override void AI()
        {
            NPC.position -= NPC.velocity * 0.5f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("Husks dwell deep in desert sands, come stumbling into their domain and they'll impart a deep hunger upon your soul to go with that dreadful thirst you must be suffering."),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.DesertCave.Chance * 0.2f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var ghoulDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.DesertGhoul, false);
            foreach (var ghoulDropRule in ghoulDropRules)
            {
                npcLoot.Add(ghoulDropRule);
            }
        }
    }
}
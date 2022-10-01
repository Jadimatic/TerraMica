using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TerraMica.Content.Projectiles.NPCs;
using Terraria.GameContent.ItemDropRules;
using TerraMica.Content.Items.Materials;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;

namespace TerraMica.Content.NPCs.Meteor
{
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class MeteorSerpentHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<MeteorSerpentBody>();

        public override int BodyTypeAlt => ModContent.NPCType<MeteorSerpentBody>();

        public override int TailType => ModContent.NPCType<MeteorSerpentTail>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Serpent");

            /*var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "ExampleMod/Content/NPCs/MeteorSerpent_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);*/

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.Confused,
                    BuffID.Poisoned,
                    BuffID.OnFire3
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SeekerHead);
            NPC.lifeMax = 175;
            NPC.npcSlots = 15f;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 12;
            NPC.value = 125;
            NPC.behindTiles = false;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
				new FlavorTextBestiaryInfoElement("A furious chain of meteorite heads who's heat melted their bodies together, they writhe and slither as a burning serpent, attacking those who come too close.")
            });
        }

        public override void Init()
        {
            MinSegmentLength = 15;
            MaxSegmentLength = 15;

            CommonWormInit(this);
        }

        internal static void CommonWormInit(Worm worm)
        {
            worm.MoveSpeed = 7.5f;
            worm.Acceleration = 0.1f;
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--;
                }

                Player target = Main.player[NPC.target];
                if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target.Center) < 200 && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
                {
                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity * 1f, ModContent.ProjectileType<Flamethrower>(), 25, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 300;
                    attackCounter = 250;
                    NPC.netUpdate = true;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Meteor.Chance * 0.2f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeteorDust>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.Meteorite, 50, 1, 3));
        }
    }

    internal class MeteorSerpentBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Serpent");

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.Confused,
                    BuffID.Poisoned,
                    BuffID.OnFire3
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SeekerBody);
            NPC.lifeMax = 175;
            NPC.npcSlots = 0f;
            NPC.aiStyle = -1;
            NPC.damage = 12;
            NPC.defense = 12;
            NPC.behindTiles = false;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
        }

        public override void Init()
        {
            MeteorSerpentHead.CommonWormInit(this);
        }
    }

    internal class MeteorSerpentBodyAlt : WormBodyAlt
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Serpent");

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.Confused,
                    BuffID.Poisoned,
                    BuffID.OnFire3
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SeekerBody);
            NPC.lifeMax = 175;
            NPC.npcSlots = 0f;
            NPC.aiStyle = -1;
            NPC.damage = 12;
            NPC.defense = 12;
            NPC.behindTiles = false;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
        }

        public override void Init()
        {
            MeteorSerpentHead.CommonWormInit(this);
        }
    }

    internal class MeteorSerpentTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Serpent");

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.Confused,
                    BuffID.Poisoned,
                    BuffID.OnFire3
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SeekerTail);
            NPC.npcSlots = 0f;
            NPC.aiStyle = -1;
            NPC.damage = 12;
            NPC.behindTiles = false;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
        }

        public override void Init()
        {
            MeteorSerpentHead.CommonWormInit(this);
        }
    }
}
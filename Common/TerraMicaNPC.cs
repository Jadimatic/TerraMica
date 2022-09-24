using Microsoft.Xna.Framework;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace TerraMica.Common
{
    public class TerraMicaNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hellishRebuke = false;
        public int lifeRegenExpectedLossPerSecond = -1;

        public override void ResetEffects(NPC npc)
        {
            hellishRebuke = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.oiled && hellishRebuke)
            {
                damage = 10;
                npc.lifeRegen -= 50;
                /*if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 50;
                if (lifeRegenExpectedLossPerSecond < 10)
                {
                    lifeRegenExpectedLossPerSecond = 10;
                }*/
            }
            if (hellishRebuke)
            {
                damage = 10;
                npc.lifeRegen -= 40;
                /*if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 40;
                if (lifeRegenExpectedLossPerSecond < 10)
                {
                    lifeRegenExpectedLossPerSecond = 10;
                }*/
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (hellishRebuke)
            {
                if (Main.rand.Next(4) < 3)
                {
                    Dust dust5 = Dust.NewDustDirect(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.Torch, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 3.5f);
                    dust5.noGravity = true;
                    dust5.velocity *= 1.8f;
                    dust5.velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        dust5.noGravity = false;
                        dust5.scale *= 0.5f;
                    }
                }
                Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
            }
        }
    }
}

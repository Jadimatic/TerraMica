using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;
using TerraMica.Content.Buffs.DoT;
using TerraMica.Content.Projectiles.Weapons;
using Terraria.DataStructures;

namespace TerraMica.Common
{
    public class TerraMicaPlayer : ModPlayer
    {
        public bool vanillaLance;
        public bool betterOiled;
        public bool overHeated;
        public bool hellishRebuke;
        public bool stickyFingers;
        public int starDustTimer = 20;
        public bool bambooSet;
        public int lifeRegenExpectedLossPerSecond = -1;

        // The fields related to the Kerosene Lance
        public int chargeStorage;

        public override void UpdateDead()
        {
            vanillaLance = false;
            betterOiled = false;
            overHeated = false;
            hellishRebuke = false;
            stickyFingers = false;
            bambooSet = false;
        }

        public override void ResetEffects()
        {
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            vanillaLance = false;
            betterOiled = false;
            overHeated = false;
            hellishRebuke = false;
            stickyFingers = false;
            bambooSet = false;
        }

        /*public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            int Copper = ModContent.ItemType<CopperheadSpear>();// Change this to your item
            int Bloodstained = ModContent.ItemType<BloodstainedJoustingLance>();
            int Laser = ModContent.ItemType<LaserLanceCannon>();
            int Kerosene = ModContent.ItemType<KeroseneStaff>();
            int Example = ModContent.ItemType<ExampleSword>();
            if (Player.inventory[Player.selectedItem].type == Copper && !stickyFingers || Player.inventory[Player.selectedItem].type == Bloodstained && !stickyFingers || Player.inventory[Player.selectedItem].type == Laser && !stickyFingers || Player.inventory[Player.selectedItem].type == Kerosene && !overHeated || Player.inventory[Player.selectedItem].type == Kerosene && !stickyFingers || Player.inventory[Player.selectedItem].type == Example && !stickyFingers)
            {
                Player.channel = false;
                Player.itemAnimation = 0;
                Player.itemAnimationMax = 0;
                item.InterruptChannelOnHurt = true;
                item.StopAnimationOnHurt = true;
            }
        }*/

        public override void UpdateLifeRegen()
        {
            if (betterOiled && (Player.onFire || Player.onFire2 || Player.onFire3 || Player.onFrostBurn || Player.onFrostBurn2 || hellishRebuke))
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegen -= 50;
                if (lifeRegenExpectedLossPerSecond < 10)
                {
                    lifeRegenExpectedLossPerSecond = 10;
                }
            }
            if (hellishRebuke)
            {
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                // Player.lifeRegenTime uses to increase the speed at which the player reaches its maximum natural life regeneration
                // So we set it to 0, and while this debuff is active, it never reaches it
                Player.lifeRegenTime = 0;
                // lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second
                Player.lifeRegen -= 40;
                lifeRegenExpectedLossPerSecond = 5;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (hellishRebuke)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, DustID.Torch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    drawInfo.DustCache.Add(dust);
                }
                r *= 1f;
                g *= 0.3f;
                b *= 0.1f;
                fullBright = true;
            }
            if (betterOiled && !Main.rand.NextBool(3)) // Please re-do this if you have time so it's more like the one above lol
            {
                int num = 175;
                Color newColor = new(0, 0, 0, 140);
                Vector2 vector = Player.position;
                vector.X -= 2f;
                vector.Y -= 2f;
                if (Main.rand.NextBool(2))
                {
                    Dust dust8 = Dust.NewDustDirect(vector, Player.width + 4, Player.height + 2, DustID.TintableDust, 0f, 0f, num, newColor, 1.4f);
                    if (Main.rand.NextBool(2))
                    {
                        dust8.alpha += 25;
                    }
                    if (Main.rand.NextBool(2))
                    {
                        dust8.alpha += 25;
                    }
                    dust8.noLight = true;
                    dust8.velocity *= 0.2f;
                    dust8.velocity.Y += 0.2f;
                    dust8.velocity += Player.velocity;
                }
            }
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if ((item.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && bambooSet && Main.rand.NextBool(1 + (1 / 2)))
                target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(3, 7), false);
            if ((item.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated)
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if ((item.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && bambooSet && Main.rand.NextBool(1 + (1 / 2)))
                target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(3, 7), false);
            if ((item.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated)
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if ((proj.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && bambooSet && Main.rand.NextBool(1 + (1 / 2)))
                target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(3, 7), false);
            if ((proj.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated && !(proj.type == ModContent.ProjectileType<JetFuelGhost>()))
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            if ((proj.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && bambooSet && Main.rand.NextBool(1 + (1 / 2)))
                target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(3, 7), false);
            if ((proj.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated && !(proj.type == ModContent.ProjectileType<JetFuelGhost>()))
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
    }
}
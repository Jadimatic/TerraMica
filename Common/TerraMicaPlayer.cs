using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TerraMica.Content.Buffs.Misc;
using TerraMica.Content.Items.Accessories;
using TerraMica.Content.Items.Weapons;
using Terraria;
using Terraria.ModLoader;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using static Terraria.ModLoader.PlayerDrawLayer;
using TerraMica.Content.Buffs.DoT;
using TerraMica.Content.Projectiles.Weapons;
using Terraria.GameInput;

namespace TerraMica.Common
{
    public class TerraMicaPlayer : ModPlayer
    {
        public bool vanillaLance;
        public bool betterOiled;
        public bool overHeated;
        public bool hellishRebuke;
        public bool stickyFingers;
        public bool aeroGel;
        public int lifeRegenExpectedLossPerSecond = -1;
        // These indicate what direction is what in the timer arrays used
        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public const int DashCooldown = 50; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int DashDuration = 35; // Duration of the dash afterimage effect in frames

        // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public const float DashVelocity = 8f;

        // The direction the player has double tapped.  Defaults to -1 for no dash double tap
        public int DashDir = -1;

        // The fields related to the dash accessory
        public bool rusticShield;
        public int DashDelay = 0; // frames remaining till we can dash again
        public int DashTimer = 0; // frames remaining in the dash
        public bool DashHit = false; // if contact is made

        public override void UpdateDead()
        {
            vanillaLance = false;
            betterOiled = false;
            overHeated = false;
            hellishRebuke = false;
            stickyFingers = false;
            aeroGel = false;
        }

        public override void ResetEffects()
        {
            vanillaLance = false;
            betterOiled = false;
            overHeated = false;
            hellishRebuke = false;
            stickyFingers = false;
            aeroGel = false;
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            rusticShield = false;

            // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
            // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
            {
                DashDir = DashDown;
            }
            else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
            {
                DashDir = DashUp;
            }
            else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                DashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                DashDir = DashLeft;
            }
            else
            {
                DashDir = -1;
            }
        }

        // This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
        // If they double tapped this frame, they'll move fast this frame
        public override void PreUpdateMovement()
        {
            // if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
            if (CanUseDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashUp when Player.velocity.Y > -DashVelocity:
                    case DashDown when Player.velocity.Y < DashVelocity:
                        {
                            // Y-velocity is set here
                            // If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                            // This adjustment is roughly 1.3x the intended dash velocity
                            float dashDirection = DashDir == DashDown ? 1 : -1.3f;
                            newVelocity.Y = dashDirection * DashVelocity / 1.5f;
                            break;
                        }
                    case DashLeft when Player.velocity.X > -DashVelocity:
                    case DashRight when Player.velocity.X < DashVelocity:
                        {
                            // X-velocity is set here
                            float dashDirection = DashDir == DashRight ? 1 : -1;
                            newVelocity.X = dashDirection * DashVelocity;
                            break;
                        }
                    default:
                        return; // not moving fast enough, so don't start our dash
                }

                // start our dash
                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;

                // Here you'd be able to set an effect that happens when the dash first activates
                // Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
            }

            if (DashDelay > 0)
                DashDelay--;

            if (DashTimer > 0)
            { // dash is active
              // This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
              // Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
              // Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect

                Player.armorEffectDrawShadowEOCShield = true;

                // count down frames remaining
                DashTimer--;
                Player.eocDash = DashTimer;
            }
        }

        public override void PostUpdate()
        {
            if (DashTimer > 0)
            {
                if (DashHit == false)
                {
                    Rectangle rectangle = new((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC nPC = Main.npc[i];
                        if (!nPC.active || nPC.dontTakeDamage || nPC.friendly || (nPC.aiStyle == 112 && !(nPC.ai[2] <= 1f)) || !Player.CanNPCBeHitByPlayerOrPlayerProjectile(nPC))
                        {
                            continue;
                        }
                        Rectangle rect = nPC.getRect();
                        if (rectangle.Intersects(rect) && (nPC.noTileCollide || Player.CanHit(nPC)))
                        {
                            float num = Player.GetTotalDamage(DamageClass.Melee).ApplyTo(8);
                            float num12 = Player.GetTotalKnockback(DamageClass.Melee).ApplyTo(9f);
                            bool crit = false;
                            if ((float)Main.rand.Next(100) < Player.GetTotalCritChance(DamageClass.Melee))
                            {
                                crit = true;
                            }
                            int num20 = Player.direction;
                            if (Player.velocity.X < 0f)
                            {
                                num20 = -1;
                            }
                            if (Player.velocity.X > 0f)
                            {
                                num20 = 1;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.ApplyDamageToNPC(nPC, (int)num, num12, num20, crit);
                            }
                            DashTimer = 10;
                            DashDelay = 30;
                            Player.velocity.X *= -1f;
                            Player.velocity.Y *= -1f;
                            Player.GiveImmuneTimeForCollisionAttack(4);
                            DashHit = true;
                        }
                    }
                }
                else if ((!Player.controlLeft || !(Player.velocity.X < 0f)) && (!Player.controlRight || !(Player.velocity.X > 0f)))
                {
                    Player.velocity.X *= 0.95f; //slows player down after dash
                }
            }
            else
            {
                DashHit = false;
            }
        }

        private bool CanUseDash()
        {
            return rusticShield
                && Player.dashType == 0 // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                && !Player.setSolar // player isn't wearing solar armor
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
        public static void TryInterruptingItemUsage(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            bool flag = false;
            if (player.heldProj > -1 && Main.projectile[player.heldProj].IsInterruptible(player))
            {
                flag = true;
            }
            if (!flag)
            {
                return;
            }
            bool flag2 = false;
            if (PlayerInput.Triggers.Current.Hotbar1)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar2)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar3)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar4)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar5)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar6)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar7)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar8)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar9)
            {
                flag2 = true;
            }
            if (PlayerInput.Triggers.Current.Hotbar10)
            {
                flag2 = true;
            }
            if (flag2 && player.heldProj > -1)
            {
                Main.projectile[player.heldProj].Interrupt(player);
            }
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (Player.inventory[Player.selectedItem].type == ModContent.ItemType<CopperheadSpear>() && !stickyFingers || Player.inventory[Player.selectedItem].type == ModContent.ItemType<BloodstainedJoustingLance>() && !stickyFingers || Player.inventory[Player.selectedItem].type == ModContent.ItemType<LaserLanceCannon>() && !stickyFingers ||  Player.inventory[Player.selectedItem].type == ModContent.ItemType<KeroseneStaff>() && !stickyFingers || Player.inventory[Player.selectedItem].type == ModContent.ItemType<ExampleSword>() && !stickyFingers)
            {
                Player.channel = false;
                Player.itemAnimation = 0;
                Player.itemAnimationMax = 0;
            }
        }
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
        /*public override void UpdateBadLifeRegen()
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
        }*/
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
			if (hellishRebuke) 
            {
				if (Main.rand.NextBool(4) && drawInfo.shadow == 0f) {
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
            if ((item.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated)
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if ((item.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated)
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if ((proj.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated && !(proj.type == ModContent.ProjectileType<JetFuelGhost>()))
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            if ((proj.DamageType == ModContent.GetInstance<PiercingDamageClass>()) && overHeated && !(proj.type == ModContent.ProjectileType<JetFuelGhost>()))
                target.AddBuff(ModContent.BuffType<HellishRebuke>(), 60 * Main.rand.Next(3, 7), false);
        }
    }
}

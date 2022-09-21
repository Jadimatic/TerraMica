using IL.Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TerraMica.Content.Items.Accessories;
using TerraMica.Content.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace TerraMica.Common
{
    public class TerraMicaPlayer : ModPlayer
    {
        public bool moddedLances;
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
        public bool DashAccessoryEquipped;
        public int DashDelay = 0; // frames remaining till we can dash again
        public int DashTimer = 0; // frames remaining in the dash
        public bool DashHit = false; // if contact is made

        public override void UpdateDead()
        {
            moddedLances = false;
        }

        public override void ResetEffects()
        {
            moddedLances = false;
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            DashAccessoryEquipped = false;

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
                            float num = Player.GetTotalDamage(DamageClass.Melee).ApplyTo(ExampleShield.Base_Damage);
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
            return DashAccessoryEquipped
                && Player.dashType == 0 // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                && !Player.setSolar // player isn't wearing solar armor
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            //int lanceWeapons = TerraMicaLists.ModdedLanceItems;
            //if (Player.inventory[Player.selectedItem].type = lanceWeapons)
            int Copper = ModContent.ItemType<CopperheadSpear>();// Change this to your item
            int Bloodstained = ModContent.ItemType<BloodstainedJoustingLance>();
            int Laser = ModContent.ItemType<LaserLanceCannon>();
            int Example = ModContent.ItemType<ExampleSword>();

            if (Player.inventory[Player.selectedItem].type == Copper || Player.inventory[Player.selectedItem].type == Bloodstained || Player.inventory[Player.selectedItem].type == Laser || Player.inventory[Player.selectedItem].type == Example)
            {
                Player.channel = false;
                Player.itemAnimation = 0;
                Player.itemAnimationMax = 0;
            }
        }
    }
}

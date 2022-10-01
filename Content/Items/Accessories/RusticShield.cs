using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using TerraMica.Common;

namespace TerraMica.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
    public class RusticShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rustic Shield");
            Tooltip.SetDefault("Allows the player to dash into the enemy\nDouble tap a direction");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(20, 0));
            Item.accessory = true;
            Item.defense = 1;
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.damage = 8;
            Item.knockBack = 9f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RusticShieldPlayer>().rusticShield = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 1);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    public class RusticShieldPlayer : ModPlayer
    {
        public const int rusticDashCooldown = 50; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int rusticDashDuration = 35; // Duration of the dash afterimage effect in frames

        // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public const float rusticDashVelocity = 8f;

        // The fields related to the dash accessory
        public bool rusticShield;
        public int rusticDashDelay = 0; // frames remaining till we can dash again
        public int rusticDashTimer = 0; // frames remaining in the dash
        public bool rusticDashHit = false; // if contact is made

        // The direction the player has double tapped.  Defaults to -1 for no dash double tap
        public int DashDir = -1;
        // These indicate what direction is what in the timer arrays used
        public const int DashRight = 2;
        public const int DashLeft = 3;
        public override void ResetEffects()
        {
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            rusticShield = false;

            // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
            // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
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
            if (CanUseDash() && DashDir != -1 && rusticDashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashLeft when Player.velocity.X > -rusticDashVelocity:
                    case DashRight when Player.velocity.X < rusticDashVelocity:
                        {
                            // X-velocity is set here
                            float dashDirection = DashDir == DashRight ? 1 : -1;
                            newVelocity.X = dashDirection * rusticDashVelocity;
                            break;
                        }
                    default:
                        return; // not moving fast enough, so don't start our dash
                }

                // start our dash
                rusticDashDelay = rusticDashCooldown;
                rusticDashTimer = rusticDashDuration;
                Player.velocity = newVelocity;

                // Here you'd be able to set an effect that happens when the dash first activates
                // Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
            }

            if (rusticDashDelay > 0)
                rusticDashDelay--;

            if (rusticDashTimer > 0)
            { // dash is active
              // This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
              // Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
              // Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect

                Player.armorEffectDrawShadowEOCShield = true;

                // count down frames remaining
                rusticDashTimer--;
                Player.eocDash = rusticDashTimer;
            }
        }

        public override void PostUpdate()
        {
            if (rusticDashTimer > 0 && rusticShield)
            {
                if (rusticDashHit == false)
                {
                    Rectangle rectangle = new((int)(Player.position.X + Player.velocity.X * 0.5 - 4.0), (int)(Player.position.Y + Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
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
                            rusticDashTimer = 10;
                            rusticDashDelay = 30;
                            Player.velocity.X *= -1f;
                            Player.velocity.Y *= -1f;
                            Player.GiveImmuneTimeForCollisionAttack(4);
                            rusticDashHit = true;
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
                rusticDashHit = false;
            }
        }

        private bool CanUseDash()
        {
            return rusticShield
                && Player.dashType == 0 // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                && !Player.setSolar // player isn't wearing solar armor
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
    }
}
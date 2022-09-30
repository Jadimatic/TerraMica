using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Back, EquipType.Front)]
    public class AstralweaveMantle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astralweave Mantle");
            Tooltip.SetDefault("Allows the player to magically float up, left, or right\nWhile floating, you are granted temporary immunity\nDouble tap a direction to float");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(45));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AstralWeavePlayer>().astralWeave = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
    public class AstralWeavePlayer : ModPlayer
    {
        public bool astralWeave;
        public const float astralWeaveDashVelocity = 12.5f; // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public const int astralWeaveDashCooldown = (int)astralWeaveDashVelocity / 2 * 10; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int astralWeaveDashDuration = (int)astralWeaveDashVelocity / 2 * 7; // Duration of the dash afterimage effect in frames
        public int astralWeaveDelay = 0; // frames remaining till we can dash again
        public int astralWeaveTimer = 0; // frames remaining in the dash
        public int DashDir = -1; // The direction the player has double tapped.  Defaults to -1 for no dash double tap
        // These indicate what direction is what in the timer arrays used
        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public override void UpdateDead()
        {
            astralWeave = false;
        }

        public override void ResetEffects()
        {
            astralWeave = false;

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
            if (CanUseAstralweave() && DashDir != -1 && astralWeaveDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashLeft when Player.velocity.X > -astralWeaveDashVelocity:
                    case DashRight when Player.velocity.X < astralWeaveDashVelocity:
                        {
                            float dashDirection = DashDir == DashRight ? 1 : -1;
                            newVelocity.X = dashDirection * astralWeaveDashVelocity;
                            for (int index = 0; index < 20; ++index)
                            {
                                int starDust0 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.YellowStarDust, 0f, 0f, 0, default, 1.5f);
                                Main.dust[starDust0].scale *= 1f;
                                Main.dust[starDust0].position.X = Main.dust[starDust0].position.X + Main.rand.Next(-5, 6);
                                Main.dust[starDust0].position.Y = Main.dust[starDust0].position.Y + Main.rand.Next(-5, 6);
                                Main.dust[starDust0].velocity *= 0.2f;
                                Main.dust[starDust0].scale *= 1f + Main.rand.Next(20) * 0.01f;
                                Main.dust[starDust0].noGravity = true;
                            }
                            SoundEngine.PlaySound(SoundID.DoubleJump, Player.position);
                            SoundEngine.PlaySound(SoundID.Item4, Player.position);
                            break;
                        }
                    case DashUp when Player.velocity.Y > -astralWeaveDashVelocity:
                        {
                            // Y-velocity is set here
                            // If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                            // This adjustment is roughly 1.3x the intended dash velocity
                            float dashDirection = DashDir == DashDown ? 1 : -1.3f;
                            newVelocity.Y = dashDirection * astralWeaveDashVelocity / 1.5f;
                            Player.fallStart = (int)(Player.position.Y / 16f);
                            int num = Player.height;
                            if (Player.gravDir == -1f) // The following code is adapted from vanilla, so it may be messy
                            {
                                num = -6;
                            }
                            int starDust = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)num), Player.width + 8, 4, DustID.YellowStarDust, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 100, default, 1.5f);
                            Main.dust[starDust].velocity.X = Main.dust[starDust].velocity.X * 0.5f - Player.velocity.X * 0.1f;
                            Main.dust[starDust].velocity.Y = Main.dust[starDust].velocity.Y * 0.5f - Player.velocity.Y * 0.3f;
                            int num2 = Player.height - 6;
                            if (Player.gravDir == -1f)
                            {
                                num2 = 6;
                            }
                            for (int a = 0; a < 2; a++)
                            {
                                int starDust2 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)num2), Player.width, 12, DustID.YellowStarDust, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f);
                                Main.dust[starDust2].velocity *= 0.1f;
                                if (a == 0)
                                {
                                    Main.dust[starDust2].velocity += Player.velocity * 0.03f;
                                }
                                else
                                {
                                    Main.dust[starDust2].velocity -= Player.velocity * 0.03f;
                                }
                                Main.dust[starDust2].velocity -= Player.velocity * 0.1f;
                                Main.dust[starDust2].noGravity = true;
                            }
                            for (int b = 0; b < 3; b++)
                            {
                                int starDust3 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)num2), Player.width, 12, DustID.YellowStarDust, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f);
                                Main.dust[starDust3].fadeIn = 1.5f;
                                Main.dust[starDust3].velocity *= 0.6f;
                                Main.dust[starDust3].velocity += Player.velocity * 0.8f;
                                Main.dust[starDust3].noGravity = true;
                            }
                            for (int c = 0; c < 3; c++)
                            {
                                int starDust4 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)num2), Player.width, 12, DustID.YellowStarDust, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f);
                                Main.dust[starDust4].fadeIn = 1.5f;
                                Main.dust[starDust4].velocity *= 0.6f;
                                Main.dust[starDust4].velocity -= Player.velocity * 0.8f;
                                Main.dust[starDust4].noGravity = true;
                            }
                            SoundEngine.PlaySound(SoundID.DoubleJump, Player.position);
                            SoundEngine.PlaySound(SoundID.Item4, Player.position);
                            break;
                        }
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    /*case DashRight when Player.velocity.X < astralWeaveDashVelocity:
                    {
                        // X-velocity is set here
                        float dashDirection = DashDir == DashRight ? 1 : -1;
                        newVelocity.X = dashDirection * astralWeaveDashVelocity;
                        break;
                    }*/
                    default:
                        return; // not moving fast enough, so don't start our dash
                }

                // start our dash
                astralWeaveDelay = astralWeaveDashCooldown;
                astralWeaveTimer = astralWeaveDashDuration;
                Player.velocity = newVelocity;
            }

            if (astralWeaveDelay > 0)
                astralWeaveDelay--;

            if (astralWeaveTimer > 0)
            {
                Player.immune = true;
                Player.immuneTime = astralWeaveDashDuration - (astralWeaveDashDuration / 7 * 2); //Sets the immunity time to starSilkDashDuration minus about 30% of starSilkDashDuration.
                astralWeaveTimer--;
            }
        }

        public override void PostUpdate()
        {
            if (astralWeaveTimer > 0)
            {
                Player.armorEffectDrawShadowLokis = true;
            }
        }
        private bool CanUseAstralweave()
        {
            return astralWeave
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
    }
}
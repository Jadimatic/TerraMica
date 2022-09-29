using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.PlayerDrawLayer;
using TerraMica.Content.Buffs.Misc;
using static Humanizer.In;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Neck)]
    public class StarsilkScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starsilk Scarf");
            Tooltip.SetDefault("Allows the player to magically float upwards\nWhile floating, you are granted temporary immunity\nDouble tap UP to float");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(45));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StarSilkPlayer>().starSilk = true;
            if (player.HasBuff(ModContent.BuffType<AstralDislocation1>()))
            {
                player.statDefense = (int)(player.statDefense * 0.85f);
            }
            else if (player.HasBuff(ModContent.BuffType<AstralDislocation2>()))
            {
                player.statDefense = (int)(player.statDefense * 0.8f);
            }
            else if (player.HasBuff(ModContent.BuffType<AstralDislocation3>()))
            {
                player.statDefense = (int)(player.statDefense * 0.7f);
            }
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
    public class StarSilkPlayer : ModPlayer
    {
        public bool starSilk;
        public int starDustTimer = 20;
        public const int starSilkDashCooldown = (int)starSilkDashVelocity / 2 * 10; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int starSilkDashDuration = (int)starSilkDashVelocity / 2 * 7; // Duration of the dash afterimage effect in frames
        public int starSilkDelay = 0; // frames remaining till we can dash again
        public int starSilkTimer = 0; // frames remaining in the dash
        public const float starSilkDashVelocity = 12.5f; // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public int DashDir = -1; // The direction the player has double tapped.  Defaults to -1 for no dash double tap
        // These indicate what direction is what in the timer arrays used
        public const int DashDown = 0;
        public const int DashUp = 1;

        public override void UpdateDead()
        {
            starSilk = false;
        }

        public override void ResetEffects()
        {
            starSilk = false;

            // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
            // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
            {
                DashDir = DashUp;
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
            if (CanUseStarsilk() && DashDir != -1 && starSilkDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashUp when Player.velocity.Y > -starSilkDashVelocity:
                        {
                            // Y-velocity is set here
                            // If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                            // This adjustment is roughly 1.3x the intended dash velocity
                            float dashDirection = DashDir == DashDown ? 1 : -1.3f;
                            newVelocity.Y = dashDirection * starSilkDashVelocity / 1.5f;
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
                    default:
                        return; // not moving fast enough, so don't start our dash
                }
                // start our dash
                starSilkDelay = starSilkDashCooldown;
                starSilkTimer = starSilkDashDuration;
                Player.velocity = newVelocity;
            }

            if (starSilkDelay > 0)
                starSilkDelay--;

            if (starSilkTimer > 0)
            {
                Player.immune = true;
                Player.immuneTime = starSilkDashDuration - (starSilkDashDuration / 7 * 2); //Sets the immunity time to starSilkDashDuration minus about 30% of starSilkDashDuration.
                //Player.armorEffectDrawShadowEOCShield = true;
                Player.armorEffectDrawOutlines = true;
                Player.eocDash = starSilkTimer;
                starSilkTimer--;
                if (!Player.buffImmune[ModContent.BuffType<AstralDislocationCheck>()])
                {
                    Player.AddBuff(ModContent.BuffType<AstralDislocation1>(), 300);
                }
                else if (Player.HasBuff(ModContent.BuffType<AstralDislocation1>()) && DashDir != -1)
                {
                    Player.AddBuff(ModContent.BuffType<AstralDislocation2>(), 300);
                }
                if (Player.HasBuff(ModContent.BuffType<AstralDislocation2>()) && Player.buffImmune[ModContent.BuffType<AstralDislocation1>()] && DashDir != -1)
                {
                    Player.AddBuff(ModContent.BuffType<AstralDislocation3>(), 300);
                }
            }
        }

        private bool CanUseStarsilk()
        {
            return starSilk
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
    }
}
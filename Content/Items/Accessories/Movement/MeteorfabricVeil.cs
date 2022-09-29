using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Front, EquipType.Back)]
    public class MeteorfabricVeil : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteorfabric Veil");
            Tooltip.SetDefault("Allows the player to magically float left or right\nWhile floating, you are granted temporary immunity\nDouble tap a direction to float");
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
            player.GetModPlayer<MeteorFabricPlayer>().meteorFabric = true;
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
    public class MeteorFabricPlayer : ModPlayer
    {
        public bool meteorFabric;
        public const float meteorFabricDashVelocity = 12.5f; // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public const int meteorFabricDashCooldown = (int)meteorFabricDashVelocity / 2 * 10; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int meteorFabricDashDuration = (int)meteorFabricDashVelocity / 2 * 7; // Duration of the dash afterimage effect in frames
        public int meteorFabricDelay = 0; // frames remaining till we can dash again
        public int meteorFabricTimer = 0; // frames remaining in the dash
        public int DashDir = -1; // The direction the player has double tapped.  Defaults to -1 for no dash double tap
        // These indicate what direction is what in the timer arrays used
        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public override void UpdateDead()
        {
            meteorFabric = false;
        }

        public override void ResetEffects()
        {
            meteorFabric = false;

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
            if (CanUsemeteorFabric() && DashDir != -1 && meteorFabricDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashLeft when Player.velocity.X > -meteorFabricDashVelocity:
                    case DashRight when Player.velocity.X < meteorFabricDashVelocity:
                        {
                            float dashDirection = DashDir == DashRight ? 1 : -1;
                            newVelocity.X = dashDirection * meteorFabricDashVelocity;
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
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    /*case DashRight when Player.velocity.X < meteorFabricDashVelocity:
                    {
                        // X-velocity is set here
                        float dashDirection = DashDir == DashRight ? 1 : -1;
                        newVelocity.X = dashDirection * meteorFabricDashVelocity;
                        break;
                    }*/
                    default:
                        return; // not moving fast enough, so don't start our dash
                }

                // start our dash
                meteorFabricDelay = meteorFabricDashCooldown;
                meteorFabricTimer = meteorFabricDashDuration;
                Player.velocity = newVelocity;
            }

            if (meteorFabricDelay > 0)
                meteorFabricDelay--;

            if (meteorFabricTimer > 0)
            {
                Player.immune = true;
                Player.immuneTime = meteorFabricDashDuration - (meteorFabricDashDuration / 7 * 2); //Sets the immunity time to starSilkDashDuration minus about 30% of starSilkDashDuration.
                //Player.armorEffectDrawShadowEOCShield = true;
                Player.armorEffectDrawOutlines = true;
                Player.eocDash = meteorFabricTimer;
                meteorFabricTimer--;
            }
        }

        private bool CanUsemeteorFabric()
        {
            return meteorFabric
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
    }
}
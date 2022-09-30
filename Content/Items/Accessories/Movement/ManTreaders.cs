using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using TerraMica.Common;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shoes)]
    public class ManTreaders : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Man Treaders");
            Tooltip.SetDefault("Allows the player to stomp enemies\nDouble tap DOWN to stomp");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(10);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PiercingDamageClass>();
            Item.damage = 15;
            Item.knockBack = 1f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ManTreadersPlayer>().manTreads = true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    public class ManTreadersPlayer : ModPlayer
    {
        // The fields related to the Man Treaders accessory
        public bool manTreads;
        public int manDelay = 0; // frames remaining till we can man again
        public int manTimer = 0; // frames remaining in the man
        public bool manHit = false; // if contact is made
        public const int manCooldown = 50; // Time (frames) between starting manes. If this is shorter than manDuration you can start a new man before an old one has finished
        public const int manDuration = 35; // Duration of the man afterimage effect in frames

        // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public const float manVelocity = 15f;

        // The direction the player has double tapped.  Defaults to -1 for no dash double tap
        public int DashDir = -1;
        // These indicate what direction is what in the timer arrays used
        public const int DashDown = 0;

        public override void UpdateDead()
        {
            manTreads = false;
        }

        public override void ResetEffects()
        {
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            manTreads = false;

            // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
            // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
            {
                DashDir = DashDown;
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
            if (CanUseTreads() && DashDir != -1 && manDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the man velocity if our current speed in the wanted direction is less than manVelocity
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashDown when Player.velocity.Y < manVelocity:
                        {
                            // Y-velocity is set here
                            // If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                            // This adjustment is roughly 1.3x the intended dash velocity
                            float dashDirection = DashDir == DashDown ? 1 : -1.3f;
                            newVelocity.Y = dashDirection * manVelocity / 1.5f;
                            break;
                        }
                    default:
                        return; // not moving fast enough, so don't start our dash
                }

                // start our man
                manDelay = manCooldown;
                manTimer = manDuration;
                Player.velocity = newVelocity;

                // Here you'd be able to set an effect that happens when the man first activates
                // Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
            }

            if (manDelay > 0)
                manDelay--;

            if (manTimer > 0)
            {
                manTimer--;
            }
        }

        public override void PostUpdate()
        {
            if (manTimer > 0 && Player.velocity.Y != 0)
            {
                Player.armorEffectDrawShadowLokis = true;
                if (manHit == false)
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
                            float num = Player.GetTotalDamage(ModContent.GetInstance<PiercingDamageClass>()).ApplyTo(25f);
                            float num12 = Player.GetTotalKnockback(DamageClass.Melee).ApplyTo(1f);
                            bool crit = false;
                            if ((float)Main.rand.Next(100) < Player.GetTotalCritChance(ModContent.GetInstance<PiercingDamageClass>()))
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
                            manTimer = 10;
                            manDelay = 30;
                            Player.velocity.X *= -0.85f;
                            Player.velocity.Y *= -0.85f;
                            Player.GiveImmuneTimeForCollisionAttack(4);
                            manHit = true;
                        }
                    }
                }
            }
            else
            {
                manHit = false;
            }
        }

        private bool CanUseTreads()
        {
            return manTreads
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
    }
}
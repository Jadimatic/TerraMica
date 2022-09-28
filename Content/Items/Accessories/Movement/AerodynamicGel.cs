using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Audio;

namespace TerraMica.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shoes)]
    public class AerodynamicGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sudsy Soap");
            Tooltip.SetDefault("Increases piercing damage done while moving\n3% increased movement speed\nMakes your feet soapy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(45));
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AeroGelPlayer>().aeroGel = true;
            player.maxRunSpeed *= 1.03f;
            player.slippy = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.AddIngredient(ItemID.Cloud, 3);
            recipe.AddTile(TileID.Solidifier);
            recipe.AddCondition(Recipe.Condition.NearWater);
            recipe.Register();
        }
    }

    public class AeroGelPlayer : ModPlayer
    {
        public bool aeroGel;
        public int bubbleTimer = 20;
        public int runSoundTimer = 9;
        public override void UpdateDead()
        {
            aeroGel = false;
        }
        public override void ResetEffects()
        {
            aeroGel = false;
        }
        public override void PreUpdate()
        {
            if (aeroGel && Player.velocity.X != 0 && Player.velocity.Y == 0)
            {
                int num = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)0), Player.width + 8, 4, DustID.Cloud, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.5f);
                Main.dust[num].velocity.X = Main.dust[num].velocity.X * 0.2f;
                Main.dust[num].velocity.Y = Main.dust[num].velocity.Y * 0.2f;
                Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                if (runSoundTimer > 0)
                {
                    runSoundTimer--;
                }
                if (runSoundTimer <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Run, Player.position);
                    runSoundTimer = 9;
                }
                if (bubbleTimer > 0)
                {
                    bubbleTimer--;
                }
                if (bubbleTimer <= 0)
                {
                    SoundEngine.PlaySound(Main.rand.NextBool() ? SoundID.Item54 : SoundID.Item85, Player.position);
                    bubbleTimer = 20;
                }
            }
        }
    }
}
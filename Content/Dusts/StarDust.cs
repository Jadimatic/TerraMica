using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraMica.Content.Dusts
{
    public class StarDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            Dust.CloneDust(DustID.YellowStarDust);
        }
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 12, 8);
        }
    }
}
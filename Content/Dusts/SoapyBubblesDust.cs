﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerraMica.Content.Dusts
{
    public class SoapyBubblesDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 20, 20);
            dust.scale = 1f;
            dust.alpha = 75;
            dust.velocity *= 0.1f;
        }
        public override bool Update(Dust dust)
        {
            dust.noGravity = true;
            dust.position += dust.velocity;
            dust.scale *= 0.99f;
            dust.alpha += 8;
            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
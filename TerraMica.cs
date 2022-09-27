using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.ModLoader;
using TerraMica.Common;

namespace TerraMica
{
	public class TerraMica : Mod
	{
		public const string ASSET_PATH = "TerraMica/Assets/";

        public override void Load()
        {
            TerraMicaLists.LoadLists();
        }

        public override void Unload()
        {
            TerraMicaLists.UnloadLists();
        }
    }
}
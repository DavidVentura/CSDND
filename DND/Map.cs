using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace DND
{
	static class Map
	{
		static int height, width;
		static Texture2D auxtext;
		static List<MapLayer> layers = new List<MapLayer> ();

		public static List<MapLayer> Layers {
			get { return layers; }
		}

		static int text_tile;

		public static void Initialize (int w, int h)
		{
			width = w;
			height = h;
		}

		public static void AddLayer (MapLayer layer)
		{
			layers.Add (layer);//todo check
		}

		public static void Draw (ref SpriteBatch sb)
		{
			DrawLayer (ref sb, layers [0], false);
			DrawLayer (ref sb, layers [1], true);

		}

		static void DrawLayer (ref SpriteBatch sb, MapLayer layer, bool drawPlayers)
		{
			int y = 0, x = 0, xpos = 0, ypos;
			for (y = 0; y < height; y++)
				for (x = 0; x < width; x++) {
					text_tile = layer.TileAt (x, y).TextureNumber;
					if (text_tile > 0) { //"empty"
						auxtext = TextureManager.getTexture (text_tile);
						xpos = x * Engine.TileWidth - (int)Camera.Position.X;
						if (!layer.isGround) 
							xpos -= (auxtext.Width - Engine.TileWidth) / 2;
						
						ypos = (y * Engine.TileHeight) - (int)Camera.Position.Y - (auxtext.Height - Engine.TileHeight);
						sb.Draw (auxtext, new Rectangle (xpos, ypos, auxtext.Width, auxtext.Height), Color.White);
					}
					if (drawPlayers)
						foreach (Player p in Engine.Players) {
							if (p.position.X == x && p.position.Y == y)
								p.Draw (sb);
						}
				}
		}

		public static bool withinBounds (Vector2 position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height)
				return false;
			return true;
		}

		public static bool ValidPosition (Vector2 position)
		{
			if (!withinBounds(position))
				return false;

			foreach(Player p in Engine.Players)
				if (p.position==position && !p.isLocal)
					return false;

			foreach(MapLayer l in Layers)
				if (l.isBlocking && l.TileAt(position).TextureNumber>0)
					return false;
			return true;

		}

        
	}
}

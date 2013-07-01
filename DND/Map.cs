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
		static Texture2D auxtext;
		static int height, width;
		static MapLayer GroundLayer;
		static MapLayer ObjectLayer;
		static int text_tile,y = 0, x = 0, xpos = 0, ypos;

		public static void Initialize (int w, int h)
		{
			width = w;
			height = h;
		}

		public static void Draw (ref SpriteBatch sb)
		{
			if(GroundLayer==null||ObjectLayer==null)return;

			DrawLayer (ref sb, GroundLayer);
			DrawLayer (ref sb, ObjectLayer);

		}

		static void DrawLayer (ref SpriteBatch sb, MapLayer layer)
		{

			for (y = 0; y < height; y++)
				for (x = 0; x < width; x++) {
					text_tile = layer.TileAt (x, y).TextureNumber;
					if (text_tile > 0) { //not "empty"
						auxtext = TextureManager.getTexture (text_tile);
						if(auxtext==null) continue;
						xpos = x * Engine.TileWidth - Camera.Position.X;
						if (layer.Type!= LayerType.Ground) 
							xpos -= (auxtext.Width - Engine.TileWidth) / 2;
						
						ypos = (y * Engine.TileHeight) - Camera.Position.Y - (auxtext.Height - Engine.TileHeight);
						sb.Draw (auxtext, new Rectangle (xpos, ypos, auxtext.Width, auxtext.Height), Color.White);
					}
					if (layer.Type==LayerType.Object){
						foreach (Player p in Engine.Players)
							if (p.position.X == x && p.position.Y == y)
								p.Draw (sb);
						if(Engine.LocalPlayer.position.X ==x && Engine.LocalPlayer.position.Y==y)
							Engine.LocalPlayer.Draw(sb);
					}
				}
		}
		public static bool withinBounds (Coord position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height)
				return false;
			return true;
		}

		public static void AddLayer (MapLayer mapLayer)
		{
			switch (mapLayer.type) {
			case LayerType.Ground:
				GroundLayer=mapLayer;
				break;
			case LayerType.Object:
				ObjectLayer=mapLayer;
				break;
			}
		}		

        
	}
}

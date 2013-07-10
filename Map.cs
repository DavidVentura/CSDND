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

		public static void Draw (SpriteBatch sb)
		{
			if(GroundLayer==null||ObjectLayer==null)return;

			DrawLayer (sb, GroundLayer);
			DrawLayer (sb, ObjectLayer);

		}

		static void DrawLayer (SpriteBatch sb, MapLayer layer)
		{
			int minX, minY, maxX, maxY;
			if (!Engine.isDM || Engine.CurPlayer!=null) {
				minX = Engine.CurPlayer.Position.X - Engine.CurPlayer.VisionRange;
				minY = Engine.CurPlayer.Position.Y - Engine.CurPlayer.VisionRange;
				maxX = Engine.CurPlayer.VisionRange + Engine.CurPlayer.Position.X;
				maxY = Engine.CurPlayer.VisionRange + Engine.CurPlayer.Position.Y;
			} else {
				minX=0;
				minY=0;
				maxX=Map.width;
				maxY=Map.height;
			}
			for (y = minY; y < maxY; y++)
				for (x = minX; x < maxX; x++) {
					if ((!Engine.isDM||Engine.CurPlayer!=null)
						&& (Coord.Distance(new Coord(x,y), Engine.CurPlayer.Position) >= Engine.CurPlayer.VisionRange)) continue;
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
							if (p.visible)
								if (p.Position.X == x && p.Position.Y == y)
									p.Draw (sb);
						foreach (Player p in Engine.LocalPlayers)
						if(p.Position.X ==x && p.Position.Y==y)
							p.Draw(sb);
					}
				}
		}
		public static bool withinBounds (Coord position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height)
				return false;
			return true;
		}
		public static bool withinBounds (int X, int Y)
		{
			if (X < 0 || Y < 0 || X >= width || Y >= height)
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

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
		static int text_tile,y = 0, x = 0, xpos = 0, ypos,i,playercount;
		static List<Player> Players = new List<Player>();
		static object PlayerLock=new object();

		public const int MovementTime = 120;
        public const int TileHeight = 32;
        public const int TileWidth = 32;

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

		public static void Update (GameTime gameTime)
		{
			foreach (Player p in Players)
				if (p!=null)
					p.Update (gameTime);
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
						auxtext = TextureManager.getTexture (text_tile,layer.type);
						if(auxtext==null) continue;
						xpos = x * TileWidth - Camera.Position.X;
						if (layer.Type!= LayerType.Ground) 
							xpos -= (auxtext.Width - TileWidth) / 2;
						
						ypos = (y * TileHeight) - Camera.Position.Y - (auxtext.Height - TileHeight);
						sb.Draw (auxtext, new Rectangle (xpos, ypos, auxtext.Width, auxtext.Height), Color.White);
					}
					if (layer.Type==LayerType.Object){
						playercount=Players.Count;
						for (i=0; i<playercount;i++)
								if (Players[i].Position.X == x && Players[i].Position.Y == y)
									if (Players[i].isLocal || Players[i].visible)
										Players[i].Draw (sb);
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
		public static void Modify (LayerType type, int id, int x, int y)
		{
			switch (type) {
			case LayerType.Ground:
				GroundLayer.Modify(id,x,y);
				break;
			case LayerType.Object:
				ObjectLayer.Modify (id, x, y);
				break;
			}
		}


		public static void AddPlayer (int id, int x, int y, int sprite, string name, int size, int visionRange=0)
		{
			lock (PlayerLock) {
				foreach (Player p in Players)
					if (p.ID == id)
						return;
				TextureManager.addSprites (sprite);
				Players.Add (new Player (new Coord (x, y), sprite, id, name, size, visionRange));
			}
		}
		public static void MovePlayer (int id, int x, int y)
		{
			Coord newCoord = new Coord (x, y);
			Coord diff;
			foreach (Player p in Players)
				if (p.ID == id) {
					diff = newCoord - p.Position;
					if (diff.X > 0)
						p.animation.SwitchDirection (Direction.Right);
					if (diff.X < 0)
						p.animation.SwitchDirection (Direction.Left);
					if (diff.Y > 0)
						p.animation.SwitchDirection (Direction.Down);
					if (diff.Y < 0)
						p.animation.SwitchDirection (Direction.Up);
					p.Position = newCoord;
					return;
				}
		}
		public static void RemovePlayer (int i)
		{
			foreach (Player p in Players)
				if (p.ID == i) {
					Players.Remove (p);
					return;
				}
		}
		public static void ChangeVisibility (int id)
		{
			foreach (Player p in Players)
				if (p.ID == id) {
					p.visible = !p.visible;
					return;
				}
		}
		public static bool withinSight (Coord c)
		{
			if (Engine.isDM && Engine.CurPlayer==null) return true;
			return (Coord.Distance(c, Engine.CurPlayer.Position) < Engine.CurPlayer.VisionRange);
		}
		public static List<Player> GetLocalPlayers ()
		{
			List<Player> ret = new List<Player>();
			foreach (Player p in Players)
				if (p.isLocal) ret.Add (p);
			return ret;
		}

        
	}
}

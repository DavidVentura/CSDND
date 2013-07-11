using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DND
{
	public static class GUI
	{
		private static MouseState oldMouse;
		private static Coord MouseCoords;
		private static List<Coord> tiles2 = new List<Coord>();
		private static List<Coord> tiles = new List<Coord>();
		private static int radius=0;
		private static ButtonState lastRButtonState = ButtonState.Released;
		private static double lastKeyPress;
		public static void Initialize()
		{
			tiles.Add (new Coord (0, 0));
			tiles.Add (new Coord (0, 1));
			tiles.Add (new Coord (1, 0));
			tiles.Add (new Coord (1, 1));
		}

		public static void Update (GameTime gameTime)
		{


			oldMouse = Mouse.GetState ();
			MouseCoords = GetMouseMapCoord (oldMouse.X + Camera.Position.X, oldMouse.Y + Camera.Position.Y);
			double curTime = gameTime.TotalGameTime.TotalMilliseconds;
			if (curTime - lastKeyPress < Engine.MovementTime + 20)
				return;

			if (Keyboard.GetState ().IsKeyDown (Keys.Z)) {
				lastKeyPress = curTime;
				Network.SendData (String.Format ("SPWN{0},{1},{2}", 1, MouseCoords.X, MouseCoords.Y)); //spawn id 1.. interface
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.X)) {
				lastKeyPress = curTime;
				Network.SendData (String.Format ("SOBJ{0},{1},{2},{3}", 2, 1, MouseCoords.X, MouseCoords.Y)); //spawn obj interface
				//tileID,blocking,x,y
				return;
			}

			//TODO: interface
			//if (Engine.isDM) {
				if (Keyboard.GetState ().IsKeyDown (Keys.C)) {
					lastKeyPress = curTime;
					Engine.curCharIndex = 0;
					Engine.CurPlayer = null;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.I)) {
					lastKeyPress = curTime;
					Network.SendData ("INIT");
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.R)) {
					lastKeyPress = curTime;
					Network.SendData ("REFL");
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.F)) {
					lastKeyPress = curTime;
					Network.SendData ("FORT");
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.W)) {
					lastKeyPress = curTime;
					Network.SendData ("WILL");
				}
		//	}

			if (Keyboard.GetState ().IsKeyDown (Keys.Space)) {
				lastKeyPress = curTime;
				Network.SendData ("SWCH"); //switch character
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D1)) {
				radius=1;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D2)) {
				radius=2;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D3)) {
				radius=3;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D4)) {
				radius=4;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D5)) {
				radius=5;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D6)) {
				radius=6;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D0)) {
				radius=0;
				return;
			}

			if (Engine.CurPlayer == null) return;
			//No characters -> no movement
			if (Keyboard.GetState ().IsKeyDown (Keys.LeftShift)) {
				lastKeyPress = curTime;
				Network.SendData ("NOCL"); //noclip
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.LeftAlt)) {
				lastKeyPress = curTime;
				Network.SendData ("VISI"); //change visibility
				return;
			}

			if (Keyboard.GetState ().IsKeyDown (Keys.Right)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE1,0");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Left)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE-1,0");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Up)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE0,-1");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE0,1");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE0,1");
				return;
			}
			if (Mouse.GetState ().RightButton == ButtonState.Released && lastRButtonState == ButtonState.Pressed) {
				foreach (Player p in Engine.LocalPlayers)
					if (p.Position.Equals(MouseCoords)) {
					//TODO: switch to player
					}
			}
			lastRButtonState = Mouse.GetState ().RightButton;
			/*//TODO Interface
			if (Keyboard.GetState ().IsKeyDown (Keys.J)) {
				lastKeyPress = curTime;
				Network.SendData("TALK");
			}*/
		}

		public static void Draw (SpriteBatch sb)
		{
			if (MouseCoords.X >= 0) {
				if (radius > 0) {
					GetAOETiles ();
					foreach (Coord c in tiles2)
						sb.Draw (TextureManager.getTexture (998), new Rectangle (c.X * Engine.TileWidth - Camera.Position.X, c.Y * Engine.TileHeight - Camera.Position.Y, 32, 32), new Color (0, 0, 0, 200));
				} else
					sb.Draw (TextureManager.getTexture (999), GetMouseDrawRect (), Color.White);
			}
		}
		private static void GetAOETiles ()
		{
			tiles2.Clear ();
			if (radius == 1) {
				foreach (Coord c in tiles)
					tiles2.Add(MouseCoords+c);
				return;
			}
			bool invalido = false;
			int esquinas;
			Coord curCoord;
			for (int x =MouseCoords.X-(radius-1); x <= MouseCoords.X+radius; x++) {
				for (int y =MouseCoords.Y-(radius-1); y <= MouseCoords.Y+radius; y++) {
					curCoord = new Coord (x, y);
					invalido = false;
					foreach (Coord c in tiles) {
						if (Coord.Distance (c+ MouseCoords, curCoord) > radius * 1.12f) {
							invalido = true;
							break;
						}
					}
					if (!invalido){
						esquinas=0;
						foreach(Coord c in tiles)
							if (Coord.Distance(curCoord+c,MouseCoords+new Coord(1,1)) <= radius)
								esquinas++;
						if (esquinas>2)
							tiles2.Add (curCoord);
					}
				}
			}
		}
		private static Rectangle GetMouseDrawRect ()
		{
			return new Rectangle(MouseCoords.X*Engine.TileWidth - Camera.Position.X, MouseCoords.Y*Engine.TileHeight-Camera.Position.Y,Engine.TileWidth,Engine.TileHeight);
		}
		//TODO: Spells: 2x2 block A , add to the list L the tiles T that are within R distance of the furthest tile from A

		/// <summary>
		/// Gets the mouse map coordinate.
		/// </summary>
		/// <returns>
		/// The mouse map coordinate. Returns (-1,-1) if the coordinate is outside the map
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		private static Coord GetMouseMapCoord(int x, int y) {
			Coord ret = new Coord((x-(x%Engine.TileWidth))/Engine.TileWidth,(y-(y%Engine.TileHeight))/Engine.TileHeight);
			if (!Map.withinBounds(ret) || !Engine.withinSight(ret))
				return new Coord(-1,-1);
			return ret;
		}
	}
}


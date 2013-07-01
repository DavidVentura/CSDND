using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DND
{
	public static class GUI
	{
		private static MouseState oldMouse;
		private static Coord MouseCoords;
		private static string message="";

		private static double lastKeyPress;
		public static void Initialize()
		{

		}

		public static void Update(GameTime gameTime) {
			oldMouse=Mouse.GetState();
			MouseCoords=GetMouseMapCoord(oldMouse.X+Camera.Position.X,oldMouse.Y+Camera.Position.Y);
			double curTime = gameTime.TotalGameTime.TotalMilliseconds;
			if (curTime - lastKeyPress < 120)
				return;
			if (Keyboard.GetState ().IsKeyDown (Keys.Right)) {
				lastKeyPress = curTime;
				Network.SendData("MOVE1,0");

			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Left)) {
				lastKeyPress = curTime;
				Network.SendData("MOVE-1,0");
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Up)) {
				lastKeyPress = curTime;
				Network.SendData("MOVE0,-1");
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				lastKeyPress = curTime;
				Network.SendData("MOVE0,1");
			}
			foreach(Keys k in Keyboard.GetState().GetPressedKeys()){
				message+=k.ToString();
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.J)) {

				lastKeyPress = curTime;
				Network.SendData("TALK"+message);
				message="";
			}
		}

		public static void Draw(SpriteBatch sb) {
//			if (MouseCoords.X>=0)
//				sb.Draw (TextureManager.getTexture(999), GetMouseDrawRect(),Color.White);
		}
		private static Rectangle GetMouseDrawRect ()
		{
			return new Rectangle(MouseCoords.X*Engine.TileWidth - Camera.Position.X, MouseCoords.Y*Engine.TileHeight-Camera.Position.Y,Engine.TileWidth,Engine.TileHeight);
		}

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
			if (!Map.withinBounds(ret))
				return new Coord(-1,-1);
			return ret;
		}
	}
}


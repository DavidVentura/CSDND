using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DND
{
	public static class GUI
	{
		private static MouseState oldMouse;
		private static Vector2 MouseCoords;
		public static void Initialize()
		{

		}

		public static void Update(GameTime gameTime) {
			oldMouse=Mouse.GetState();
			MouseCoords=GetMouseMapCoord((int)(oldMouse.X+Camera.Position.X),(int)(oldMouse.Y+Camera.Position.Y));
		}

		public static void Draw(SpriteBatch sb) {
			if (MouseCoords.X>=0)
				sb.Draw (TextureManager.getTexture(999), GetMouseDrawRect(),Color.White);
		}
		private static Rectangle GetMouseDrawRect ()
		{
			return new Rectangle((int)(MouseCoords.X*Engine.TileWidth - Camera.Position.X), (int)(MouseCoords.Y*Engine.TileHeight-Camera.Position.Y),Engine.TileWidth,Engine.TileHeight);
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
		private static Vector2 GetMouseMapCoord(int x, int y) {
			Vector2 ret = new Vector2((x-(x%Engine.TileWidth))/Engine.TileWidth,(y-(y%Engine.TileHeight))/Engine.TileHeight);
			if (!Map.withinBounds(ret))
				return new Vector2(-1,-1);
			return ret;
		}
	}
}


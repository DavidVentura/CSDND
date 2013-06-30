using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DND
{
    class Player
    {
		public Vector2 position;
		public Texture2D texture;
		private Rectangle drawRect;
		private static Vector2 lastCameraPosition;
		Vector2 lastPos;
		public Player(Vector2 pos, Texture2D text) {
			this.position=pos;
			this.texture=text;
			lastPos = position;
			drawRect = new Rectangle((int)(position.X*Engine.TileWidth),(int)((1+position.Y)*Engine.TileHeight)-texture.Height,texture.Width,texture.Height);
		}

		public void Update (GameTime gameTime)
		{
			if (lastCameraPosition != Camera.Position) {
				lastCameraPosition = Camera.Position;
				UpdateDrawRect ();
			}
			if (position != lastPos) {
				lastPos=position;
				UpdateDrawRect ();
			}

		}
		public void UpdateDrawRect ()
		{
			drawRect = new Rectangle ((int)(position.X * Engine.TileWidth)-(int)Camera.Position.X, 
					                          (int)((1 + position.Y) * Engine.TileHeight) - texture.Height-(int)Camera.Position.Y,
					                          texture.Width, texture.Height);
		}
		public void Draw(SpriteBatch sb) {
			sb.Draw (texture,drawRect,Color.White);
		}

    }
}

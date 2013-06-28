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
		public double lastKeyPress;
		public bool isLocal=false;
		private Rectangle drawRect;

		public Player(Vector2 pos, Texture2D text, bool isLocal) {
			this.position=pos;
			this.texture=text;
			this.isLocal=isLocal;
			drawRect = new Rectangle((int)(position.X*Engine.TileWidth),(int)((1+position.Y)*Engine.TileHeight)-texture.Height,texture.Width,texture.Height);
		}

		public void Update (GameTime gameTime)
		{
			if (!isLocal)
				return;
			double curTime = gameTime.TotalGameTime.TotalMilliseconds;
			if (curTime - lastKeyPress < 120)
				return;
			Vector2 lastPos = position;
			if (Keyboard.GetState ().IsKeyDown (Keys.Right)) {
				lastKeyPress = curTime;
				position.X += 1;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Left)) {
				lastKeyPress = curTime;
				position.X -= 1;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Up)) {
				lastKeyPress = curTime;
				position.Y -= 1;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				lastKeyPress = curTime;
				position.Y += 1;
			}
			if (position != lastPos) {
				if (!Engine.ValidPosition (position)) {
					position = lastPos;
				} else 
					drawRect = new Rectangle ((int)(position.X * Engine.TileWidth), (int)((1 + position.Y) * Engine.TileHeight) - texture.Height, texture.Width, texture.Height);
			}
		}
		public void Draw(SpriteBatch sb, Vector2 camera) {
			sb.Draw (texture,drawRect,Color.White);
		}

    }
}

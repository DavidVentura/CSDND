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
		public int ID,texture;
		public Coord position;
		private Rectangle drawRect;
		private Coord lastCameraPosition;
		private Texture2D t;
		Coord lastPos;
		private int textureHeight=0,textureWidth=0;

		public Player (Coord pos, int text, int id)
		{
			this.position = pos;
			this.texture = text;
			this.ID = id;
			lastPos = position;
			t = TextureManager.getTexture (texture);
			if (t != null) {
				textureHeight = t.Height;
				textureWidth = t.Width;
			}
			drawRect = new Rectangle(position.X*Engine.TileWidth,(1+position.Y)*Engine.TileHeight-textureHeight,textureWidth,textureHeight);
		}

		public void Update (GameTime gameTime)
		{
			if (t == null) {
				t=TextureManager.getTexture (texture);

			}
			if (t != null && textureHeight==0) {
				textureHeight = t.Height;
				textureWidth = t.Width;
				UpdateDrawRect();
			}
			if (!lastCameraPosition.Equals(Camera.Position)) {
				lastCameraPosition = Camera.Position;
				UpdateDrawRect ();
			}
			if (!position.Equals(lastPos)) {
				lastPos=position;
				UpdateDrawRect ();
			}

		}
		public void UpdateDrawRect ()
		{

			drawRect = new Rectangle (position.X * Engine.TileWidth-Camera.Position.X, 
					                          (1 + position.Y) * Engine.TileHeight - textureHeight-Camera.Position.Y,
					                          textureWidth, textureHeight);
		}
		public void Draw(SpriteBatch sb) {
			if(t!=null)
				sb.Draw (t,drawRect,Color.White);
		}

    }
}

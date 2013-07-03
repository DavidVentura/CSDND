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
		private int textureHeight=0,textureWidth=0,NameWidth=0;
		private string name;
		private Coord DrawPos;

		public Player (Coord pos, int text, int id, string Name)
		{
			this.position = pos;
			this.texture = text;
			this.ID = id;
			name = Name;
			lastPos = position;
			t = TextureManager.getTexture (texture);
			if (t != null) {
				textureHeight = t.Height;
				textureWidth = t.Width;
			}
			NameWidth = (int)TextureManager.Font.MeasureString (name).X;
			UpdateDrawRect ();
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
			DrawPos = new Coord (position.X * Engine.TileWidth - Camera.Position.X, (1 + position.Y) * Engine.TileHeight - textureHeight - Camera.Position.Y);
			drawRect = new Rectangle (DrawPos.X, DrawPos.Y,textureWidth, textureHeight);
		}
		public void Draw(SpriteBatch sb) {
			if(t!=null)
				sb.Draw (t,drawRect,Color.White);

			sb.DrawString (TextureManager.Font, name, new Vector2 (DrawPos.X+(Engine.TileWidth/2)-(NameWidth/2)-1, DrawPos.Y + textureHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2 (DrawPos.X+(Engine.TileWidth/2)-(NameWidth/2)+1, DrawPos.Y + textureHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2 (DrawPos.X+(Engine.TileWidth/2)-(NameWidth/2), DrawPos.Y-1 + textureHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2 (DrawPos.X+(Engine.TileWidth/2)-(NameWidth/2), DrawPos.Y+1 + textureHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2 (DrawPos.X+(Engine.TileWidth/2)-(NameWidth/2), DrawPos.Y + textureHeight), Color.White);
		}

    }
}

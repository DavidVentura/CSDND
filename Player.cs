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
		public int ID,texture,NameOffset=0;
		public Coord position;
		private string name;
		public Animation animation;
		double lastAnimation=0;
		Vector2 DrawPos;

		public Player (Coord pos, int texture, int id, string Name)
		{
			this.position = pos;
			this.texture = texture;
			this.ID = id;
			name = Name;
			this.animation = new Animation ();

			animation.Texture = TextureManager.getTexture (texture);
			NameOffset = (Engine.TileWidth/2) -((int)TextureManager.Font.MeasureString (name).X)/2;
		}

		public void Update (GameTime gameTime)
		{
			if (animation.Texture == null) {
				animation.Texture=TextureManager.getTexture (texture);
			}
			if (gameTime.TotalGameTime.TotalMilliseconds - lastAnimation > 50) {
				lastAnimation = gameTime.TotalGameTime.TotalMilliseconds;
				animation.Update ();
			}
		}

		public void Draw(SpriteBatch sb) {
			if (animation.Texture != null)
				animation.Draw (sb,position.X,position.Y);
			DrawPos = new Vector2 (position.X * Engine.TileWidth - Camera.Position.X + NameOffset, 
			                       (1 + position.Y) * Engine.TileHeight - Camera.Position.Y);
			sb.DrawString (TextureManager.Font, name, new Vector2(DrawPos.X+1,DrawPos.Y), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(DrawPos.X-1,DrawPos.Y), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(DrawPos.X,DrawPos.Y+1), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(DrawPos.X,DrawPos.Y-1), Color.Black);
			sb.DrawString (TextureManager.Font, name, DrawPos, Color.White);
		}

    }
}

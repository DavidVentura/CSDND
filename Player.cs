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
		public int ID,Texture,NameOffsetX=0,size;
		public Coord Position;
		public int VisionRange;
		private string name;
		public Animation animation=new Animation();
		double lastAnimation=0;

		public Player (Coord pos, int texture, int id, string Name, int visionRange, int size)
		{
			VisionRange=visionRange;
			Position = pos;
			Texture = texture;
			ID = id;
			name = Name;
			this.size=size;
			animation.Sprite = TextureManager.getSprites (texture);
			int sizeOffset = (int)(((float)(size-1)/2)*Engine.TileWidth);
			if (TextureManager.Font==null) return;
			NameOffsetX = (Engine.TileWidth/2) -((int)TextureManager.Font.MeasureString (name).X)/2+sizeOffset;
		}
		public Player (Coord pos, int texture, int id, string Name, int size)
		{
			Position = pos;
			Texture = texture;
			ID = id;
			name = Name;
			this.size=size;
			animation.Sprite = TextureManager.getSprites (texture);
			int sizeOffset = (int)(((float)(size-1)/2)*Engine.TileWidth);
			NameOffsetX = (Engine.TileWidth/2) -((int)TextureManager.Font.MeasureString (name).X)/2+sizeOffset;
		}

		public void Update (GameTime gameTime)
		{
			if (animation.Sprite == null) {
				animation.Sprite=TextureManager.getSprites (Texture);
			}
			if (gameTime.TotalGameTime.TotalMilliseconds - lastAnimation > (Engine.MovementTime/Animation.Frames)) {
				lastAnimation = gameTime.TotalGameTime.TotalMilliseconds;
				animation.Update ();
			}
		}

		public void Draw (SpriteBatch sb)
		{
			if (animation.Sprite != null)
				animation.Draw (sb, Position.X, Position.Y,size);
			if (TextureManager.Font!=null) 
				animation.DrawName(sb,name,NameOffsetX);

		}

    }
}

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
		public int VisionRange=0;
		private string name;
		public Animation animation=new Animation();
		public bool visible=true,isLocal=false;
		double lastAnimation=0;

		public Player (Coord pos, int texture, int id, string Name, int size, int visionRange=0)
		{
			Position = pos;
			Texture = texture;
			ID = id;
			name = Name;
			this.size = size;
			animation.Sprite = TextureManager.getSprites (texture);
			int sizeOffset = (int)(((float)(size - 1) / 2) * Map.TileWidth);
			NameOffsetX = (Map.TileWidth / 2) - ((int)TextureManager.Font.MeasureString (name).X) / 2 + sizeOffset;

			if (visionRange >0) {
				isLocal = true;
				VisionRange = visionRange;
			}
		}

		public void Update (GameTime gameTime)
		{
			if (animation.Sprite == null) {
				animation.Sprite=TextureManager.getSprites (Texture);
			}
			if (gameTime.TotalGameTime.TotalMilliseconds - lastAnimation > (Map.MovementTime/Animation.Frames)) {
				lastAnimation = gameTime.TotalGameTime.TotalMilliseconds;
				animation.Update ();
			}
		}

		public void Draw (SpriteBatch sb)
		{
			if (animation.Sprite != null)
				animation.Draw (sb, Position.X, Position.Y,size,visible);
			if (TextureManager.Font!=null) 
				animation.DrawName(sb,name,NameOffsetX,visible);

		}

    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DND
{
	public enum Direction {
		Down=1,
		Left=2,
		Right=3,
		Up =4,
	}
	public class Animation
	{
		public const int Frames = 4;

		int CurFrame=0;
		Direction CurDir=Direction.Down;
		public Rectangle SrcRect;
		private Texture2D text;
		public Texture2D Texture{
			get { return text; }
			set {
				if (value == null)
					return;
				FrameWidth = value.Width / Frames;
				FrameHeight = value.Height / Frames;
				text = value;
				updateSrcRect ();
			}
		}
		public int FrameWidth,FrameHeight;

		private bool animating = false;
		private Rectangle DrawRect,OffsetDestRect;
		int FrameOffsetX=0,FrameOffsetY=0;

		public void SwitchDirection(Direction d){
			animating = true;
			CurDir = d;
			updateSrcRect ();
		}
		private void updateSrcRect ()
		{
			int xModifier = 0, yModifier=0;
			if (CurDir == Direction.Right)
				xModifier = 1;
			if (CurDir == Direction.Left)
				xModifier = -1;
			if (CurDir == Direction.Down)
				yModifier = 1;
			if (CurDir == Direction.Up)
				yModifier = -1;

			FrameOffsetX = (int)(((float)CurFrame / Frames) * Engine.TileWidth);
			FrameOffsetY = (int)(((float)CurFrame / Frames) * Engine.TileHeight);
			OffsetDestRect = new Rectangle (xModifier * FrameOffsetX - xModifier * Engine.TileWidth,
			                                yModifier * FrameOffsetY - yModifier * Engine.TileHeight,
			                                0, 0);
			if (!animating) {
				OffsetDestRect.X += xModifier * Engine.TileWidth;
				OffsetDestRect.Y += yModifier * Engine.TileWidth;
			}
			SrcRect = new Rectangle (FrameWidth*CurFrame, FrameHeight * ((int)CurDir-1), FrameWidth, FrameHeight);
		}
		public void Update(){
			if (animating) {
				CurFrame++;
				if (CurFrame >= Frames) {
					CurFrame = 0;
					animating = false;
				}
				updateSrcRect ();
			}
		}

		public void Draw (SpriteBatch sb, int x, int y)
		{
			DrawRect = new Rectangle(OffsetDestRect.X + x * Engine.TileWidth - Camera.Position.X,
			                         OffsetDestRect.Y + (1 + y) * Engine.TileHeight - FrameHeight - Camera.Position.Y,
			                         FrameWidth,FrameHeight);
			sb.Draw (Texture, DrawRect, SrcRect, Color.White);
		}
		public void DrawName (SpriteBatch sb, string name,int NameOffsetX)
		{
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X +1,DrawRect.Y+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X-1,DrawRect.Y+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X,DrawRect.Y+1+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X,DrawRect.Y-1+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X,DrawRect.Y+FrameHeight), Color.White);
		}

	}
}


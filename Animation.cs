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
		private Color textureColor=Color.White;
		private Color nameColor=Color.White;

		private Texture2D text;
		public Texture2D Sprite{
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

			FrameOffsetX = (int)(((float)CurFrame / Frames) * Map.TileWidth);
			FrameOffsetY = (int)(((float)CurFrame / Frames) * Map.TileHeight);
			OffsetDestRect = new Rectangle (xModifier * FrameOffsetX - xModifier * Map.TileWidth,
			                                yModifier * FrameOffsetY - yModifier * Map.TileHeight,
			                                0, 0);
			if (!animating) {
				OffsetDestRect.X += xModifier * Map.TileWidth;
				OffsetDestRect.Y += yModifier * Map.TileWidth;
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



		public void Draw (SpriteBatch sb, int x, int y, int size, bool visible)
		{
			if (visible)
				textureColor=Color.White;
			else
				textureColor=new Color(255,255,255,150);
			DrawRect = new Rectangle(OffsetDestRect.X + x * Map.TileWidth - Camera.Position.X,
			                         OffsetDestRect.Y + (size + y) * Map.TileHeight - FrameHeight - Camera.Position.Y,
			                         FrameWidth,FrameHeight);
			sb.Draw (Sprite, DrawRect, SrcRect, textureColor);
			//TODO: If (IsActive) DrawBorder
		}
		public void DrawName (SpriteBatch sb, string name,int NameOffsetX,bool visible)
		{
			if (visible)
				nameColor=Color.White;
			else
				nameColor=Color.Cyan;
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X +1,DrawRect.Y+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X-1,DrawRect.Y+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X,DrawRect.Y+1+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X,DrawRect.Y-1+FrameHeight), Color.Black);
			sb.DrawString (TextureManager.Font, name, new Vector2(NameOffsetX+DrawRect.X,DrawRect.Y+FrameHeight), nameColor);
		}

	}
}


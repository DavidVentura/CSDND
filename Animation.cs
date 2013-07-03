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
		const int Frames = 4;

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
		private Rectangle DrawRect;

		public Animation ()
		{
		}
		public void SwitchDirection(Direction d){
			animating = true;
			CurDir = d;
			updateSrcRect ();
		}
		private void updateSrcRect(){

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
			DrawRect = new Rectangle(x * Engine.TileWidth - Camera.Position.X,(1 + y) * Engine.TileHeight - FrameHeight - Camera.Position.Y,FrameWidth,FrameHeight);
			sb.Draw (Texture, DrawRect, SrcRect, Color.White);
		}
	}
}


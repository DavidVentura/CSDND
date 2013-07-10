using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DND
{
    static class Camera
    {
		//public static bool FollowPlayer = true; //TODO: Implemet smooth follow
		private static double lastKeyPress=0;
		private static double curTime=0;
        public static Coord Position;
		private const int speed = 10;
		private static int Width,Height,curFrame;
		private static Coord LastPosition,TargetPosition;

		/// <summary>
		/// Initialize the specified pos, w and h.
		/// </summary>
		/// <param name='pos'>
		/// Position.
		/// </param>
		/// <param name='w'>
		/// Width in tiles
		/// </param>
		/// <param name='h'>
		/// H in tiles
		/// </param>
        public static void Initialize(Coord pos, int w, int h)
        {
            Position = pos;
			TargetPosition=pos;
			LastPosition=pos;

			Width=w;
			Height=h;
        }
        public static void Update (GameTime gameTime)
		{
				
			curTime = gameTime.TotalGameTime.TotalMilliseconds;
			/*if (!Position.Equals(TargetPosition)) { //Smooth camera, good for moving to the player, bad for following him
				if (curTime - lastKeyPress >= Engine.MovementTime / 4) {
					lastKeyPress=curTime;
					Position += (TargetPosition - Position) / 4;
					return;
				}
			}*/

			if (curTime - lastKeyPress < 120)
				return;
			if (Engine.CurPlayer != null) {
				if (Keyboard.GetState ().IsKeyDown (Keys.LeftControl)) {//TODO: Smooth movement
					lastKeyPress = curTime;
					LastPosition = Position;
					curFrame = 0;
					TargetPosition.X = (Engine.CurPlayer.Position.X - (Width / 2)) * Engine.TileWidth;
					TargetPosition.Y = (Engine.CurPlayer.Position.Y - (Height / 2)) * Engine.TileHeight;
					return;
				}
			}
          	if (Keyboard.GetState ().IsKeyDown (Keys.W)) {
				lastKeyPress = curTime;
				Position.Y-=speed;
			}

			if (Keyboard.GetState ().IsKeyDown (Keys.S)) {
				lastKeyPress = curTime;
				Position.Y+=speed;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.A)) {
				lastKeyPress = curTime;
				Position.X-=speed;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D)) {
				lastKeyPress = curTime;
				Position.X +=speed;
			}
            
        }
    }
}

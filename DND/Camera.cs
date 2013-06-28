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
		private static double lastKeyPress=0;
		private static double curTime=0;
        public static Vector2 Position;
        /*Rectangle viewPort;
        int speed = 1;*/

        public static void Initialize(Vector2 pos)
        {
            Position = pos;
        }
        public static void Update(GameTime gameTime)
        {
			curTime=gameTime.ElapsedGameTime.TotalMilliseconds;
			if (curTime - lastKeyPress > 120)
				return;
          if (Keyboard.GetState ().IsKeyDown (Keys.W)) {
				lastKeyPress = curTime;
				Position.Y-=3;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.S)) {
				lastKeyPress = curTime;
				Position.Y+=3;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.A)) {
				lastKeyPress = curTime;
				Position.X-=3;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D)) {
				lastKeyPress = curTime;
				Position.X +=3;
			}
            
        }
    }
}

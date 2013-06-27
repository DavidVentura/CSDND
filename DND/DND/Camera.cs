using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DND
{
    class Camera
    {
        public Vector2 position;
        Rectangle viewPort;
        int speed = 1;

        public Camera(/*Rectangle viewPortRectangle, */Vector2 pos)
        {
            position = pos;
            //viewPort = viewPortRectangle;
        }
        public void Update(GameTime gameTime)
        {
          /*  if (InputHandler.KeyDown(Keys.Left))
                position.X -= speed;
            else
                if (InputHandler.KeyDown(Keys.Right))
                    position.X += speed;
            if (InputHandler.KeyDown(Keys.Up))
                position.Y -= speed;
            else
                if (InputHandler.KeyDown(Keys.Down))
                    position.Y += speed;*/
            position.X -= 0.1F;
        }
    }
}

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
        public Vector2 Position;
        /*Rectangle viewPort;
        int speed = 1;*/

        public Camera(Vector2 pos)
        {
            Position = pos;
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
            
        }
    }
}

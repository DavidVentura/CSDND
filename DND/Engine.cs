using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace DND
{
    class Engine
    {
        public const int tileHeight = 32;
        public const int tileWidth = 32;
        Map map = new Map(2,2);
        ContentManager content;
        

        public void Draw(SpriteBatch sb, int cameraX)
        {
            map.Draw(sb, cameraX);
        }
        public void LoadContent(ContentManager c)
        {
            content = c;
            map.LoadContent(c);
        }
        public void UnloadContent()
        {
            content.Unload();
        }
    }
}

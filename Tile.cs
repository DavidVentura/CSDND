using Microsoft.Xna.Framework.Graphics;

namespace DND
{
    class Tile
    {
        int textureNumber;
        public int TextureNumber
        {
            get { return textureNumber; }
        }

        public Tile(int t)
        {
            textureNumber = t;
        }
    }
}

using Microsoft.Xna.Framework.Graphics;

namespace DND
{
    class Tile
    {
        int textureNumber;
        /*public int width
        {
            get { return 32; }
        }
        public int height
        {
            get { return 32; }
        }*/
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

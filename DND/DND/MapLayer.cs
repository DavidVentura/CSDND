using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DND
{
    class MapLayer
    {
        private Tile[,] tiles;

        public MapLayer(int width, int height, int[,] textures)
        {
            tiles = new Tile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = new Tile(textures[x,y]);
                }
            }
            
        }
        internal Tile TileAt(int x, int y)
        {
            return tiles[x, y];
        }
    }
}

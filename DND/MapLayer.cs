using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DND
{
    class MapLayer
    {
        private Tile[,] tiles;
		private int id;//sorting

        public MapLayer(int id,int width, int height, int[,] tiledata)
        {
			this.id =id;
            tiles = new Tile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = new Tile(tiledata[x,y]);
                }
            }
            
        }
        internal Tile TileAt(int x, int y)
        {
            return tiles[x, y];
        }
    }
}

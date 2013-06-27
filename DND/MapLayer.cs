using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DND
{
    class MapLayer
    {
		private bool isblocking=false;
		public bool isBlocking {
			get { return isblocking; }
		}

        private Tile[,] tiles;
		private int id;//sorting

        public MapLayer(int id,int width, int height, int[,] tiledata, bool blocking)
        {
			this.id =id;
			this.isblocking = blocking;
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
		internal Tile TileAt(Vector2 v)
        {
            return tiles[(int)v.X, (int)v.Y];
        }
    }
}

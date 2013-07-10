using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DND
{
    class MapLayer
    {


        private Tile[,] tiles;
		public LayerType Type;
		public LayerType type {
			get { return Type; }
		}

        public MapLayer(LayerType type, int width, int height, int[,] tiledata)
        {
			Type = type;
            tiles = new Tile[width, height];
            for (int y = 0; y < height; y++)
            	for (int x = 0; x < width; x++)
                    tiles[x, y] = new Tile(tiledata[x,y]);
            
        }
		internal Tile TileAt(int x, int y)
        {
			if (!Map.withinBounds(x,y)) return new Tile(-1);
            return tiles[x, y];
        }
		internal Tile TileAt(Coord v)
        {
			if (!Map.withinBounds(v)) return new Tile(-1);
            return tiles[v.X, v.Y];
        }
		public void Modify (int id, int x, int y)
		{
			tiles[x,y]= new Tile(id);
		}
    }
}

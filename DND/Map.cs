using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace DND
{
    class Map
    {
        int height, width;

        static List<MapLayer> layers = new List<MapLayer>();
        public static List<MapLayer> Layers
        {
            get { return layers; }
        }
		int text_tile;

        public Map(int w, int h)
        {
            width = w;
            height = h;
        }
		public void addLayer(MapLayer layer) {
			layers.Add (layer);//todo check
		}

        public void Draw(SpriteBatch sb,Vector2 camera)
        {
            int y = 0, x=0;
            foreach (MapLayer layer in layers)
                for (y = 0; y < height; y++)
                    for (x = 0; x < width; x++)
                    {
                        text_tile = layer.TileAt(x, y).TextureNumber;
						if (text_tile>0) //"empty"
                        	sb.Draw(TextureManager.getTexture(text_tile), 
						        	new Rectangle(x*Engine.tileWidth-(int)camera.X,y*Engine.tileHeight-(int)camera.Y,Engine.tileWidth,Engine.tileHeight),
						        	Color.White);
                    }
       	}

        
    }
}

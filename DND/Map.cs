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
		Texture2D auxtext;

        static List<MapLayer> layers = new List<MapLayer>();
        public List<MapLayer> Layers
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
            int y = 0, x=0, xpos,ypos;
            foreach (MapLayer layer in layers)
                for (y = 0; y < height; y++)
                    for (x = 0; x < width; x++)
                    {
                        text_tile = layer.TileAt(x, y).TextureNumber;
						if (text_tile>0){ //"empty"
							auxtext = TextureManager.getTexture(text_tile);
							xpos = x*Engine.tileWidth-(int)camera.X;
							ypos = (y*Engine.tileHeight)-(int)camera.Y-(auxtext.Height-Engine.tileHeight);
                        	sb.Draw(auxtext, new Rectangle(xpos,ypos,auxtext.Width,auxtext.Height),Color.White);
						}
					}
       	}
		public bool withinBounds(Vector2 position) {
			if (position.X < 0 || position.Y <0 || position.X >= width || position.Y >= height)
				return false;
			return true;
		}

        
    }
}

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

        public void Draw (ref SpriteBatch sb, Vector2 camera)
		{
			int y = 0, x = 0, xpos=0, ypos;
            
			for (y = 0; y < height; y++)
				for (x = 0; x < width; x++) {
					foreach (MapLayer layer in layers) {
						text_tile = layer.TileAt (x, y).TextureNumber;
						if (text_tile > 0) { //"empty"
							auxtext = TextureManager.getTexture (text_tile);
							xpos = x * Engine.TileWidth - (int)camera.X;
							ypos = (y * Engine.TileHeight) - (int)camera.Y - (auxtext.Height - Engine.TileHeight);
							sb.Draw (auxtext, new Rectangle (xpos, ypos, auxtext.Width, auxtext.Height), Color.White);
						}

					}
					foreach(Player p in Engine.Players) {
						if (p.position.X ==x && p.position.Y == y)
						p.Draw (sb,camera);
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

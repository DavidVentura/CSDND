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
		Tile tileaux;


        public Map(int w, int h)
        {
            width = w;
            height = h;
        }

		public void addLayer(MapLayer layer) {
			layers.Add (layer);//todo check
		}

        public void LoadContent(ContentManager c)
        {
            

        }

        public void Draw(SpriteBatch sb,float cameraX)
        {
            int y = 0, x=0;
            foreach (MapLayer layer in layers)
                for (y = 0; y < height; y++)
                    for (x = 0; x < width; x++)
                    {
                        tileaux = layer.TileAt(x, y);
                        sb.Draw(TextureManager.getTexture(tileaux.TextureNumber), new Rectangle(x*tileaux.width-(int)cameraX,y*tileaux.height,tileaux.width,tileaux.height),Color.White);
                    }
       	}

        
    }
}

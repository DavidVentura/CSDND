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

        int[,] mapa = new int[2,2] { 
                                    {1,2},
                                    {2,1}
                                };

        public Map(int w, int h)
        {
            width = w;
            height = h;
            layers.Add(new MapLayer(2,2,mapa));
        }
        Texture2D t,t2;

        public void LoadContent(ContentManager c)
        {
            t = c.Load<Texture2D>("t");
            t2 = c.Load<Texture2D>("t2");

        }

        public void Draw(SpriteBatch sb,float cameraX)
        {
            int y = 0, x=0;
            Tile t;

            foreach (MapLayer layer in layers)
            {
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        t = layer.TileAt(x, y);
                        sb.Draw(getTexture(t.TextureNumber), new Rectangle(x*t.width-(int)cameraX,y*t.height,t.width,t.height),Color.White);
                    }
                }
            }
        }

        private Texture2D getTexture(int p)
        {
            if (p==1) return t;
            return t2;
        }
    }
}

using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public static class TextureManager
	{
		private static readonly object _lockTexture = new object();
		private static readonly object _lockSprite = new object();

		private static List<Textura> textures = new List<Textura>();
		private static List<Textura> sprites = new List<Textura>();

		private static ContentManager c;
		private static Textura temp;
		private static SpriteFont font;
		public static SpriteFont Font {
			get { return font; }
		}
		public struct Textura
		{
			public Textura(int id) {
				this.id = id;
				tex = null;
			}
			public int id;
			public Texture2D tex;
		}

		public static void LoadContent (ContentManager content)
		{
			c=content;
			addTexture(999); //Mouse
			font = c.Load<SpriteFont> ("Arial"); 
			
		}

		public static Texture2D getSprites(int p)
        {
            foreach(Textura t in sprites)
				if (t.id==p) return t.tex;

			return null;
        }

		public static Texture2D getTexture(int p)
        {
            foreach(Textura t in textures)
				if (t.id==p) return t.tex;

			return null;
        }

		public static void addTexture (int id) 
		{
			lock (_lockTexture) {
				foreach (Textura tex in textures)
					if (tex.id == id)
						return;
				textures.Add (new Textura (id));
			}
		}
		public static void addSprites (int id) 
		{
			lock (_lockSprite) {
				foreach (Textura tex in sprites)
					if (tex.id == id)
						return;
				sprites.Add (new Textura (id));
			}
		}

		private static void LoadTextures ()
		{

			for (int i=0; i<textures.Count; i++) {
				temp = textures [i];
				if (temp.tex==null){
					temp.tex = c.Load<Texture2D>("Textures/"+temp.id.ToString());
					textures [i] = temp;
				}
			}
		}
		private static void LoadSprites ()
		{

			for (int i=0; i<sprites.Count; i++) {
				temp = sprites [i];
				if (temp.tex==null){
					temp.tex = c.Load<Texture2D>("Sprites/"+temp.id.ToString());
					sprites [i] = temp;
				}
			}
		}
		public static void Update() {
			LoadTextures ();
			LoadSprites ();
		}
	}
}


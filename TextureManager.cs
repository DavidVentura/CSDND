using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public static class TextureManager
	{
		private static List<Textura> textures = new List<Textura>();
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

		public static Texture2D getTexture(int p)
        {
            foreach(Textura t in textures)
				if (t.id==p) return t.tex;

			return null;
        }

		public static void addTexture (int id) //TODO: thread safe
		{
			foreach(Textura tex in textures)
				if (tex.id == id)
					return;
			textures.Add (new Textura(id));
		}


		private static void LoadTextures ()
		{

			for (int i=0; i<textures.Count; i++) {
				temp = textures [i];
				if (temp.tex==null){
					temp.tex = c.Load<Texture2D>(temp.id.ToString());
					textures [i] = temp;
				}
			}
		}
		public static void Update() {
			LoadTextures ();
		}
	}
}


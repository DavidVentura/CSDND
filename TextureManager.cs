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

		public struct Textura
		{
			public Textura(int id) {
				this.id = id;
				tex = null;
			}
			public int id;
			public Texture2D tex;
		}

		public static void Initialize (ContentManager content)
		{
			c=content;
		}

		public static Texture2D getTexture(int p)
        {
            foreach(Textura t in textures)
				if (t.id==p) return t.tex;

			return null;
        }

		public static void addTexture (int id)
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


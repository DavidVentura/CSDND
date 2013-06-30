using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public static class TextureManager
	{
		private static List<Textura> textures = new List<Textura>();

		public struct Textura
		{
			public Textura(int id) {
				this.id = id;
				tex = null;
			}
			public int id;
			public Texture2D tex;
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

		public static void LoadTextures (int[] textureID, ContentManager c)
		{
			foreach (int t in textureID) {
				for(int i=0;i<textures.Count;i++){
					if (t==textures[i].id){
						Textura temp = textures[i];
						temp.tex = c.Load<Texture2D>(textures[i].id.ToString());
						textures[i] = temp;
					}
				}
			}
		}
		public static void LoadTextures (ContentManager c)
		{
			for (int i=0; i<textures.Count; i++) {
				Textura temp = textures [i];
				temp.tex = c.Load<Texture2D> (textures [i].id.ToString ());
				textures [i] = temp;
			}

		}
	}
}


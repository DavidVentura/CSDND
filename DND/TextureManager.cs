using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public static class TextureManager
	{
		private static List<int> allTextures = new List<int>();
		private static List<Textura> textures = new List<Textura>();

		public struct Textura
		{
			public Textura(int id, string n) {
				this.id = id;
				this.name = n;
				tex = null;
			}
			public int id;
			public Texture2D tex;
			public string name;
		}

		public static void addRequiredTexture(int id) {

		}
		public static Texture2D getTexture(int p)
        {
            foreach(Textura t in textures)
				if (t.id==p) return t.tex;

			return null;
        }

		public static void addTexture (int id,string name)
		{
			foreach(Textura tex in textures)
				if (tex.id == id)
					return;
			textures.Add (new Textura(id,name));
		}

		public static void LoadTextures (List<int> textureID, ContentManager c)
		{
			foreach (int t in textureID) {
				for(int i=0;i<textures.Count;i++){
					if (t==textures[i].id){
						Textura temp = textures[i];
						temp.tex = c.Load<Texture2D>(textures[i].name);
						textures[i] = temp;
					}
				}
			}
		}
	}
}


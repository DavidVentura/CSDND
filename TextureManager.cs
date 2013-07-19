using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public static class TextureManager
	{
		private const string texturePath="Textures/";
		private const string tilePath	="Tiles/";
		private const string objectPath	="Objects/";
		private static readonly object _lockTexture = new object();
		private static readonly object _lockSprite = new object();

		private static List<Textura> textures = new List<Textura>();
		private static List<Sprite> sprites = new List<Sprite>();

		private static ContentManager c;
		private static Textura temp;
		private static Sprite SpriteTemp;
		private static SpriteFont font;

		public static SpriteFont Font {
			get { return font; }
		}
		public struct Textura
		{
			public Textura(int id,LayerType t) {
				this.id = id;
				tex = null;
				layerType=t;
			}
			public LayerType layerType;
			public int id;
			public Texture2D tex;
		}
		public struct Sprite
		{
			public Sprite(int id) {
				this.id = id;
				tex = null;
			}
			public int id;
			public Texture2D tex;
		}
		public static void LoadContent (ContentManager content)
		{
			c=content;
			addTexture(999,LayerType.GUI); //Mouse
			addTexture(998,LayerType.GUI); //AOE Indicator
			font = c.Load<SpriteFont> ("Arial");
			LoadTextures ();
		}

		public static Texture2D getSprites (int p)
		{
			foreach (Sprite t in sprites)
				if (t.id == p) {
					if (t.tex == null)
						break;
					return t.tex;
				}
			addSprites (p);
			LoadSprites ();
			return null;
		}

		public static Texture2D getTexture (int p, LayerType type)
		{
			foreach (Textura t in textures)
				if (t.id == p && t.layerType==type) {
					if (t.tex==null) break;
					return t.tex;
				}
			addTexture (p,type);
			LoadTextures ();
			return null;
		}

		public static void addTexture (int id,LayerType t) 
		{
			lock (_lockTexture) {
				foreach (Textura tex in textures)
					if (tex.id == id && tex.layerType == t)
						return;
				textures.Add (new Textura (id,t));
			}
		}

		public static void addSprites (int id) 
		{
			lock (_lockSprite) {
				foreach (Sprite tex in sprites)
					if (tex.id == id)
						return;
				sprites.Add (new Sprite (id));
			}
		}

		private static void LoadTextures ()
		{

			for (int i=0; i<textures.Count; i++) {
				temp = textures [i];
				if (temp.tex==null){
					switch (textures[i].layerType){
						case LayerType.GUI:
							temp.tex = c.Load<Texture2D>(texturePath +temp.id.ToString());
							break;
						case LayerType.Ground:
							temp.tex = c.Load<Texture2D>(tilePath +temp.id.ToString());
							break;
						case LayerType.Object:
							temp.tex = c.Load<Texture2D>(objectPath +temp.id.ToString());
							break;
					}
					textures [i] = temp;
				}
			}
		}
		private static void LoadSprites ()
		{

			for (int i=0; i<sprites.Count; i++) {
				SpriteTemp = sprites [i];
				if (SpriteTemp.tex==null){
					SpriteTemp.tex = c.Load<Texture2D>("Sprites/"+SpriteTemp.id.ToString());
					sprites [i] = SpriteTemp;
				}
			}
		}
		public static void Update() {

		}
	}
}


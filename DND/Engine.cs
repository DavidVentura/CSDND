using Mono.Data.Sqlite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;

namespace DND
{
	public enum LayerType {
		Ground=0,
		Blocking=1,
		Object=2
	}
    static class Engine
    {

        public const int TileHeight = 32;
        public const int TileWidth = 32;

		public static Player LocalPlayer;
		public static List<Player> Players = new List<Player>();

		public static bool TexturesNotReady=true;
		public static int Initialize ()
		{
			if (Network.Initialize()==-1) return -1;
			Camera.Initialize(new Vector2(0,0));
			return 0;
		}

        public static void Draw(ref SpriteBatch sb)
        {
            Map.Draw(ref sb);
			GUI.Draw(sb);
        }
        public static void LoadContent (ContentManager c)
		{
			TextureManager.addTexture (999);
			while (TexturesNotReady) {
				System.Threading.Thread.Sleep(100);
			}
			TextureManager.LoadTextures(c);
			LocalPlayer= new Player(new Vector2(3,3), c.Load<Texture2D>("player"));
        }

		public static void Update (GameTime gameTime)
		{
			Camera.Update(gameTime);
			GUI.Update(gameTime);
			LocalPlayer.Update(gameTime);
			foreach (Player p in Players)
				p.Update (gameTime);

		}
			




    }
}

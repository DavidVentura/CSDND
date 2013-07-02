using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using System.Windows.Forms;

namespace DND
{
	public enum LayerType {
		Ground=0,
		Blocking=1,
		Object=2
	}
	public struct Coord {
		public int X;
		public int Y;
		public Coord (int x, int y)
		{
			X=x;
			Y=y;
		}
		public static bool operator ==(Coord c, Coord c2){
			return (c.X==c2.X && c.Y==c2.Y);
		}
		public static bool operator !=(Coord c, Coord c2){
			return (c.X!=c2.X || c.Y!=c2.Y);
		}

	}

    static class Engine
    {
		private static ContentManager content;

        public const int TileHeight = 32;
        public const int TileWidth = 32;

		public static Player LocalPlayer;
		public static List<Player> Players = new List<Player>();

		public static bool TexturesNotReady=true;
		public static int Initialize ()
		{
			Camera.Initialize(new Coord(0,0));
			return 0;
		}

        public static void Draw(ref SpriteBatch sb)
        {
            Map.Draw(ref sb);
			GUI.Draw(sb);
        }
        public static void LoadContent (ContentManager c)
		{
			content =c;
			TextureManager.Initialize(c); //todo: move
			if (Network.Initialize()==-1) return;

			TextureManager.addTexture (999);
			TextureManager.addTexture (6);

			TextureManager.Update();
			LocalPlayer= new Player(new Coord(3,3), 6,0);//TODO: move to database
        }

		public static void Update (GameTime gameTime)
		{
			TextureManager.Update();
			Camera.Update(gameTime);
			GUI.Update(gameTime);
			LocalPlayer.Update(gameTime);
			foreach (Player p in Players)
				p.Update (gameTime);

		}
		public static void AddPlayer (int id, int x, int y, int texture)
		{
			foreach (Player p in Players)
				if (p.ID == id)
					return;
			TextureManager.addTexture(texture);
			Players.Add (new Player(new Coord(x,y),texture,id));

		}

		public static void MovePlayer (int id, int x, int y)
		{
			foreach (Player p in Players)
				if (p.ID == id) {
				p.position= new Coord(x,y);
				}
		}
		public static void RemovePlayer (int i)
		{
			foreach (Player p in Players)
				if (p.ID == i) {
					Players.Remove (p);
					return;
				}
		}
		public static void AddText (string str)
		{
			Console.WriteLine(str);
		}

    }
}

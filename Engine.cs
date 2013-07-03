using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
		public bool Equals(Coord o){
			return (this.X == o.X && this.Y == o.Y);
		}
		public static Coord operator -(Coord c, Coord c2){
			return new Coord (c.X - c2.X, c.Y - c2.Y);
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
			TextureManager.Initialize(c); //TODO: move
			TextureManager.addTexture (999); //TODO: Mouse
			if (Network.Initialize()==-1) return;

			while (LocalPlayer==null)
				System.Threading.Thread.Sleep (100);
			TextureManager.Update();
        }

		public static void Login(Coord pos, int texture, int id, string name) {
			TextureManager.addTexture(texture);
			LocalPlayer = new Player (pos, texture, id, name);
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
		public static void AddPlayer (int id, int x, int y, int texture, string name)
		{
			foreach (Player p in Players)
				if (p.ID == id)
					return;
			TextureManager.addTexture(texture);
			Players.Add (new Player(new Coord(x,y),texture,id,name));

		}

		public static void MovePlayer (int id, int x, int y)
		{
			Coord newCoord = new Coord (x, y);

			foreach (Player p in Players)
				if (p.ID == id) {
					Coord diff = newCoord - p.position;
					if (diff.X > 0)
						p.animation.SwitchDirection (Direction.Right);
					if (diff.X < 0)
						p.animation.SwitchDirection (Direction.Left);
					if (diff.Y > 0)
						p.animation.SwitchDirection (Direction.Down);
					if (diff.Y < 0)
						p.animation.SwitchDirection (Direction.Up);
					p.position = newCoord;
					return;
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

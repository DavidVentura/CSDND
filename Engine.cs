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
		public static Double Distance (Coord c1, Coord c2) {
			return Math.Sqrt(((c1.X - c2.X)*(c1.X - c2.X))+((c1.Y - c2.Y)*(c1.Y - c2.Y)));
		}
	}

    static class Engine
    {
		public const int MovementTime = 120;
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
        public static int LoadContent (ContentManager c)
		{
			TextureManager.LoadContent (c);
			if (Network.Initialize () == -1)
				return -1;

			while (LocalPlayer==null) {
				if (Network.receiver.ThreadState != System.Threading.ThreadState.Running){
					return -1;
				}
				System.Threading.Thread.Sleep (100);
			}
			TextureManager.Update();
			return 0;
        }

		public static void Login(Coord pos, int sprite, int id, string name, int visionRange) {
			TextureManager.addSprites(sprite);
			LocalPlayer = new Player (pos, sprite, id, name,visionRange);
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
		public static void AddPlayer (int id, int x, int y, int sprite, string name)
		{
			foreach (Player p in Players)
				if (p.ID == id)
					return;
			TextureManager.addSprites(sprite);
			Players.Add (new Player(new Coord(x,y),sprite,id,name));

		}

		public static void MovePlayer (int id, int x, int y)
		{
			Coord newCoord = new Coord (x, y);
			Coord diff;
			foreach (Player p in Players)
				if (p.ID == id) {
					diff = newCoord - p.Position;
					if (diff.X > 0)
						p.animation.SwitchDirection (Direction.Right);
					if (diff.X < 0)
						p.animation.SwitchDirection (Direction.Left);
					if (diff.Y > 0)
						p.animation.SwitchDirection (Direction.Down);
					if (diff.Y < 0)
						p.animation.SwitchDirection (Direction.Up);
					p.Position = newCoord;
					return;
				}

			diff = newCoord - LocalPlayer.Position;
			if (diff.X > 0)
				LocalPlayer.animation.SwitchDirection (Direction.Right);
			if (diff.X < 0)
				LocalPlayer.animation.SwitchDirection (Direction.Left);
			if (diff.Y > 0)
				LocalPlayer.animation.SwitchDirection (Direction.Down);
			if (diff.Y < 0)
				LocalPlayer.animation.SwitchDirection (Direction.Up);
			LocalPlayer.Position = newCoord;
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

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
		public static Coord operator +(Coord c, Coord c2){
			return new Coord (c.X + c2.X, c.Y + c2.Y);
		}
		public static Coord operator /(Coord c, int v){
			return new Coord (c.X/v, c.Y/v);
		}
		public static Double Distance (Coord c1, Coord c2) {
			return Math.Sqrt(((c1.X - c2.X)*(c1.X - c2.X))+((c1.Y - c2.Y)*(c1.Y - c2.Y)));
		}
	}

    static class Engine 
    {
		#region Services
		private static GameServiceContainer _services;
    	private static GraphicsDevice _graphicsDevice;
    	private static ContentManager _content;
    	private static SpriteBatch _spriteBatch;
		#endregion

		public const int MovementTime = 120;
        public const int TileHeight = 32;
        public const int TileWidth = 32;

		public static int curCharIndex=0;
		private static int WindowWidth =0,WindowHeight=0;
		public static List<Player> LocalPlayers= new List<Player>();
		public static Player CurPlayer;

		public static List<Player> Players = new List<Player>();

		public static bool isDM = true;

		public static int Initialize (IGraphicsDeviceService graphics)
		{
			_services = new GameServiceContainer();
	        _services.AddService(typeof(IGraphicsDeviceService), graphics);

	        _graphicsDevice = graphics.GraphicsDevice;

	        _content = new ContentManager(_services, "Content");
	        _spriteBatch = new SpriteBatch(_graphicsDevice);
			if (LoadContent()==-1)
				return -1;
			GUI.Initialize();
			/*
			#if FPS30
			double dt = (double)1000 / (double)30;
			_graphicsDevice.SynchronizeWithVerticalRetrace = false;
			this.TargetElapsedTime = TimeSpan.FromMilliseconds(dt);
#else
			_graphicsDevice.SynchronizeWithVerticalRetrace = true;
#endif

			_graphicsDevice.DisplayMode =  .PreferredBackBufferHeight=400;
			_graphicsDevice.PreferredBackBufferWidth=400;
			Window.AllowUserResizing=true;
			
			_graphicsDevice.ApplyChanges();
			IsMouseVisible = true;*/
			WindowWidth=graphics.GraphicsDevice.Viewport.Width;
			WindowHeight=graphics.GraphicsDevice.Viewport.Height;
			Camera.Initialize(new Coord(0,0),WindowWidth/TileWidth,WindowHeight/TileHeight);
			return 0;
		}

        public static void Draw(GameTime gt)
        {
			_graphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin();
            Map.Draw(_spriteBatch);
			GUI.Draw(_spriteBatch);
			_spriteBatch.End();
        }
		public static int LoadContent ()
		{
			TextureManager.LoadContent (_content);
			if (Network.Initialize () == -1)
				return -1;

			while (LocalPlayers.Count==0 && !isDM) {
				if (Network.receiver.ThreadState != System.Threading.ThreadState.Running)
					return -1;				
				System.Threading.Thread.Sleep (100);
			}
			if (!isDM)
				CurPlayer = LocalPlayers [curCharIndex];
			
			TextureManager.Update();
			return 0;
        }

		public static void AddLocalPlayer(Coord pos, int sprite, int id, string name, int visionRange, int size) {
			TextureManager.addSprites(sprite);
			LocalPlayers.Add(new Player (pos, sprite, id, name,visionRange, size));
		}
		public static void Update (GameTime gameTime)
		{
			TextureManager.Update();
			Camera.Update(gameTime);
			GUI.Update(gameTime);
			foreach (Player p in LocalPlayers)
				p.Update(gameTime);
			foreach (Player p in Players)
				p.Update (gameTime);
		}
		public static void AddPlayer (int id, int x, int y, int sprite, string name, int size)
		{
			foreach (Player p in Players)
				if (p.ID == id)
					return;
			TextureManager.addSprites(sprite);
			Players.Add (new Player(new Coord(x,y),sprite,id,name,size));

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

			if (isDM && CurPlayer==null) return;
			diff = newCoord - CurPlayer.Position;
			if (diff.X > 0)
				CurPlayer.animation.SwitchDirection (Direction.Right);
			if (diff.X < 0)
				CurPlayer.animation.SwitchDirection (Direction.Left);
			if (diff.Y > 0)
				CurPlayer.animation.SwitchDirection (Direction.Down);
			if (diff.Y < 0)
				CurPlayer.animation.SwitchDirection (Direction.Up);
			CurPlayer.Position = newCoord;
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
			Console.WriteLine(str);//TODO: add to interface
		}
		public static void Unload() {
			Network.Unload();
		}

		public static void SwitchChar() {
			curCharIndex++;
			if (curCharIndex>=LocalPlayers.Count)
				curCharIndex=0;
			CurPlayer=LocalPlayers[curCharIndex];
		}
		public static bool withinSight (Coord c)
		{
			if (isDM && CurPlayer==null) return true;
			return (Coord.Distance(c, CurPlayer.Position) < CurPlayer.VisionRange);
		}
		public static void ChangeVisibility (int id)
		{
			foreach (Player p in Players)
				if (p.ID == id) {
					p.visible = !p.visible;
					return;
				}
			foreach (Player p in LocalPlayers)
				if (p.ID == id) {
					p.visible = !p.visible;
					return;
				}
		}



    }
}

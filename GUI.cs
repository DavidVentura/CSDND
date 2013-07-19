using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RamGecXNAControls;

namespace DND
{
	public static class GUI
	{
		private static MouseState oldMouse;
		private static Coord MouseCoords;
		private static List<Coord> tiles2 = new List<Coord>();
		private static List<Coord> tiles = new List<Coord>();
		private static int radius=0;
		private static ButtonState lastRButtonState = ButtonState.Released;
		private static double lastKeyPress;

		private static Window ChatWindow = new Window (new Rectangle (300, 0, 220, 410));
		private static TextArea ChatText = new TextArea (new Rectangle (10, 22, 200, 358));
		private static TextBox ChatTextSend = new TextBox (new Rectangle (10, 382, 200, 22));

		private static Window MainWindow = new Window (new Rectangle (540, 0, 260, 400));
		private static TabsContainer TabContainer = new TabsContainer (new Rectangle (5, 25, 250, 360));
		private static ListBox MobList = new ListBox ( new Rectangle(5,5,220,260));
		private static ListBox TileList = new ListBox ( new Rectangle(5,5,220,260));
		private static ListBox ObjectList = new ListBox ( new Rectangle(5,5,220,260));

		public static GUIManager guiManager;

		public static bool Typing = false;

		public static void Initialize()
		{
			guiManager.Controls.Clear();

			tiles.Add (new Coord (0, 0));
			tiles.Add (new Coord (0, 1));
			tiles.Add (new Coord (1, 0));
			tiles.Add (new Coord (1, 1));

			MainWindow.Movable = true;
			MainWindow.Title = "Titulo";
			MainWindow.TitleColor = Color.Black;

			ChatWindow.Controls.Add (ChatText);
			ChatTextSend.OnSubmit+= (GUIControl sender) => { Talk(); } ;
			ChatWindow.Controls.Add (ChatTextSend);
			TabControl tctrl = new TabControl ();

			Button b = new Button (new Rectangle (25, 10, 100, 20), "Toggle chat");
			b.OnClick += (GUIControl sender) => { ChatWindow.Visible=!ChatWindow.Visible; };
			tctrl.Controls.Add (b);
			b = new Button (new Rectangle (25, 35, 100, 20), "Toggle Visibility");
			b.OnClick += (GUIControl sender) => { if (Engine.CurPlayer!=null) Network.SendData ("VISI"); };
			tctrl.Controls.Add (b);
			b = new Button (new Rectangle (25, 60, 100, 20), "Toggle collision");
			b.OnClick += (GUIControl sender) => { if (Engine.CurPlayer!=null) Network.SendData ("NOCL"); };
			tctrl.Controls.Add (b);
			b = new Button (new Rectangle (25, 85, 100, 20), "Delay");
			b.OnClick += (GUIControl sender) => { if (Engine.CurPlayer!=null) Network.SendData ("DELA"); };
			tctrl.Controls.Add (b);

			tctrl.Text = "Settings";
			TabContainer.Controls.Add (tctrl);

			MainWindow.Controls.Add (TabContainer);
			guiManager.Controls.Add (MainWindow);
			guiManager.Controls.Add (ChatWindow);
		}

		public static void AddDMGUI() {//TODO: add combat, objects and mobs tabs
			TabControl tDM = new TabControl ();
			tDM.Text = "DM";
			TabsContainer tDMContainer = new TabsContainer(new Rectangle(5,5,240,320));

			TabControl tMisc = new TabControl ();
			tMisc.Text="Misc";

			Button b = new Button (new Rectangle (25, 10, 100, 20), "Initiative");
			b.OnClick += (GUIControl sender) => { Network.SendData ("INIT"); };
			tMisc.Controls.Add (b);
			b = new Button (new Rectangle (25, 35, 100, 20), "Reflexes");
			b.OnClick += (GUIControl sender) => { Network.SendData ("REFL"); };
			tMisc.Controls.Add (b);
			b = new Button (new Rectangle (25, 60, 100, 20), "Fortitude");
			b.OnClick += (GUIControl sender) => { Network.SendData ("FORT"); };
			tMisc.Controls.Add (b);
			b = new Button (new Rectangle (25, 85, 100, 20), "Will");
			b.OnClick += (GUIControl sender) => { Network.SendData ("WILL"); };
			tMisc.Controls.Add (b);
			b = new Button (new Rectangle (25, 110, 100, 20), "DM mode");
			b.OnClick += (GUIControl sender) => { DMMode(); };
			tMisc.Controls.Add (b);
			b = new Button (new Rectangle (25, 135, 100, 20), "Next turn");
			b.OnClick += (GUIControl sender) => { Network.SendData ("NEXT"); };
			tMisc.Controls.Add (b);


			TabControl tSpawn = new TabControl ();
			tSpawn.Text="Spawn";
			TabsContainer tSpawnContainer = new TabsContainer(new Rectangle(5,5,230,290));
			TabControl tc = new TabControl();
			tc.Text="Tiles";
			tc.Controls.Add (TileList);
			tSpawnContainer.Controls.Add (tc);

			tc = new TabControl();
			tc.Text="Objects";
			tc.Controls.Add (ObjectList);
			tSpawnContainer.Controls.Add (tc);

			tc = new TabControl();
			tc.Text="Mobs";
			tc.Controls.Add (MobList);
			tSpawnContainer.Controls.Add (tc);

			tSpawn.Controls.Add (tSpawnContainer);

			tDMContainer.Controls.Add (tMisc);
			tDMContainer.Controls.Add (tSpawn);
			tDM.Controls.Add (tDMContainer);

			TabContainer.Controls.Add (tDM);
		}

		private static void DMMode() {
			Engine.curCharIndex = 0;
			Engine.CurPlayer = null;
			Network.SendData ("DMMD"); //dm mode
		}

		private static void Talk() {
			Network.SendData ("TALK" + ChatTextSend.Text);
			ChatTextSend.Text = "";
			ChatTextSend.Focused = false;
		}

		public static void Update (GameTime gameTime)
		{
			if (!ChatWindow.IsMouseOver){
				ChatWindow.Transparency = 0.1F;
				ChatText.Transparency = 0.4F;
				ChatTextSend.Transparency = 0.1F;
			}
			else
			{
				ChatWindow.Transparency = 0.9F;
				ChatText.Transparency = 0.9F;
				ChatTextSend.Transparency = 0.9F;
			}

			Typing = ChatTextSend.Focused;

			guiManager.Update (gameTime);
			oldMouse = Mouse.GetState ();
			MouseCoords = GetMouseMapCoord (oldMouse.X + Camera.Position.X, oldMouse.Y + Camera.Position.Y);
			double curTime = gameTime.TotalGameTime.TotalMilliseconds;
			if (curTime - lastKeyPress < Map.MovementTime + 20 || Typing)
				return;

			if (Keyboard.GetState ().IsKeyDown (Keys.Z)) {
				lastKeyPress = curTime;
				int selected=Engine.MobID(MobList.SelectedString);
				if (selected>-1)
				Network.SendData (String.Format ("SPWN{0},{1},{2}",selected, MouseCoords.X, MouseCoords.Y));
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.X)) {
				lastKeyPress = curTime;
				Network.SendData (String.Format ("SOBJ{0},{1},{2},{3}", 2, 1, MouseCoords.X, MouseCoords.Y)); //TODO spawn obj interface
				//tileID,blocking,x,y
				return;
			}


			//TODO: move to spell list
			if (Keyboard.GetState ().IsKeyDown (Keys.D1)) {
				radius=1;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D2)) {
				radius=2;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D3)) {
				radius=3;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D4)) {
				radius=4;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D5)) {
				radius=5;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D6)) {
				radius=6;
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.D0)) {
				radius=0;
				return;
			}

			if (Mouse.GetState ().RightButton == ButtonState.Released && lastRButtonState == ButtonState.Pressed) {
				foreach (Player p in Map.GetLocalPlayers())
					if (p.Position.Equals(MouseCoords))
						Network.SendData("SWCH"+p.ID);
			}
			lastRButtonState = Mouse.GetState ().RightButton;

			if (Engine.CurPlayer == null) return;
			//No characters -> no movement

			if (Keyboard.GetState ().IsKeyDown (Keys.Right)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE1,0");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Left)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE-1,0");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Up)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE0,-1");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE0,1");
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				lastKeyPress = curTime;
				Network.SendData ("MOVE0,1");
				return;
			}

		}

		public static void AddMob (string name)
		{
			MobList.Items.Add(name);
		}
		public static void AddTile (string name)
		{
			TileList.Items.Add(name);
		}
		public static void AddObject (string name)
		{
			ObjectList.Items.Add(name);
		}
		public static void AddText(string s) {
			ChatText.Text += s+'\n';
		}
		public static void Draw (SpriteBatch sb)
		{
			guiManager.Draw (sb);
			if (!ChatWindow.IsMouseOver && !MainWindow.IsMouseOver) {
				if (MouseCoords.X >= 0 && MouseCoords.Y >= 0) {
					if (radius > 0) {
						GetAOETiles ();
						foreach (Coord c in tiles2)
							sb.Draw (TextureManager.getTexture (998,LayerType.GUI), new Rectangle (c.X * Map.TileWidth - Camera.Position.X, c.Y * Map.TileHeight - Camera.Position.Y, 32, 32), new Color (0, 0, 0, 200));
					} else
						sb.Draw (TextureManager.getTexture (999,LayerType.GUI), GetMouseDrawRect (), Color.White);
				}
			}

			for(int i =0; i<Engine.Initiatives.Count; i++)
				if (i==Engine.CurrentTurn)
				sb.DrawString(TextureManager.Font,Engine.Initiatives[i],new Vector2(0,i*20),Color.Red);
				else
				sb.DrawString(TextureManager.Font,Engine.Initiatives[i],new Vector2(0,i*20),Color.Yellow);

		}
		private static void GetAOETiles ()
		{
			tiles2.Clear ();
			if (radius == 1) {
				foreach (Coord c in tiles)
					tiles2.Add(MouseCoords+c);
				return;
			}
			bool invalido = false;
			int esquinas;
			Coord curCoord;
			for (int x =MouseCoords.X-(radius-1); x <= MouseCoords.X+radius; x++) {
				for (int y =MouseCoords.Y-(radius-1); y <= MouseCoords.Y+radius; y++) {
					curCoord = new Coord (x, y);
					invalido = false;
					foreach (Coord c in tiles) {
						if (Coord.Distance (c+ MouseCoords, curCoord) > radius * 1.12f) {
							invalido = true;
							break;
						}
					}
					if (!invalido){
						esquinas=0;
						foreach(Coord c in tiles)
							if (Coord.Distance(curCoord+c,MouseCoords+new Coord(1,1)) <= radius)
								esquinas++;
						if (esquinas>2)
							tiles2.Add (curCoord);
					}
				}
			}
		}
		private static Rectangle GetMouseDrawRect ()
		{
			return new Rectangle(MouseCoords.X*Map.TileWidth - Camera.Position.X, MouseCoords.Y*Map.TileHeight-Camera.Position.Y,Map.TileWidth,Map.TileHeight);
		}

		/// <summary>
		/// Gets the mouse map coordinate.
		/// </summary>
		/// <returns>
		/// The mouse map coordinate. Returns (-1,-1) if the coordinate is outside the map
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		private static Coord GetMouseMapCoord(int x, int y) {
			Coord ret = new Coord((x-(x%Map.TileWidth))/Map.TileWidth,(y-(y%Map.TileHeight))/Map.TileHeight);
			if (!Map.withinBounds(ret) || !Map.withinSight(ret))
				return new Coord(-1,-1);
			return ret;
		}
	}
}


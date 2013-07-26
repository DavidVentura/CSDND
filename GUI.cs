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




		private static Window MainWindow 			= new Window (new Rectangle (400, 0, 270, 400));
		private static TabsContainer TabContainer 	= new TabsContainer (new Rectangle (5, 25, 260, 360));
		private static ListBox MobList 				= new ListBox ( new Rectangle(5,5,240,290));
		private static ListBox TileList 			= new ListBox ( new Rectangle(5,5,240,290));
		private static ListBox ObjectList 			= new ListBox ( new Rectangle(5,25,240,270));
		private static CheckBox Blocking 			= new CheckBox(new Rectangle(5,5,230,15),"Blocking");

		private static ChatWindow ChatWindow;
		private static BuffWindow BuffWindow;
		private static RollWindow rollWindow 	= new RollWindow(new Rectangle(150,150,260,300),"Roll");
		private static EditRoll AddEditRoll;


		public static GUIManager guiManager;

		public static bool Typing = false;
		public static int curTargetID = -1;
		private static Player MouseOverPlayer= null;

		public static void Initialize()
		{
			guiManager.Controls.Clear();
			BuffWindow 	= new BuffWindow (new Rectangle (200, 100, 300, 200), "(DE)Buff");
			AddEditRoll = new EditRoll (new Rectangle (100, 200, 300, 200), "Add Roll");
			ChatWindow 	= new ChatWindow (new Rectangle (0, 300, 600, 165),"Chat");

			tiles.Add (new Coord (0, 0));
			tiles.Add (new Coord (0, 1));
			tiles.Add (new Coord (1, 0));
			tiles.Add (new Coord (1, 1));

			MainWindow.Movable = true;
			MainWindow.Title = "Titulo";
			MainWindow.TitleColor = Color.Black;
			TabControl tctrl = new TabControl ();

			Button b = new Button (new Rectangle (25, 10, 100, 20), "Toggle chat");
			b.OnClick += (GUIControl sender) => { if (ChatWindow.Visible) ChatWindow.Hide(); else ChatWindow.Show(); };
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

			b = new Button (new Rectangle (25, 200, 100, 20), "Exit");
			b.OnClick += (GUIControl sender) => { Engine.Unload(); };
			tctrl.Controls.Add (b);

			tctrl.Text = "Settings";
			TabContainer.Controls.Add (tctrl);

			MainWindow.Controls.Add (TabContainer);
			guiManager.Controls.Add (MainWindow);
			guiManager.Controls.Add (BuffWindow);

			guiManager.Controls.Add (ChatWindow);
			guiManager.Controls.Add (rollWindow);
			guiManager.Controls.Add (AddEditRoll);
		}

		public static void AddDMGUI() {
			TabControl tDM = new TabControl ();
			tDM.Text = "DM";
			TabsContainer tDMContainer = new TabsContainer(new Rectangle(2,5,252,320));

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


			TabControl tc = new TabControl();
			tc.Text="Tiles";
			tc.Controls.Add (TileList);
			tDMContainer.Controls.Add (tc);

			tc = new TabControl();
			tc.Text="Objects";
			tc.Controls.Add(Blocking);
			tc.Controls.Add (ObjectList);
			tDMContainer.Controls.Add (tc);

			tc = new TabControl();
			tc.Text="Mobs";
			tc.Controls.Add (MobList);
			tDMContainer.Controls.Add (tc);
			tDMContainer.Controls.Add (tMisc);

			tDM.Controls.Add (tDMContainer);

			TabContainer.Controls.Add (tDM);
		}

		private static void DMMode() {
			Engine.curCharIndex = 0;
			Engine.CurPlayer = null;
			Network.SendData ("DMMD"); //dm mode
		}



		public static void Update (GameTime gameTime)
		{
			MouseOverPlayer = Map.PlayerAt (MouseCoords);
			Typing = (ChatWindow.IsTyping() || BuffWindow.IsTyping () || AddEditRoll.IsTyping () || rollWindow.IsTyping() );

			guiManager.Update (gameTime);
			oldMouse = Mouse.GetState ();
			MouseCoords = Map.GetMouseMapCoord (oldMouse.X + Camera.Position.X, oldMouse.Y + Camera.Position.Y);
			double curTime = gameTime.TotalGameTime.TotalMilliseconds;
			if (curTime - lastKeyPress < Map.MovementTime + 20 || Typing)
				return;

			if (Keyboard.GetState ().IsKeyDown (Keys.Z)) {
				lastKeyPress = curTime;
				int selected = Engine.MobID (MobList.SelectedString);
				if (selected > -1)
					Network.SendData (String.Format ("SPWN{0},{1},{2}", selected, MouseCoords.X, MouseCoords.Y));
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.X)) {
				lastKeyPress = curTime;
				int selected = Engine.ObjID (ObjectList.SelectedString);
				if (selected > -1)
					Network.SendData (String.Format ("SOBJ{0},{1},{2},{3}", selected, Convert.ToInt32 (Blocking.Checked), MouseCoords.X, MouseCoords.Y)); 
				//OBJID,blocking,x,y
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.C)) {
				lastKeyPress = curTime;
				int selected = Engine.TileID (TileList.SelectedString);
				if (selected > -1)
					Network.SendData (String.Format ("TILE{0},{1},{2}", selected, MouseCoords.X, MouseCoords.Y));
				//OBJID,blocking,x,y
				return;
			}

			if (Keyboard.GetState ().IsKeyDown (Keys.B)) {
				lastKeyPress = curTime;
				curTargetID = (MouseOverPlayer == null ? -1 : MouseOverPlayer.ID);

				if (curTargetID > -1) {
					BuffWindow.Show ();
				}
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.R)) {
				lastKeyPress = curTime;
				curTargetID = (MouseOverPlayer == null ? -1 : MouseOverPlayer.ID);

				if (Engine.CurPlayer!=null)
					AddEditRoll.Show ();
				return;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.V)) {
				lastKeyPress = curTime;
				curTargetID = (MouseOverPlayer == null ? -1 : MouseOverPlayer.ID);

				if (Engine.CurPlayer!=null )
					rollWindow.Show();
				return;
			}
			if (!Typing) {
				//TODO: move to spell list
				if (Keyboard.GetState ().IsKeyDown (Keys.D1)) {
					radius = 1;
					return;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.D2)) {
					radius = 2;
					return;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.D3)) {
					radius = 3;
					return;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.D4)) {
					radius = 4;
					return;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.D5)) {
					radius = 5;
					return;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.D6)) {
					radius = 6;
					return;
				}
				if (Keyboard.GetState ().IsKeyDown (Keys.D0)) {
					radius = 0;
					return;
				}
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
			ChatWindow.AddText(s);
		}
		public static void Draw (SpriteBatch sb)
		{
			guiManager.Draw (sb);
			if ((!ChatWindow.Visible || !ChatWindow.IsMouseOver) && !MainWindow.IsMouseOver && (!BuffWindow.Visible || !BuffWindow.IsMouseOver) &&
			    (!rollWindow.Visible || !rollWindow.IsMouseOver) && (!AddEditRoll.Visible || !AddEditRoll.IsMouseOver)) {
				DrawBuffs(sb);
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

		private static Rectangle GetMouseDrawRect ()
		{
			return new Rectangle(MouseCoords.X*Map.TileWidth - Camera.Position.X, MouseCoords.Y*Map.TileHeight-Camera.Position.Y,Map.TileWidth,Map.TileHeight);
		}

		static void DrawBuffs (SpriteBatch sb)
		{
			if (MouseOverPlayer != null) {
					int curBuff=0;
					foreach(Buff b in MouseOverPlayer.GetBuffs()) {
						sb.Draw (TextureManager.getTexture(1000,LayerType.GUI),new Rectangle((1+MouseCoords.X)* Map.TileWidth,MouseCoords.Y * Map.TileHeight+curBuff*25,150,20),Color.CornflowerBlue);
						sb.DrawString(TextureManager.Font,b.Desc(),new Vector2((1+MouseCoords.X)* Map.TileWidth,MouseCoords.Y * Map.TileHeight+curBuff*25),Color.White);
						curBuff++;
					}
				}
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
		public static void SetRoll() {
			rollWindow.SetRolls(Engine.CurPlayer.Rolls);
		}

	}
}


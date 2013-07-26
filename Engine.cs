using System.Collections.Generic;
using System.Xml;
using System;


namespace DND
{
	public enum State {
		Waiting=0,
		Error=1,
		OK=2
	}
	public enum LayerType {
		Ground=0,
		GUI=1,
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
	public struct Mob {
		public string name;
		public int id;
		public Mob (int id, string name)
		{
			this.id=id;
			this.name=name;
		}
	}
	public struct Roll {
		public string desc;
		public string val;
		public string mult;
		public Roll (string d, string v, string m)
		{
			desc=d;
			val=v;
			mult=m;
		}
	}
	public struct MapObject {
		public string name;
		public int id;
		public int type;
		/// <summary>
		/// Initializes a new instance of the <see cref="DND.MapObject"/> struct.
		/// </summary>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='type'>
		/// Type. 0 for tiles, 1 for objects
		/// </param>
		public MapObject (int id, string name, int type)
		{
			this.id=id;
			this.name=name;
			this.type=type;
		}
	}
	public struct Buff {
		private string desc;
		public Buff(string desc){
			this.desc = desc;
		}
		public String Desc() {
			return desc;
		}
	}
    static class Engine
    {
		public static State CurrentState;
		public static bool isDM = false;
		public static int curCharIndex=0;
		public static Player CurPlayer;
		public static string Username,IP;
		public static List<string> Initiatives= new List<string>();
		public static int CurrentTurn;
		public static List <Mob> Mobs = new List<Mob>();
		public static List<MapObject> MapObjects = new List<MapObject>();

		public static Roll TempRoll;
		public static void Unload() {
			Network.Unload();
			Environment.Exit(0);
		}

		public static void SwitchChar (int id)
		{
			foreach (Player p in Map.GetLocalPlayers())
				if (id == p.ID) {
					CurPlayer = p;
					break;
				}
			GUI.SetRoll();
		}

		public static void ParseXML() {
			using (XmlReader reader = XmlReader.Create("Config.xml")) {
				reader.MoveToContent ();
				while (reader.Read()) {
					if (reader.NodeType == XmlNodeType.Element)
						if (reader.Name == "IP")
							IP = reader.ReadElementContentAsString();
				}
			}
		}
		public static void SetInitiatives (string[] args)
		{
			Initiatives.Clear();
			Initiatives.AddRange(args);
		}
		public static void ParseMobs (string [] mobs)
		{
			foreach (string s in mobs) {
				if (s.Length==0) continue;
				string[] mob = s.Split('-');
				Mobs.Add (new Mob (Int32.Parse (mob[0]), mob[1]));
				GUI.AddMob(mob[1]);
			}
		}
		public static int MobID (string name)
		{
			foreach (Mob s in Mobs)
				if (s.name==name)
					return s.id;
			return -1;
		}
		public static void ParseTiles (string[] args)
		{
			foreach (string s in args) {
				if (s.Length == 0)
					continue;
				string[] tile = s.Split ('-');
				MapObjects.Add(new MapObject(Int16.Parse(tile[0]),tile[1],0));
				GUI.AddTile (tile [1]);
			}
		}
		public static void ParseObjs (string[] args)
		{
			foreach (string s in args) {
				if (s.Length == 0)
					continue;
				string[] obj = s.Split ('-');
				MapObjects.Add(new MapObject(Int16.Parse(obj[0]),obj[1],1));
				GUI.AddObject (obj [1]);
			}
		}
		public static int ObjID (string selectedString)
		{
			foreach(MapObject o in MapObjects)
				if (o.type==1) //object
					if (o.name==selectedString)
						return o.id;
				return -1;
		}
		public static int TileID (string selectedString)
		{
			foreach(MapObject o in MapObjects)
				if (o.type==0) //tile
					if (o.name==selectedString)
						return o.id;
				return -1;
		}


    }
}

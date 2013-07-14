using System;
using System.Collections.Generic;
using System.Xml;


namespace DND
{
	public enum State {
		Waiting=0,
		Error=1,
		OK=2
	}
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
		public static State CurrentState;
		public static bool isDM = false;
		public static int curCharIndex=0;
		public static Player CurPlayer;
		public static string Username,IP;
		public static List<string> Initiatives= new List<string>();
		public static int CurrentTurn;

		public static void Unload() {
			Network.Unload();
		}

		public static void SwitchChar(int id) {
			foreach (Player p in Map.GetLocalPlayers())
				if (id==p.ID)
					CurPlayer=p;
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

    }
}

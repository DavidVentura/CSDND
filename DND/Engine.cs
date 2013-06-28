using Mono.Data.Sqlite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DND
{
    static class Engine
    {
		const string ConnectionString = "URI=file:database.db";
        public const int TileHeight = 32;
        public const int TileWidth = 32;
		private const int MAPID = 1;
		private static List<int> textures = new List<int>();
		public static List<Player> Players = new List<Player>();
      


        public static void Draw(ref SpriteBatch sb)
        {
            Map.Draw(ref sb);
        }
        public static void LoadContent(ContentManager c)
        {
			LoadDatabase();
			TextureManager.LoadTextures(textures,c);
			Players.Add(new Player(new Vector2(3,3), c.Load<Texture2D>("player"),true));
			Players.Add(new Player(new Vector2(4,2), c.Load<Texture2D>("player2"),false));
			textures.Clear();
        }

		public static void Update (GameTime gameTime)
		{
			Camera.Update(gameTime);
			foreach (Player p in Players)
				p.Update (gameTime);

		}
		private static void LoadDatabase ()
		{
			IDbConnection dbcon;
			IDataReader reader;
			dbcon = (IDbConnection)new SqliteConnection (ConnectionString);
			dbcon.Open ();
			IDbCommand dbcmd = dbcon.CreateCommand ();

			dbcmd.CommandText = "SELECT WIDTH,HEIGHT FROM MAP WHERE ID=" + MAPID;
			reader = dbcmd.ExecuteReader ();
			if (reader.Read ()) {
				Map.Initialize(reader.GetInt16 (0), reader.GetInt16 (1));
			} else {
				return; //die
			}
			dbcmd.Dispose ();
			dbcmd.CommandText = "SELECT ID,BLOCKING,GROUND,DATA FROM LAYERS WHERE MAPID=" + MAPID;
			reader = dbcmd.ExecuteReader ();
			while (reader.Read ()) {
				Map.AddLayer (ParseMapLayer (reader.GetInt16 (0), reader.GetInt16 (1),reader.GetInt16 (2), 6, 6, reader.GetString (3)));
			}
			
			dbcmd.Dispose ();
			dbcmd.CommandText = "SELECT ID,TEXTURE FROM TEXTURES";
			reader = dbcmd.ExecuteReader ();

			while (reader.Read ()) {
				TextureManager.addTexture(reader.GetInt16(0),reader.GetString(1));
			}


			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
			dbcon.Close();
			dbcon = null;
		}

		private static MapLayer ParseMapLayer (int id, int blocking, int ground, int width, int height, string parseData)
		{
			int[,] data = new int[width, height];
			string[] rows = parseData.Split ('|');
			string[] cols;
			for (int y =0; y < rows.Length;y++) {
				cols = rows[y].Split(',');
				for (int x = 0; x < cols.Length;x++) {
					data[x,y] = Int16.Parse(cols[x]);
					if (!textures.Contains(data[x,y]))
						textures.Add (data[x,y]);
				}
			}
			return new MapLayer(id,width,height,data,blocking==1,ground==1);
		}		




    }
}

using Mono.Data.Sqlite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Data;
using System.Collections.Generic;

namespace DND
{
    class Engine
    {
		const string connectionString = "URI=file:database.db";

        public const int tileHeight = 32;
        public const int tileWidth = 32;
		private const int MAPID = 1;
		private List<int> textures = new List<int>();

		Map map;
        ContentManager content;
        

        public void Draw(SpriteBatch sb, int cameraX)
        {
            map.Draw(sb, cameraX);
        }
        public void LoadContent(ContentManager c)
        {
            content = c;
			LoadDatabase();
			TextureManager.LoadTextures(textures,c);
            map.LoadContent(c);
        }
		private void LoadDatabase ()
		{
			IDbConnection dbcon;
			IDataReader reader;
			dbcon = (IDbConnection)new SqliteConnection (connectionString);
			dbcon.Open ();
			IDbCommand dbcmd = dbcon.CreateCommand ();

			dbcmd.CommandText = "SELECT WIDTH,HEIGHT FROM MAP WHERE ID=" + MAPID;
			reader = dbcmd.ExecuteReader ();
			if (reader.Read ()) {
				map = new Map (reader.GetInt16 (0), reader.GetInt16 (1));
			} else {
				return; //die
			}
			dbcmd.Dispose ();
			dbcmd.CommandText = "SELECT ID,BLOCKING,DATA FROM LAYERS WHERE MAPID=" + MAPID;
			reader = dbcmd.ExecuteReader ();
			while (reader.Read ()) {
				map.addLayer (parseMapLayer (reader.GetInt16 (0), reader.GetInt16 (1), 6, 6, reader.GetString (2)));
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

		private MapLayer parseMapLayer (int id, int blocking, int width, int height, string parseData)
		{
			int[,] data = new int[width, height];
			string[] rows = parseData.Split ('|');
			string[] cols;
			for (int y =0; y < rows.Length;y++) {
				cols = rows[y].Split(',');
				for (int x = 0; x < cols.Length;x++) {
					data[x,y] = Int16.Parse(cols[x]);
					addTexture(data[x,y]);
				}
			}

			return new MapLayer(id,width,height,data);
		}
        public void UnloadContent()
        {
            content.Unload();
        }

		private void addTexture(int id){
			if (!textures.Contains(id))
				textures.Add (id);
		}
    }
}

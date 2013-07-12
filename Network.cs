using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace DND
{
	public static class Network
	{
		static TcpClient client = new TcpClient();
		static IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
		static NetworkStream clientStream;
		static ASCIIEncoding encoder = new ASCIIEncoding();
		public static Thread receiver;
		private static byte[] buffer;

		public static int Initialize ()
		{
			try {				
				client.Connect (serverEndPoint);
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				return -1;
			}
			clientStream = client.GetStream();
			receiver =new Thread(new ThreadStart(GetData));
			receiver.Start();

			SendData("LOGIDM");//TODO: ask for name
			return 0;
		}

		public static void SendData(string data){
			data+="|";
			buffer = encoder.GetBytes(data);
			clientStream.Write(buffer, 0 , buffer.Length);
			clientStream.Flush();
			//Thread.Sleep (10);
		}

		private static void GetData ()
		{
			byte[] bytes;
			string data,header;
			string[] args,allData;
			while (true) {
				if (!client.Connected)
					return;
				try {
					bytes = new byte[client.ReceiveBufferSize];
					clientStream.Read (bytes, 0, client.ReceiveBufferSize);
				} catch (Exception e) {
					Console.WriteLine (e.Message);
					clientStream.Close();
					Unload();
					return;
				}

				data=encoder.GetString (bytes).TrimEnd ('\0');//Some versions fill the string with nulls
				allData=data.Split('|');
				for (int i=0; i<allData.Length;i++){
					data =allData[i];

					if (data.Length<4) continue;
					#if DEBUG 
						Console.WriteLine(data);
					#endif
					header = data.Substring (0,4);
					args = data.Substring (4).Split(',');
					switch(header){
					case "LAYR":
						int width=Int32.Parse(args[0]);
						int height=Int32.Parse(args[1]);
						Map.Initialize(width,height);
						LayerType type = (LayerType)Int32.Parse(args[2]);
						int[,] textures = new int[width,height];
						for (int x=0; x<width;x++)
							for(int y=0;y<height;y++)
								textures[x,y]=Int32.Parse(args[3+x].Split ('-')[y]);

						Map.AddLayer(new MapLayer(type,width,height,textures));
						break;
					case "TXTR":
						for (int j=0;j<args.Length;j++)
							TextureManager.addTexture(Int32.Parse(args[j]));
						break;
					case "NPLR": //new player: ID,X,Y,Texture,Name,size
						Engine.AddPlayer(Int32.Parse(args[0]),Int32.Parse(args[1]),Int32.Parse(args[2]),Int32.Parse(args[3]),args[4],Int32.Parse(args[5]));
						break;
					case "MPLR": //move player: ID,X,Y
						Engine.MovePlayer(Int32.Parse(args[0]),Int32.Parse(args[1]),Int32.Parse(args[2]));
						break;
					case "RPLR": //remove Player: id
						Engine.RemovePlayer(Int32.Parse(args[0]));
						break;
					case "TALK": //player talks
						GUI.AddText(args[0]+"> " + args[1]);
						break;
					case "MESS": //gets message
						GUI.AddText(args[0]);
						break;
					case "LOGI"://log in: x,y,texture,id,name,vision range(tiles)
						Engine.AddLocalPlayer (new Coord (Int32.Parse (args [0]), Int32.Parse (args [1])), Int32.Parse (args [2]), Int32.Parse (args [3]), args [4],Int32.Parse (args [5]),Int32.Parse (args [6]));
						break;
					case "ERRO": //error loggin in, disconnect
						client.Close();
						break;
					case "SWCH": //switch character: args: id of the new current-char
						Engine.SwitchChar();
						break;
					case "VISI": //switch character: args: id of the new current-char
						Engine.ChangeVisibility(Int32.Parse(args[0]));
						break;
					case "SOBJ"://id,x,y
						Map.Modify(Int32.Parse(args[0]),Int32.Parse(args[1]),Int32.Parse(args[2]));
						break;
					case "DMOK": //dm rights
						Engine.isDM=true;
						GUI.AddDMGUI();
						break;
					default:
						Console.WriteLine(data);
						break;
					}
				}
			}
		}
		public static void Unload ()
		{
			if (receiver!=null && receiver.IsAlive)
				receiver.Abort();
		}

	}
}


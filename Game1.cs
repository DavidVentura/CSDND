using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;


namespace DND
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
	/// 

	public class Game1 : Game
    {
		private GraphicsDeviceManager _graphics;
		private GraphicsDevice g;
		private SpriteBatch spriteBatch;
		private Screen currentScreen;
		private GameScreen GS;
		private MenuScreen MS;
		private ContentManager contentManager;

		public Game1 ()
	    {
			_graphics = new GraphicsDeviceManager(this);
			contentManager = new ContentManager(Services, "Content");
			g = new Microsoft.Xna.Framework.Graphics.GraphicsDevice();
			_graphics.PreferredBackBufferWidth=800;
			_graphics.PreferredBackBufferHeight=600;
			//_graphics.IsFullScreen=true;
			_graphics.ApplyChanges();

			spriteBatch = new SpriteBatch(GraphicsDevice);
			Engine.ParseXML();
			Window.AllowUserResizing=true;
			GUI.guiManager = new RamGecXNAControls.GUIManager (this);
			GS = new GameScreen(ExitGame);
			MS = new MenuScreen(StartGame);
			currentScreen = MS;
    	}
		protected override void LoadContent ()
		{
			GS.LoadContent(contentManager);
			MS.LoadContent(contentManager);
		}
		protected void StartGame (object obj, EventArgs e)
		{
			Network.Initialize();
			while (Engine.CurrentState==State.Waiting)
				System.Threading.Thread.Sleep (200);

			if (Engine.CurrentState == State.Error) {
				MS.LoginError();
				return;
			}
			currentScreen = GS;
			GS.Start();
		}
		protected void ExitGame (object obj, EventArgs e)
		{
			currentScreen = MS;
		}

        protected override void Update(GameTime gameTime)
        {
			currentScreen.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime)
		{
			g.Clear (Color.Black);
			spriteBatch.Begin ();
			currentScreen.Draw(spriteBatch);
			spriteBatch.End();
        }

		protected override void OnExiting (object sender, EventArgs args)
		{
			Engine.Unload();
			Environment.Exit (0);
		}


    }
}

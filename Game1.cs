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
		public Game1 ()
	    {
        	_graphics = new GraphicsDeviceManager(this);
			GUI.guiManager = new RamGecXNAControls.GUIManager (this);
    	}

        protected override void Initialize ()
		{
			base.Initialize ();
			if (Engine.Initialize (_graphics) == -1) {
				Engine.Unload();
				Environment.Exit(-1);
			}
            
        }


        protected override void Update(GameTime gameTime)
        {
			Engine.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Engine.Draw(gameTime);
        }

		protected override void OnExiting (object sender, EventArgs args)
		{
			Engine.Unload();
			Environment.Exit (0);
		}


    }
}

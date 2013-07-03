using System;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DND
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
	/// 


    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		const int FRAMERATE = 30;
		int curFrames = 0;
		double lastCheck = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
        }

        protected override void Initialize ()
		{
			double dt = (double)1000 / (double)FRAMERATE;
			graphics.SynchronizeWithVerticalRetrace = false;
			this.TargetElapsedTime = TimeSpan.FromMilliseconds(dt);
			graphics.PreferredBackBufferHeight=640;
			graphics.PreferredBackBufferWidth=480;
			Window.AllowUserResizing=true;
			
			graphics.ApplyChanges();
			IsMouseVisible = true;
			if (Engine.Initialize () == -1) {
				MessageBox.Show ("Something borked");
				Environment.Exit(1);
			}

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Engine.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
			Content.Unload ();
        }

        protected override void Update(GameTime gameTime)
        {
			Engine.Update(gameTime);

			curFrames++;
			if (gameTime.TotalGameTime.TotalMilliseconds-lastCheck > 1000) {
				lastCheck = gameTime.TotalGameTime.TotalMilliseconds;
				this.Window.Title= curFrames.ToString();
				curFrames=0;
			}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();
                Engine.Draw(ref spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
		protected override void OnExiting(Object sender, EventArgs args)
		{
			Network.Unload();
		    base.OnExiting(sender, args);

		    // Stop the threads
		}



    }
}

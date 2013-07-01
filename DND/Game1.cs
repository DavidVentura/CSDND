using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DND
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		int curFrames = 0;
		double lastCheck = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			IsMouseVisible = true;
			if (Engine.Initialize () == -1) {
				MessageBox.Show ("Something borked");
				Environment.Exit(1);
			}
			SetFrameRate(graphics,30);
			graphics.SynchronizeWithVerticalRetrace=true;
			graphics.PreferredBackBufferHeight=230;
			graphics.PreferredBackBufferWidth=230;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Engine.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
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
		public void SetFrameRate(GraphicsDeviceManager manager, int frames)
		{
		   double dt = (double)1000 / (double)frames;
		   manager.SynchronizeWithVerticalRetrace = false;
		   this.TargetElapsedTime = TimeSpan.FromMilliseconds(dt);
		   manager.ApplyChanges();
		}


    }
}

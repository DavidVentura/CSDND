using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public class GameScreen : Screen
	{
		public GameScreen (EventHandler theScreenEvent): base(theScreenEvent)
		{			

		}
		public override void Update(GameTime gameTime){
			TextureManager.Update();
			Camera.Update(gameTime);
			GUI.Update(gameTime);
			Map.Update(gameTime);
		}
		public override void Draw (SpriteBatch _spriteBatch)
		{
			Map.Draw(_spriteBatch);
			GUI.Draw(_spriteBatch);
		}
		public override void LoadContent (ContentManager c)
		{
			TextureManager.LoadContent (c);
        }

		public void Start() {
			if (!Engine.isDM)
				Engine.CurPlayer = Map.GetLocalPlayers()[Engine.curCharIndex];
			GUI.Initialize();
			Camera.Initialize(new Coord(0,0),800,600);
		}
	}
}


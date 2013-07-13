using System;
using xnacontrols;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DND
{
	public class Screen
	{
        protected EventHandler ScreenEvent;
        public Screen(EventHandler theScreenEvent)
        {
            ScreenEvent = theScreenEvent;
        }

		public virtual void Update(GameTime theTime){	}
		public virtual void Draw(SpriteBatch theBatch){	}
		public virtual void LoadContent(ContentManager c) { }
		public virtual void Unload() {}
	}
}


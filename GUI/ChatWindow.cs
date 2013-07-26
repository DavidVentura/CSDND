using System;
using RamGecXNAControls;
using Microsoft.Xna.Framework;

namespace DND
{
	public class ChatWindow:Window
	{
		private static TextArea ChatText 	= new TextArea (new Rectangle (5, 22, 590, 115));
		private static TextBox ChatTextSend = new TextBox (new Rectangle (5, 137, 590, 22));

		public ChatWindow  (Rectangle bounds,string title) : base(bounds,title,"Chat")
		{
			Controls.Add (ChatText);
			Hide();
			ChatTextSend.OnSubmit+= (GUIControl sender) => { Talk(); } ;
			Controls.Add (ChatTextSend);
		}
		private static void Talk() {
			Network.SendData ("TALK" + ChatTextSend.Text);
			ChatTextSend.Text = "";
			ChatTextSend.Focused = false;
		}
		public override void Update (GameTime gameTime)
		{
			if (!IsMouseOver) {
				Transparency = 0.1F;
				ChatText.Transparency = 0.4F;
				ChatTextSend.Transparency = 0.1F;
			} else {
				Transparency = 0.9F;
				ChatText.Transparency = 0.9F;
				ChatTextSend.Transparency = 0.9F;
			}
			base.Update(gameTime);
		}
		public void AddText (string s)
		{
			ChatText.Text = s+'\n'+ChatText.Text;
		}

	}
}


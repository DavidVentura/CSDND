using System;
using RamGecXNAControls;
using Microsoft.Xna.Framework;

namespace DND
{
	public class EditRoll : Window
	{
		private TextBox RollMultiplier 	= new TextBox (new Rectangle (5, 25, 290, 20));
		private TextBox RollValue	 	= new TextBox (new Rectangle (5, 50, 290, 20));
		private TextBox RollDescription	= new TextBox (new Rectangle (5, 75, 290, 100));
		private Button SendRoll 		= new Button (new Rectangle (195, 175, 100, 20), "OK");
		private Button CancelRoll 		= new Button (new Rectangle (5, 175, 100, 20), "Cancel");
		private Point lastPos;
		public EditRoll (Rectangle bounds,string title) : base(bounds,title,"Editroll")
		{
			Visible	= false;
			SendRoll.OnClick	+= (sender) => { Send(); };
			CancelRoll.OnClick	+= (sender) => { Hide(); };
			Controls.Add (RollDescription);
			Controls.Add (RollMultiplier);
			Controls.Add (SendRoll);
			Controls.Add (RollValue);
			Controls.Add (CancelRoll);
		}

		private void Send() {
			if (RollMultiplier.Text.Length==0||RollValue.Text.Length==0||RollDescription.Text.Length==0) return;
			Network.SendData("AROL"+RollDescription.Text+","+RollMultiplier.Text+","+RollValue.Text);
			Hide();
		}

		public bool GetFocused ()
		{
			return (RollMultiplier.Focused || RollMultiplier.Focused || RollDescription.Focused);
		}
		public void Show() {
			RollMultiplier.Text="";
			RollValue.Text="";
			RollDescription.Text="";
			Bounds.Location=lastPos;
			Enabled=true;
			Visible=true;
		}
		void Hide ()
		{
			lastPos=Bounds.Location;
			Bounds.Location= new Point(-999,-999);
		}
	}
}


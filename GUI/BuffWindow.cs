using System;
using RamGecXNAControls;
using Microsoft.Xna.Framework;

namespace DND
{
	public class BuffWindow:Window
	{
		private TextBox BuffDescription = new TextBox (new Rectangle (5, 50, 290, 120));
		private TextBox BuffDuration 	= new TextBox (new Rectangle (5, 25, 290, 20));
		private Button SendBuff			= new Button (new Rectangle (195, 175, 100, 20), "OK");
		private Button CancelBuff 		= new Button (new Rectangle (5, 175, 100, 20), "Cancel");
		private Point lastPos;


		public BuffWindow (Rectangle bounds,string title) : base(bounds,title,"Buff")
		{
			Visible=false;
			BuffDuration.NumbersOnly=true;
			CancelBuff.OnClick += (GUIControl sender) => { Hide ();};
			SendBuff.OnClick += (GUIControl sender) => { Buff(); } ;
			Controls.Add (BuffDescription);
			Controls.Add (BuffDuration);
			Controls.Add (SendBuff);
			Controls.Add (CancelBuff);
		}
		private void Buff() {
			if (BuffDuration.Text.Length==0||BuffDescription.Text.Length==0) return;
			int duration = Convert.ToInt32(MathHelper.Clamp (Int32.Parse (BuffDuration.Text), 1, 100));
			Network.SendData (String.Format ("BUFF{0},{1},{2}", GUI.curTargetID, duration, BuffDescription.Text));
			Visible = false;
		}
		public bool GetFocused ()
		{
			return (BuffDescription.Focused || BuffDuration.Focused);
		}
		public void Show() {
			BuffDescription.Text="";
			BuffDuration.Text="";
			Bounds.Location=lastPos;
			Enabled=true;
			Visible=true;
		}
		void Hide ()
		{
			lastPos=Bounds.Location;//TODO: move to Window
			Bounds.Location= new Point(-999,-999);
		}
	}
}


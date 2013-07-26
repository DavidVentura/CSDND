using System;
using RamGecXNAControls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DND
{
	public class RollWindow:Window
	{
		private ListBox Rolls	= new ListBox(new Rectangle(5,25,250,240));
		private Button CANCEL 	= new Button(new Rectangle(5,270,80,20),"Cancel");
		private Button DELETE 	= new Button(new Rectangle(90,270,80,20),"Delete");
		private Button OK 		= new Button(new Rectangle(175,270,80,20),"Ok");
		public RollWindow (Rectangle bounds,string title) : base(bounds,title,"Rollwindow")
		{
			OK.OnClick+= (sender) => { SendRoll(); };
			CANCEL.OnClick+= (sender) => { Hide(); };
			DELETE.OnClick += (sender) =>  {} ;
			Controls.Add(Rolls);
			Controls.Add(OK);
			Controls.Add(CANCEL);
			Hide();
		}

		private void SendRoll() {
			if (Rolls.SelectedString.Length==0) return;
			Network.SendData("ROLL"+Rolls.SelectedString);
		}
		public void SetRolls (List<Roll> rolls)
		{
			Rolls.Items.Clear();
			foreach(Roll r in rolls)
				Rolls.Items.Add (r.desc);
		}

	}
}


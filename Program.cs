#region Using Statements
using System;
using System.Collections.Generic;

#endregion

namespace DND
{
	static class Program
	{
		[STAThread]
		static void Main ()
		{
			using (Game1 game = new Game1()) {
				game.Run ();
			}
		}
	}
}

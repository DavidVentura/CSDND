#region Using Statements
using System;
using System.Collections.Generic;

#endregion

namespace DND
{
	static class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
			using (Game1 game = new Game1()) {
				game.Run ();
			}
			/*
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new why());
			Environment.Exit (0);*/
		}
	}
}

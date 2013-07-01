#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

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
            Game1 game = new Game1();
            game.Run();            
		}
	}
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RamGecXNAControls;
namespace DND
{
	public class MenuScreen:Screen
	{
		Window mainWindow,ErrorWindow;
		TextBox loginText;
		Button loginButton,ErrorButton;
		Label ErrorLabel;

		public MenuScreen(EventHandler theScreenEvent): base(theScreenEvent)
		{
			mainWindow = new Window(new Rectangle(100,0,300,300), "Login");
			mainWindow.Transparency=1f;
			loginText = new TextBox(new Rectangle(100,100,100,20),"");
			loginText.Focused=true;
			loginText.OnSubmit+= (sender) => { Login(); };
			mainWindow.Controls.Add (loginText);
			loginButton = new Button(new Rectangle(100,130,100,20),"Log in");

			loginButton.OnClick+= (sender) => { Login(); } ; //invoco el evento asignado en el constructor

			mainWindow.Controls.Add (loginButton);


			ErrorWindow = new Window(new Rectangle(150,50,300,300), "Login");
			ErrorWindow.Movable=false;
			ErrorWindow.Title="Error";
			ErrorButton= new Button(new Rectangle(100,250,100,25),"OK");
			ErrorButton.OnClick += (sender) => { ErrorWindow.Visible = false; ErrorWindow.TopMost=false;mainWindow.Visible=true;};
			ErrorLabel = new Label(new Rectangle(10,25,290,20),"Error on login");

			ErrorWindow.Controls.Add(ErrorLabel);
			ErrorWindow.Controls.Add (ErrorButton);
			ErrorWindow.Visible=false;
			GUI.guiManager.Controls.Add (ErrorWindow);
			GUI.guiManager.Controls.Add (mainWindow);
		}

		public override void Update(GameTime gameTime)
		{
			loginButton.Enabled=(loginText.Text.Length>0);
			GUI.guiManager.Update(gameTime);
		}
		public override void Draw (SpriteBatch _spriteBatch)
		{
			GUI.guiManager.Draw(_spriteBatch);
		}
		public override void LoadContent (ContentManager c)
		{

        }
		public void LoginError ()
		{
			ErrorWindow.TopMost=true;
			ErrorWindow.Visible=true;
			mainWindow.Visible=false;
		}
		private void Login() {
			Engine.Username=loginText.Text;
			ScreenEvent.Invoke(this, new EventArgs());
		}
	}
}


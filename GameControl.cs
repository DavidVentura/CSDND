using System;
using WinFormsGraphicsDevice;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace DND
{
	public abstract class GameControl : GraphicsDeviceControl
{
    GameTime _gameTime;
    Stopwatch _timer;
    TimeSpan _elapsed;

    protected override void Initialize ()
    {
        _timer = Stopwatch.StartNew();

        Application.Idle += delegate { GameLoop(); };
    }

    protected override void Draw ()
    {
        Draw(_gameTime);
    }

    private void GameLoop ()
    {
        _gameTime = new GameTime(_timer.Elapsed, _timer.Elapsed - _elapsed);
        _elapsed = _timer.Elapsed;

        Update(_gameTime);
        Invalidate();
    }

    protected abstract void Update (GameTime gameTime);
    protected abstract void Draw (GameTime gameTime);
}
}


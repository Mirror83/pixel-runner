using Godot;

namespace PixelRunner.Scenes;

public partial class Background : Control
{
	private Vector2 _scrollVelocity = new(-80f, 0);

	private Vector2 _screenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += _scrollVelocity * (float)delta;
		if (Position.X >= -_screenSize.X) return;
		Position = new Vector2(0, Position.Y);
	}
}

using Godot;

namespace PixelRunner.Scenes;

public partial class Ground : StaticBody2D
{
	private Vector2 _groundVelocity = new(-200f, 0);

	private Vector2 _screenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var textureRect = GetNode<TextureRect>("TextureRect");
		var textureRectCopy = GetNode<TextureRect>("TextureRectCopy");

		textureRect.Position += _groundVelocity * (float)delta;
		textureRectCopy.Position += _groundVelocity * (float)delta;
		
		
		if (textureRect.Position.X >= -_screenSize.X) return;
		textureRect.Position = new Vector2(0, textureRect.Position.Y);
		textureRectCopy.Position = new Vector2(_screenSize.X, textureRectCopy.Position.Y);

	}
}

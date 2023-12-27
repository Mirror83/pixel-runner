using Godot;

namespace PixelRunner.Scenes;

public partial class GroundTexture : ScrollingTexture
{
	protected override Vector2 ScrollVelocity { get; set; } = new(-300f, 0);

}
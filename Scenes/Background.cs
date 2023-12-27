using Godot;

namespace PixelRunner.Scenes;

public partial class Background : ScrollingTexture
{
	protected override Vector2 ScrollVelocity { get; set; } = new(-80f, 0);
	
}

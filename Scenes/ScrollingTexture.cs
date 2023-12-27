using Godot;

namespace PixelRunner.Scenes;

public abstract partial class ScrollingTexture: Control
{
    protected abstract Vector2 ScrollVelocity { get; set; }

    private Vector2 _screenSize;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position += ScrollVelocity * (float)delta;
        if (Position.X >= -_screenSize.X) return;
        Position = new Vector2(0, Position.Y);
    }
}
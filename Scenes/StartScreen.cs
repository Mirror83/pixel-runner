using Godot;

namespace PixelRunner.Scenes;

public partial class StartScreen : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			EmitSignal(SignalName.StartGame);
		}
		
	}

	public void Disable()
	{
		Hide();
		SetProcess(false);
	}

	public void Enable()
	{
		Show();
		SetProcess(true);
	}
	
}

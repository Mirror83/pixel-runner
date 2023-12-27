using Godot;

namespace PixelRunner.Scenes;

public partial class Main : Node
{
    [Export] public PackedScene SnailScene { get; set; }
    [Export] public PackedScene FlyScene { get; set; }

    private int _score;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnEnemySpawnTimerTimeout()
    {
        var snail = SnailScene.Instantiate<Snail>();

        var spawnPosition = GetNode<Marker2D>("SnailSpawnPosition").Position;
        snail.Position = spawnPosition;

        // The x component is negative as the snail is supposed to move to the left side of
        // the screen. The y component is zero as it should move along the ground, which is not sloped.
        var velocity = new Vector2(-300, 0);
        snail.LinearVelocity = velocity;

        AddChild(snail);
    }

    private void OnNewGame()
    {
        var player = GetNode<Player>("Player");
        
        // 1. Enable player physics
        player.ProcessMode = ProcessModeEnum.Inherit;
        // 2. Show player
        if (!player.Visible) player.Show();
        // 3. (Re)start enemy spawn timer
        GetNode<Timer>("EnemySpawnTimer").Start();
        
        // 4. Revert process mode to Inherit
        GetTree().SetGroup("snails", Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Inherit);
        
    }
    private async void OnPlayerHit()
    {
        GD.Print("Game over");
        
        var enemySpawnTimer = GetNode<Timer>("EnemySpawnTimer");
        enemySpawnTimer.Stop();
        
        GetTree().SetGroup("snails", Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Disabled);

        var player = GetNode<Player>("Player");
        player.ProcessMode = ProcessModeEnum.Disabled;
        
        await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

        GetTree().CallGroup("snails", Node.MethodName.QueueFree);
        
        player.Hide();
    }
}
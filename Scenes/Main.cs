using Godot;

namespace PixelRunner.Scenes;

public partial class Main : Node
{
    [Export] public PackedScene SnailScene { get; set; }
    [Export] public PackedScene FlyScene { get; set; }

    private Vector2 _playerStartPosition = new(80, 258);

    private int _score;

    private const int FlyThresholdScore = 10;
    // private const int ScoreMultiplier = 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnEnemySpawnTimerTimeout()
    {
        var isFly = false;
        if (_score >= FlyThresholdScore)
            isFly = GD.Randi() % 2 == 0;

        var enemy = isFly ? FlyScene.Instantiate<Fly>() : SnailScene.Instantiate<Snail>();

        var spawnPosition = isFly
            ? GetNode<Marker2D>("FlySpawnPosition").Position
            : GetNode<Marker2D>("SnailSpawnPosition").Position;
        enemy.Position = spawnPosition;

        // The x component is negative as the snail is supposed to move to the left side of
        // the screen. The y component is zero as it should move along the ground, which is not sloped.
        var velocity = new Vector2(-300, 0);
        enemy.LinearVelocity = velocity;

        AddChild(enemy);
    }

    private void OnNewGame()
    {
        var player = GetNode<Player>("Player");
        
        // Reset Player start position
        player.Position = _playerStartPosition;
        player.Velocity = Vector2.Zero;

        var startScreen = GetNode<StartScreen>("StartScreen");

        // Enable Player physics
        player.ProcessMode = ProcessModeEnum.Inherit;

        if (!player.Visible) player.Show();

        // (Re)start enemy spawn timer
        GetNode<Timer>("EnemySpawnTimer").Start();

        // Enable physics for the snails
        GetTree().SetGroup("enemies", Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Inherit);

        // Enable ground and background scrolling
        GetNode<GroundTexture>("Ground/GroundTexture").SetProcess(true);
        GetNode<Background>("Background").SetProcess(false);
        
        _score = 0;
        UpdateScoreLabel(_score);
        GetNode<Timer>("ScoreTimer").Start();
        
        startScreen.Disable();
    }

    private async void OnPlayerHit()
    {
        GD.Print("Game over");

        var enemySpawnTimer = GetNode<Timer>("EnemySpawnTimer");
        enemySpawnTimer.Stop();
        
        var startScreen = GetNode<StartScreen>("StartScreen");

        GetNode<GroundTexture>("Ground/GroundTexture").SetProcess(false);

        GetNode<Background>("Background").SetProcess(false);
        
        GetNode<Timer>("ScoreTimer").Stop();

        GetTree().SetGroup("enemies", Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Disabled);

        var player = GetNode<Player>("Player");
        player.ProcessMode = ProcessModeEnum.Disabled;

        await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

        GetTree().CallGroup("enemies", Node.MethodName.QueueFree);

        player.Hide();
        
        startScreen.Enable();
    }

    private void OnScoreTimerTimeout()
    {
        _score += 1;
        UpdateScoreLabel(_score);
    }

    private void UpdateScoreLabel(int score)
    {
        var label = GetNode<Label>("ScoreDisplay/Label");
        label.Text = $"Score: {score.ToString()}";
    }
}
using Godot;

namespace PixelRunner.Scenes;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export] public const float JumpVelocity = -500.0f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    [Export] public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    
    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
            animatedSprite2D.Play("jump");    
        } else animatedSprite2D.Play("walk");
            

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        Velocity = velocity;

        // If a collision occurs, we have to code the response
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider().IsClass("RigidBody2D"))
            {
                EmitSignal(SignalName.Hit);
            }
                
        }
    }
}
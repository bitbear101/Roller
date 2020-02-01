using Godot;
using System;

public class GoalArea : Area2D
{
    [Signal]
    delegate void GoalReached();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
    public void BodyEntered(RigidBody2D body2D)
    {        
        EmitSignal(nameof(GoalReached));
    }
}

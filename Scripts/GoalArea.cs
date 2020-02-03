using Godot;
using System;

public class GoalArea : Area2D
{
    [Signal]
    delegate void GoalReached();

    public void BodyEntered(RigidBody2D body2D)
    {        
        EmitSignal(nameof(GoalReached));
    }
}

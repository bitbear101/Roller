using Godot;
using System;

public class ClickPlatform : KinematicBody2D
{
    bool platformExtended = false;
    bool movePlatform = false, movingPlatform = false;
    int speed = 500;
    Vector2 velocity = new Vector2();
    Vector2 platformStartPos;
    [Signal]
    delegate void Clicked(KinematicBody2D body);

    public override void _Ready()
    {
        platformStartPos = Position;
    }

    public override void _InputEvent(Godot.Object viewport, Godot.InputEvent @event, int shapeIdx)
    {
        if (Input.IsActionPressed("click"))
        {
            if (!movingPlatform) movePlatform = !movePlatform;
        }
    }
    public override void _Process(float delta)
    {
        if (movePlatform)
        {
            movingPlatform = true;
            if (platformStartPos.DistanceTo(Position) < GetNode<Sprite>("Sprite").Texture.GetSize().x)
            {
                velocity = new Vector2(speed, 0).Rotated(Rotation);
                velocity = MoveAndSlide(velocity);
                platformExtended = true;
            }
            else
            {
                movingPlatform = false;
            }
        }
        else
        {
            movingPlatform = true;
            if (platformStartPos.DistanceTo(Position) > 1)
            {
                velocity = new Vector2(-speed, 0).Rotated(Rotation);
                velocity = MoveAndSlide(velocity);
                platformExtended = false;
            }
            else
            {
                movingPlatform = false;
            }
        }


        //if (!Input.IsActionPressed("click"))
        //{
        //    dragging = false;
        //    EmitSignal(nameof(Clicked), this, dragging);
        //}
    }

}
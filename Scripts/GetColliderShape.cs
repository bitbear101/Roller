using Godot;
using System;

public class GetColliderShape : CollisionPolygon2D
{
    
    Polygon2D myShape;
        public override void _Ready()
    {
       myShape = GetNode<Polygon2D>("../../../Polygon2D");
       this.Polygon = myShape.Polygon;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

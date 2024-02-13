using Godot;
using System;


public partial class Ship : Area2D
{
	public Vector2 ScreenSize; // Size of the game window.	 
	
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}
	
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("MoveToPos"))
		{
			
			//Get the Position
			Vector2 Targetpos = GetViewport().GetMousePosition();// InputEventMouse.position;

			velocity = (Targetpos-Position); //.Normalized();	
		}
		
		
		if (velocity.Length() > 0)
		{
			
			velocity = velocity.Normalized() * 40;
			Rotation = velocity.Angle();
		}
		else
		{
		}
		
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}

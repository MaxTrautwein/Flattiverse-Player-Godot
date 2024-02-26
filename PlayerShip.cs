using Godot;
using System;
using Flattiverse.Connector;

public partial class PlayerShip : Node2D
{
	private Vector2 ScreenSize;

	private Vector2 _ScreenCenter;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		_ScreenCenter = ScreenSize / 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.PlayerShip != null)
		{
			shipSize = GameManager.PlayerShip.Size;
			var pos = GameManager.PlayerShip.Position;
			//GD.Print($"Ship Size: {shipSize} - {pos}");
			position.X = (float)pos.X;
			position.Y = (float)pos.Y;
			direction = GameManager.PlayerShip.Direction;
			QueueRedraw();
		}
	}

	private double direction = 0;
	private double shipSize = 0;
	private Vector2 position = Vector2.Zero;
	public double zoom = 1;
	
	public override void _Draw()
	{
		base._Draw();
		DrawCircle(_ScreenCenter, (float)(shipSize * zoom),Colors.Green);

	}
}

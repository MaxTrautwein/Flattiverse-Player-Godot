using Godot;
using System;
using Flattiverse.Connector;
using Flattiverse.Utils;

public partial class PlayerShip : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Root.Connect("size_changed", Callable.From(() => updateScreenSize()));
		updateScreenSize();
	}
	
	public void updateScreenSize()
	{
		DisplayHelper.Screensize = GetViewportRect().Size;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.PlayerShip != null)
		{
			shipSize = GameManager.PlayerShip.Size;

			DisplayHelper.PlayerPos = GameManager.PlayerShip.Position.toGodot();
		//	GD.Print($"{GameManager.PlayerShip.Position}");
			
			direction = GameManager.PlayerShip.Direction;
			
			QueueRedraw();
		}
	}

	private double direction = 0;
	private double shipSize = 0;

	private float PlayerDispalyRadius => (float)(shipSize * DisplayHelper.Zoom);
	public override void _Draw()
	{
		base._Draw();
		var PlayerPos = DisplayHelper.TransformToDisplay(DisplayHelper.PlayerPos);
		DrawCircle(PlayerPos, PlayerDispalyRadius,Colors.Green);
		
		DisplayHelper.DrawDirectionIndicator(this,PlayerDispalyRadius,PlayerPos,direction,Colors.Pink);
	}
}

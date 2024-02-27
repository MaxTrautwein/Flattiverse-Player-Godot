using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public partial class PlayerShip : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Root.Connect("size_changed", Callable.From(() => UpdateScreenSize()));
		UpdateScreenSize();
	}

	private void UpdateScreenSize()
	{
		DisplayHelper.Screensize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.PlayerShip != null)
		{
			_shipSize = GameManager.PlayerShip.Size;

			DisplayHelper.PlayerPos = GameManager.PlayerShip.Position.ToGodot();
			//	GD.Print($"{GameManager.PlayerShip.Position}");
			
			_direction = GameManager.PlayerShip.Direction;
			
			QueueRedraw();
		}
	}

	private double _direction = 0;
	private double _shipSize = 0;

	private float PlayerDisplayRadius => (float)(_shipSize * DisplayHelper.Zoom);
	public override void _Draw()
	{
		base._Draw();
		var playerPos = DisplayHelper.TransformToDisplay(DisplayHelper.PlayerPos);
		DrawCircle(playerPos, PlayerDisplayRadius,Colors.Green);
		
		DisplayHelper.DrawDirectionIndicator(this,PlayerDisplayRadius,playerPos,_direction,Colors.Pink);
	}
}
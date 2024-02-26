using Godot;
using System;
using System.Linq;
using Flattiverse.Connector;
using Flattiverse.Connector.Units;

public partial class GameObject : Node2D
{

	private Unit _unit = null;
	
	// Called when the node enters the scene tree for the first time.
	public GameObject(Unit unit)
	{
		_unit = unit;
	}

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		base._Draw();
		Vector2 pos = Vector2.Zero;
		pos.X = (float)_unit.Position.X;
		pos.Y = (float)_unit.Position.Y;
		DrawCircle(pos, (float)(_unit.Radius * 1),Colors.Red);

	}
}

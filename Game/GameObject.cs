using Godot;
using System;
using System.Linq;
using Flattiverse.Connector;
using Flattiverse.Connector.Units;
using Flattiverse.Utils;

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
		Vector2 pos = DisplayHelper.TransformToDisplay(_unit.Position.toGodot());
		
		DrawCircle(pos, (float)(_unit.Radius * DisplayHelper.Zoom),Colors.Red);
		DrawString(ThemeDB.FallbackFont,pos,string.Format($"{_unit.Name}"), fontSize: Mathf.CeilToInt(16 * DisplayHelper.Zoom) , modulate: Colors.White );

	}
}

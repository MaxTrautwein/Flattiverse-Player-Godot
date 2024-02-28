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

	protected virtual Color UnitColor => Colors.Red;
	protected virtual Color TextColor => Colors.White;
	protected virtual string DisplayString => string.Format($"{_unit.Name}");
	
	protected Vector2 GoDotPos => DisplayHelper.TransformToDisplay(_unit.Position.ToGodot());

	public override void _Draw()
	{
		base._Draw();
		
		DrawCircle(GoDotPos, (float)(_unit.Radius * DisplayHelper.Zoom),UnitColor);
		
		DrawMultilineString(ThemeDB.FallbackFont,GoDotPos,DisplayString, fontSize: Mathf.CeilToInt(16 * DisplayHelper.Zoom) , modulate: TextColor );
		
		
	}
}

using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using Flattiverse;
using Flattiverse.Connector.Hierarchy;
using Flattiverse.Connector.MissionSelection;
using Flattiverse.Connector.Units;
using Flattiverse.Utils;


public partial class game : Node
{
	private static game Instance = null;

	public game()
	{
		Instance = this;
	}

	public override async void _Ready()
	{
		
		
	}

	public static void RegisterUnit(Unit unit)
	{
		GD.Print($"Registerd new Unit");
		var newUnit = new GameObject(unit);
		
		displayMap.Add(unit, newUnit);
		
		Instance.CallDeferred("add_child", newUnit);
		//.AddChild(newUnit);
	}
	public static void DeRegisterUnit(Unit unit)
	{
		displayMap.Remove(unit);
	}

	private static Dictionary<Unit, GameObject> displayMap = new Dictionary<Unit, GameObject>();
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (Input.IsActionPressed("ZoomIn"))
		{
			DisplayHelper.Zoom = 1f;
		}
		if (Input.IsActionJustPressed("ZoomOut"))
		{
			DisplayHelper.Zoom *= 0.99f;
			GD.Print($"Zoom: {DisplayHelper.Zoom}");
		}
		
	}
	
	
	

	
}

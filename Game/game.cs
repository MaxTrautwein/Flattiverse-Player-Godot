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

		if (Input.IsActionJustPressed("ZoomIn"))
		{
			DisplayHelper.Zoom *= 1.1f;
		}
		if (Input.IsActionJustPressed("ZoomOut"))
		{
			DisplayHelper.Zoom *= 0.99f;
		}

		if (Input.IsActionJustPressed("ResetZoom"))
		{
			DisplayHelper.Zoom = 1f;
		}
		if (Input.IsActionPressed("MoveToPos"))
		{
			//Get the Position
			Vector2 Targetpos = GetViewport().GetMousePosition();// InputEventMouse.position;
			var angle = DisplayHelper.ScreenCenter.AngleToPoint(Targetpos) ;
			
			GD.Print($"Angle {Mathf.RadToDeg(angle)} - {GameManager.PlayerShip.Nozzle} - {GameManager.PlayerShip.NozzleMax} ");

			GameManager.PlayerShip.SetThruster(GameManager.PlayerShip.ThrusterMaxForward);
			//SetNozzel(angle);
		}
		else
		{
			GameManager.PlayerShip.SetThruster(0);	
		}
		
		
	}

	private void SetNozzel(float targetAng)
	{
		var direct = GameManager.PlayerShip.Direction;
		if (targetAng > direct)
		{
			GameManager.PlayerShip.SetNozzle(0.01f);
		}
		else
		{
			GameManager.PlayerShip.SetNozzle(-0.01f);
		}


	}
	
	

	
}

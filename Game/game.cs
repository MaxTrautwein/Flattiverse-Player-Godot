using System;
using Godot;
using System.Collections.Generic;
using Flattiverse.Connector.Units;
using Flattiverse.Game;
using Flattiverse.Utils;


public partial class game : Node
{
	private static game _instance;
	public static game GetInstance => _instance;
	
	/// <summary>
	/// Units to Display
	/// </summary>
	private static readonly Dictionary<Unit, GameObject> _displayMap = new Dictionary<Unit, GameObject>();
	/// <summary>
	/// Moments Markers
	/// </summary>
	private static List<MovementMarker> _movementMarkers = new List<MovementMarker>();
	
	/// <summary>
	/// Handles the Control of a Controllable
	/// </summary>
	public ShipControl ShipController = null;
	
	public game()
	{
		_instance = this;
	}

	public override void _Ready()
	{
		
	}

	public static void RegisterUnit(Unit unit)
	{
		GD.Print($"Registered new Unit");

		GameObject newUnit;

		switch (unit.Kind)
		{
			case UnitKind.Sun:
				newUnit = new DisplaySun(unit);
				break;
			case UnitKind.BlackHole:
				newUnit = new DisplayBlackHole(unit);
				break;
			case UnitKind.Planet:
				newUnit = new DisplayPlanet(unit);
				break;
			case UnitKind.Moon:
				newUnit = new DisplayMoon(unit);
				break;
			case UnitKind.Meteoroid:
				newUnit = new DisplayMeteoroid(unit);
				break;
			case UnitKind.Buoy:
				newUnit = new DispalyBuoy(unit);
				break;
			case UnitKind.PlayerUnit:
				newUnit = new DisplayOtherPlayer(unit);
				break;
			default:
				newUnit = new GameObject(unit);
				break;
		}
		
		
		_displayMap.Add(unit, newUnit);
		
		_instance.CallDeferred("add_child", newUnit);
	}
	public static void DeRegisterUnit(Unit unit)
	{
		//Todo need to test if this work like i think
		_instance.CallDeferred("remove_child", _displayMap[unit]);
		_displayMap.Remove(unit);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.

	private void ZoomHandler()
	{
		if (Input.IsActionJustPressed("ZoomIn"))
		{
			DisplayHelper.Zoom *= 1.1f;
		}
		if (Input.IsActionJustPressed("ZoomOut"))
		{
			DisplayHelper.Zoom *= 0.9f;
		}
		if (Input.IsActionJustPressed("ResetZoom"))
		{
			DisplayHelper.Zoom = 1f;
		}
	}
	
	
	public override void _Process(double delta)
	{
		if (GameManager.PlayerShip is null) return;
		if (ShipController is null) ShipController = new ShipControl(GameManager.PlayerShip);
		ZoomHandler();

		if (Input.IsActionJustPressed("MoveToPos"))
		{
			var pos = DisplayHelper.TransformToGamePos(DisplayHelper.MouseDisplayPos(this));
			MovementMarker marker = new MovementMarker(pos);
			_movementMarkers.Add(marker);
			
			GD.Print($"Set Marker @{pos}");
			_instance.CallDeferred("add_child", marker);
		}
		else if (Input.IsActionPressed("MoveInDirection"))
		{
			//Get the Position
			Vector2 targetpos = GetViewport().GetMousePosition();// InputEventMouse.position;
			var angle = DisplayHelper.ScreenCenter.AngleToPoint(targetpos) ;
			
			GameManager.PlayerShip.SetThruster(GameManager.PlayerShip.ThrusterMaxForward);
			ShipController.SetNozzle(Mathf.RadToDeg(angle),delta);
		}
		else
		{
			GameManager.PlayerShip.SetThruster(0);	
			ShipController.StabilizeTurn();
		}

		if (Input.IsActionPressed("Stabelize"))
		{
			ShipController.StabilizePosition(delta);

			foreach (var marker in _movementMarkers)
			{
				RemoveChild(marker);
			}
			_movementMarkers.RemoveAll((e) => true);
			
		}
		
		
	}



}

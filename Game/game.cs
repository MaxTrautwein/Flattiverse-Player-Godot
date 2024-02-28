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
	public game()
	{
		_instance = this;
	}

	public override void _Ready()
	{
		_nozzelControl = new PidController(0.1, 0, -0.3, 0);
	}

	public PidController NozzelControl => _nozzelControl;

	

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
		_displayMap.Remove(unit);
	}

	private static readonly Dictionary<Unit, GameObject> _displayMap = new Dictionary<Unit, GameObject>();
	private static List<MovementMarker> _movementMarkers = new List<MovementMarker>();
	
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
			SetNozzel(Mathf.RadToDeg(angle),delta);
		}
		else
		{
			GameManager.PlayerShip.SetThruster(0);	
			StabelizeTurn();
		}

		if (Input.IsActionPressed("Stabelize"))
		{
			StabelizePosition(delta);
		}
		
		
	}

	private void StabelizePositionReverse(double deltaT, float ang)
	{
		SetNozzel(ang, deltaT);

		if (CalcDiff(ang) < 10)
		{
			var speed = GameManager.PlayerShip.Movement.Length;
			var thruster = 0.02;

			thruster = Mathf.Min(thruster, speed / 5);
			//GD.Print($"{movement.toGodot()}- in {ang} --> {thruster}");
			thruster = Mathf.Min(thruster, GameManager.PlayerShip.ThrusterMaxBackward);
			
			GameManager.PlayerShip.SetThruster(-thruster);
		}
	}

	//TODO Check somthing is broken here
	private void StabelizePositionForward(double deltaT, float ang)
	{
		SetNozzel(ang + 180, deltaT);

		if (CalcDiff(ang) < 10)
		{
			var speed = GameManager.PlayerShip.Movement.Length;
			var thruster = 0.02;

			thruster = speed / 5;
			
			thruster = Mathf.Min(thruster, GameManager.PlayerShip.ThrusterMaxForward);
			
			
			GameManager.PlayerShip.SetThruster(thruster);
		}
	}
	
	private void StabelizePosition(double deltaT)
	{
		var movement = GameManager.PlayerShip.Movement;


		var ang = Mathf.RadToDeg( Vector2.Zero.AngleToPoint(movement.ToGodot()));

		if (movement.Length > 0.5)
		{
			//	StabelizePositionForward(deltaT, ang);
			StabelizePositionReverse(deltaT, ang);
		}
		else
		{
			StabelizePositionReverse(deltaT, ang);
		}
		


	}

	private void StabelizeTurn()
	{
		//Try To Stabelize the rotation
		var turnrate = GameManager.PlayerShip.Turnrate;
		var nozzelMax = GameManager.PlayerShip.NozzleMax;

		var nozzel =  Mathf.Clamp(-turnrate, -nozzelMax,nozzelMax) ;
		
		GameManager.PlayerShip.SetNozzle( nozzel);
	}

	private double CalcDiff(double target)
	{
		//Normalize Target
		var targetAng = (target + 360) % 360;
		
		return targetAng - GameManager.PlayerShip.Direction;
	}
	
	private PidController _nozzelControl;
	

	private void SetNozzel(float targetAng,double deltaT)
	{
		targetAng = (targetAng + 360) % 360;
		
		var outval = _nozzelControl.Control(deltaT, targetAng, GameManager.PlayerShip.Direction, GameManager.PlayerShip.Turnrate);
		
		//GD.Print($"PID: target:{targetAng} - actual:{GameManager.PlayerShip.Direction} --> {outval}");
		outval = Mathf.Clamp(outval, -GameManager.PlayerShip.NozzleMax, GameManager.PlayerShip.NozzleMax);
		GameManager.PlayerShip.SetNozzle(outval);
	}
	
}

using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using Flattiverse;
using Flattiverse.Connector.Hierarchy;
using Flattiverse.Connector.MissionSelection;
using Flattiverse.Connector.Units;
using Flattiverse.Game;
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

		GameObject newUnit = null;

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
		if (GameManager.PlayerShip is null) return;
		
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
		if (Input.IsActionPressed("MoveToPos"))
		{
			//Get the Position
			Vector2 Targetpos = GetViewport().GetMousePosition();// InputEventMouse.position;
			var angle = DisplayHelper.ScreenCenter.AngleToPoint(Targetpos) ;
			
			//GD.Print($"Angle {Mathf.RadToDeg(angle)} - {GameManager.PlayerShip.Nozzle} - {GameManager.PlayerShip.NozzleMax} ");

			GameManager.PlayerShip.SetThruster(GameManager.PlayerShip.ThrusterMaxForward);
			SetNozzel(Mathf.RadToDeg(angle));
		}
		else
		{
			GameManager.PlayerShip.SetThruster(0);	
			StabelizeTurn();
		}

		if (Input.IsActionPressed("Stabelize"))
		{
			StabelizePosition();

		}
		
		
	}

	private void StabelizePosition()
	{
		var movement = GameManager.PlayerShip.Movement;


		var ang = Mathf.RadToDeg( Vector2.Zero.AngleToPoint(movement.toGodot()));
			
		
		SetNozzel(ang);

		if (CalcDiff((double)ang) < 10)
		{
			var speed = GameManager.PlayerShip.Movement.Length;
			var thruster = 0.02;

			thruster = Mathf.Min(thruster, speed / 5);
			//GD.Print($"{movement.toGodot()}- in {ang} --> {thruster}");
			thruster = Mathf.Min(thruster, GameManager.PlayerShip.ThrusterMaxBackward);
			
			GameManager.PlayerShip.SetThruster(-thruster);
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
	
	private void SetNozzel(float targetAng)
	{
		var diff = CalcDiff(targetAng);
		var absDiff = Mathf.Abs(diff);
		if (absDiff >= 180) diff *= -1;

		var NozzelRate = 0.01d;
		
		var MaxNozzleRate = GameManager.PlayerShip.NozzleMax * 0.75;

		NozzelRate = MaxNozzleRate / 180 * absDiff;
		
		NozzelRate = Mathf.Min(NozzelRate, MaxNozzleRate);

		var turnrate = GameManager.PlayerShip.Turnrate;
		double turnRateLimit = 10d;

		if (absDiff < 90) turnRateLimit = 8;
		if (absDiff < 60) turnRateLimit = 5;
		if (absDiff < 20) turnRateLimit = 5;
		if (absDiff < 10) turnRateLimit = 2;
		if (absDiff < 1) turnRateLimit = 1;
		/*
		if (absDiff < 1)
		{
			StabelizeTurn();
			GD.Print("Stabelize");
			return;
		}*/
		
		
		if (diff > 0)
		{
			if (turnrate > turnRateLimit)
			{
				var offset = turnRateLimit - turnrate;
				GameManager.PlayerShip.SetNozzle(offset);
				return;
			}
			GameManager.PlayerShip.SetNozzle(NozzelRate);
		}
		else
		{
			if (turnrate < -turnRateLimit)
			{
				var offset = -turnRateLimit - turnrate;
				GameManager.PlayerShip.SetNozzle(offset);
				return;
			}
			GameManager.PlayerShip.SetNozzle(-NozzelRate);
		}


	}
	
	

	
}

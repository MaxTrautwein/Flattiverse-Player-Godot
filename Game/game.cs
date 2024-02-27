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

			StabelizeTurn();
		}
		
		
	}

	private void StabelizeTurn()
	{
		//Try To Stabelize the rotation
		var turnrate = GameManager.PlayerShip.Turnrate;
		var nozzelMax = GameManager.PlayerShip.NozzleMax;
		var stanelizeSpeed = 0;//GameManager.PlayerShip.ThrusterMaxForward / 4;

		var nozzel =  Mathf.Clamp(-turnrate, -nozzelMax,nozzelMax) ;

		GameManager.PlayerShip.SetThrusterNozzle(stanelizeSpeed, nozzel);
	}

	private void SetNozzel(float targetAng)
	{
		var direct = GameManager.PlayerShip.Direction;
		
		//Normalize Target
		targetAng = (targetAng + 360) % 360;
		
		var diff = targetAng - direct;
		var absDiff = Mathf.Abs(diff);
		if (absDiff >= 180) diff *= -1;

		var NozzelRate = 0.01d;
		
		//if ( absDiff  > 20) NozzelRate *= 10f;

		var MaxNozzleRate = GameManager.PlayerShip.NozzleMax * 0.75;

		NozzelRate = MaxNozzleRate / 180 * absDiff;
		
		NozzelRate = Mathf.Min(NozzelRate, MaxNozzleRate);
		if (diff > 0)
		{
			GameManager.PlayerShip.SetNozzle(NozzelRate);
		}
		else
		{
			GameManager.PlayerShip.SetNozzle(-NozzelRate);
		}


	}
	
	

	
}

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
		_nozzelControl = new Pid_Controller(0.1, 0, -0.3, 0);
		

	}

	public Pid_Controller NozzelControl => _nozzelControl;

	

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
		//.AddChild(newUnit);
	}
	public static void DeRegisterUnit(Unit unit)
	{
		_displayMap.Remove(unit);
	}

	private static readonly Dictionary<Unit, GameObject> _displayMap = new Dictionary<Unit, GameObject>();
	
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
			Vector2 targetpos = GetViewport().GetMousePosition();// InputEventMouse.position;
			var angle = DisplayHelper.ScreenCenter.AngleToPoint(targetpos) ;
			
			//GD.Print($"Angle {Mathf.RadToDeg(angle)} - {GameManager.PlayerShip.Nozzle} - {GameManager.PlayerShip.NozzleMax} ");

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

	private void StabelizePosition(double deltaT)
	{
		var movement = GameManager.PlayerShip.Movement;


		var ang = Mathf.RadToDeg( Vector2.Zero.AngleToPoint(movement.ToGodot()));
			
		
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

	public class Pid_Controller
	{
		private double _errorPrior;
		private double _integralPrior;
		private double _kp;
		public double Kp
		{
			get => _kp;
			set => _kp = value;
		}
		
		private double _ki;
		public double Ki
		{
			get => _ki;
			set => _ki = value;
		}
		
		private double _kd;
		public double Kd
		{
			get => _kd;
			set => _kd = value;
		}
		
		private double _bias;
		public double Bias
		{
			get => _bias;
			set => _bias = value;
		}
		
		
		public Pid_Controller(double kp,double ki,double kd, double bias)
		{
			_errorPrior = 0;
			_integralPrior = 0;
			_kp =  kp; //Some value you need to come up (see tuning section below)
			_ki =  ki; //Some value you need to come up (see tuning section below)
			_kd =  kd; //Some value you need to come up (see tuning section below)
			_bias = bias; // (see below)
		}

		// This function normalizes the angle so it returns a value between -180° and 180° instead of 0° to 360°.
		public double angleWrap(double radians) {
			while (radians > Mathf.Pi) {
				radians -= 2 * Mathf.Pi;
			}
			while (radians < -Mathf.Pi) {
				radians += 2 * Mathf.Pi;
			}

			// keep in mind that the result is in radians
			return radians;
		}

		
		public double Control(double timeDelta,double desired_value, double actual_value, double turnrate)
		{

			var error = Mathf.RadToDeg(angleWrap(Mathf.DegToRad(desired_value - actual_value)));//desired_value - actual_value;
			var integral = _integralPrior + error * timeDelta;
			var derivative = turnrate; //(error - _errorPrior) / timeDelta;
			_errorPrior = error;
			_integralPrior = integral;
			return _kp * error + _ki * integral + _kd * derivative + _bias;
		}

	}


	private Pid_Controller _nozzelControl;
	

	private void SetNozzel(float targetAng,double deltaT)
	{
		targetAng = (targetAng + 360) % 360;
		
		var outval = _nozzelControl.Control(deltaT, targetAng, GameManager.PlayerShip.Direction, GameManager.PlayerShip.Turnrate);
		
		//GD.Print($"PID: target:{targetAng} - actual:{GameManager.PlayerShip.Direction} --> {outval}");
		outval = Mathf.Clamp(outval, -GameManager.PlayerShip.NozzleMax, GameManager.PlayerShip.NozzleMax);
		GameManager.PlayerShip.SetNozzle(outval);
	}
	
	

	
}

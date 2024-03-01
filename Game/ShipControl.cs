using Flattiverse.Connector;
using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public class ShipControl
{
    private readonly Controllable _ship;
    public Controllable Ship => _ship;

    /// <summary>
    /// Movement Vector of the Ship as a Vector2
    /// </summary>
    public Vector2 Movement => _ship.Movement.ToGodot();

    /// <summary>
    /// Game Position as a Vector2
    /// </summary>
    public Vector2 Position => _ship.Position.ToGodot();
    public double ThrusterForwardMax => _ship.ThrusterForwardMax;
    public double ThrusterBackwardMax => _ship.ThrusterBackwardMax;
    

    /// <summary>
    /// PID Controll for the Nozzle Angle
    /// </summary>
    public PidController NozzleControl { get; }
    
    public Vector2 GravityVector = Vector2.Zero;

    public double MovementDirectionDeg => Mathf.RadToDeg(Vector2.Zero.AngleToPoint(Movement) );
    
    private double _desierdThrustForward;
    public double DesierdThrustForward
    {
        get
        {
            if (Input.IsActionPressed("FullSpeedOverride")) return ThrusterForwardMax;
            return _desierdThrustForward;
        }
        set
        {
            _desierdThrustForward = value;
            _desierdThrustForward = Mathf.Clamp(_desierdThrustForward, 0, ThrusterForwardMax);
        }
    }

    public ShipControl(Controllable ship)
    {
        _ship = ship;
        NozzleControl = new PidController(0.1, 0, -0.3, 0);
    }
    
    private void StabilizePositionReverse(double deltaT, float ang)
    {
        SetNozzle(ang, deltaT);

        if (DisplayHelper.CalcDiff(ang) < 10)
        {
            var speed = _ship.Movement.Length;
            var thruster = 0.02;

            thruster = Mathf.Min(thruster, speed / 5);
            //GD.Print($"{movement.toGodot()}- in {ang} --> {thruster}");
            thruster = Mathf.Min(thruster, _ship.ThrusterBackwardMax);
			
            _ship.SetThruster(-thruster);
        }
    }

    //TODO Check something is broken here
    private void StabilizePositionForward(double deltaT, float ang)
    {
        SetNozzle(ang + 180, deltaT);

        if (DisplayHelper.CalcDiff(ang) < 10)
        {
            var speed = _ship.Movement.Length;
            var thruster = 0.02;

            thruster = speed / 5;
			
            thruster = Mathf.Min(thruster, _ship.ThrusterForwardMax);
			
			
            _ship.SetThruster(thruster);
        }
    }
	
    public void StabilizePosition(double deltaT)
    {
        var movement = _ship.Movement;


        var ang = Mathf.RadToDeg( Vector2.Zero.AngleToPoint(movement.ToGodot()));

        if (movement.Length > 0.5)
        {
            //	StabilizePositionForward(deltaT, ang);
            StabilizePositionReverse(deltaT, ang);
        }
        else
        {
            StabilizePositionReverse(deltaT, ang);
        }
    }
    
    public void StabilizeTurn()
    {
        //Try To Stabelize the rotation
        var turnrate = _ship.Turnrate;
        var nozzelMax = _ship.NozzleMax;

        var nozzel =  Mathf.Clamp(-turnrate, -nozzelMax,nozzelMax) ;
		
        _ship.SetNozzle( nozzel);
    }

    /// <summary>
    /// TODO Do i Really need this to be public
    /// </summary>
    /// <param name="targetAng"></param>
    /// <param name="deltaT"></param>
    public void SetNozzle(float targetAng,double deltaT)
    {
        targetAng = (targetAng + 360) % 360;
		
        var outval = NozzleControl.Control(deltaT, targetAng, _ship.Direction, _ship.Turnrate);
		
        //GD.Print($"PID: target:{targetAng} - actual:{_ship.Direction} --> {outval}");
        outval = Mathf.Clamp(outval, -_ship.NozzleMax, _ship.NozzleMax);
        _ship.SetNozzle(outval);
    }

    /// <summary>
    /// TODO Add Option for Intertia correction
    /// </summary>
    /// <param name="targetpos">In the Display System</param>
    /// <param name="deltaT"></param>
    public void MoveTowards(Vector2 targetpos,double deltaT)
    {
        MoveTowards(targetpos, deltaT, DesierdThrustForward);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetpos">In the Display System</param>
    /// <param name="deltaT"></param>
    /// <param name="Thrust"></param>
    public void MoveTowards(Vector2 targetpos,double deltaT, double Thrust)
    {
        var Targetangle = Mathf.RadToDeg(DisplayHelper.ScreenCenter.AngleToPoint(targetpos)) ;
        
        SetNozzle(Targetangle,deltaT);
        
        GameManager.PlayerShip.SetThruster(Thrust);
    }
    
}
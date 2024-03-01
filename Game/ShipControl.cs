using Flattiverse.Connector;
using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public class ShipControl
{
    private readonly Controllable _ship;
    
    /// <summary>
    /// Movement Vector of the Ship as a Vector2
    /// </summary>
    public Vector2 Movement => _ship.Movement.ToGodot();

    public double MovementSpeed => _ship.Movement.Length;

    /// <summary>
    /// Game Position as a Vector2
    /// </summary>
    public Vector2 Position => _ship.Position.ToGodot();
    public double ThrusterForwardMax => _ship.ThrusterForwardMax;
    public double ThrusterBackwardMax => _ship.ThrusterBackwardMax;
    

    /// <summary>
    /// PID Controll for the Nozzle Angle
    /// </summary>
    public PDController NozzleControl { get; }
    
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
        NozzleControl = new PDController(0.1, -0.3);
    }
    
    private void StabilizePositionReverse(double deltaT, float ang)
    {
        SetNozzle(ang);

        if (DisplayHelper.CalcDiff(ang) < 10)
        {
            var thruster = 0.02;

            thruster = Mathf.Min(thruster, MovementSpeed / 5);
            thruster = Mathf.Min(thruster, ThrusterBackwardMax);
			
            _ship.SetThruster(-thruster);
        }
    }

    //TODO Check something is broken here
    private void StabilizePositionForward(double deltaT, float ang)
    {
        SetNozzle(ang + 180);

        if (DisplayHelper.CalcDiff(ang) < 10)
        {
            var thruster = 0.02;

            thruster = MovementSpeed / 5;
			
            thruster = Mathf.Min(thruster, ThrusterForwardMax);
			
			
            _ship.SetThruster(thruster);
        }
    }
	
    public void StabilizePosition(double deltaT)
    {
        var ang = Mathf.RadToDeg( Vector2.Zero.AngleToPoint(Movement));

        if (MovementSpeed > 0.5)
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
    public void SetNozzle(float targetAng)
    {
        targetAng = (targetAng + 360) % 360;
		
        var outval = NozzleControl.Control(targetAng, _ship.Direction, _ship.Turnrate);
		
        outval = Mathf.Clamp(outval, -_ship.NozzleMax, _ship.NozzleMax);
        _ship.SetNozzle(outval);
    }

    /// <summary>
    /// TODO Add Option for Intertia correction
    /// </summary>
    /// <param name="targetpos">In the Display System</param>
    /// <param name="deltaT"></param>
    public void MoveTowards(Vector2 targetpos)
    {
        MoveTowards(targetpos, DesierdThrustForward);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetpos">In the Display System</param>
    /// <param name="deltaT"></param>
    /// <param name="Thrust"></param>
    public void MoveTowards(Vector2 targetpos, double Thrust)
    {
        var Targetangle = Mathf.RadToDeg(DisplayHelper.ScreenCenter.AngleToPoint(targetpos)) ;
        
        SetNozzle(Targetangle);
        
        GameManager.PlayerShip.SetThruster(Thrust);
    }
    
}
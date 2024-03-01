using Godot;

namespace Flattiverse.Utils;

public class PDController
{
    		private double _kp;
    		public double Kp
    		{
    			get => _kp;
    			set => _kp = value;
    		}
    		
    		private double _kd;
    		public double Kd
    		{
    			get => _kd;
    			set => _kd = value;
    		}
    		
    		public PDController(double kp,double kd)
    		{
    			_kp =  kp;
    			_kd =  kd;
    		}
		    
		   
    
    		
    		public double Control(double desired_value, double actual_value, double turnrate)
    		{
    			var error = AngleCalc.AngleWrapDeg(desired_value - actual_value);
    			var derivative = turnrate;
    			return _kp * error +  _kd * derivative;
    		}
}
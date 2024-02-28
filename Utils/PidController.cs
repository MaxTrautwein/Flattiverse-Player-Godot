using Godot;

namespace Flattiverse.Utils;

public class PidController
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
    		
    		
    		public PidController(double kp,double ki,double kd, double bias)
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
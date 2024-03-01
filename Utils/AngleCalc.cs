namespace Flattiverse.Utils;

public static class AngleCalc
{
    /// <summary>
    /// Keep Angle between 180° & -180°
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static double AngleWrapDeg(double degree) {
        while (degree > 180) {
            degree -= 360;
        }
        while (degree < -180) {
            degree += 360;
        }
        return degree;
    }
}
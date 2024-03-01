using System.Collections.Generic;
using Flattiverse.Connector.Units;
using Godot;

namespace Flattiverse.Utils;

public static class VectorCalc
{
    /// <summary>
    /// Calculate the total Known Gravity Vector for a given Position
    ///
    /// Game Vector Space
    /// </summary>
    /// <param name="gravitors">List of known things with an affect on gravity (Position & Gravity-Strength)</param>
    /// <param name="position">Position to run the Calculation for</param>
    /// <returns></returns>
    public static Vector2 CalcGravity(List<Unit> gravitors,Vector2 position)
    {
        Vector2 knownGravity = Vector2.Zero;
		
        foreach (Unit gravitor in gravitors)
        {
            Vector2 diff = gravitor.Position.ToGodot() - position;
			
            var factor = gravitor.Gravity * 60.0 / (diff.LengthSquared() > 3600.0f ? diff.Length() : 60.0);
            diff = diff.Normalized() * (float)factor;
			
            knownGravity += diff;
        }

        return knownGravity;
    }
}
using Flattiverse.Connector;
using Godot;

namespace Flattiverse.Utils;

public static class VectorExtension
{
    public static Vector2 toGodot(this Vector vector)
    {
        Vector2 vect = Vector2.Zero;
        vect.X = (float)vector.X;
        vect.Y = (float)vector.Y;
        return vect;
    }
    
}
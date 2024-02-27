using Flattiverse.Connector;
using Godot;

namespace Flattiverse.Utils;

public static class VectorExtension
{
    public static Vector2 ToGodot(this Vector vector)
    {
        Vector2 vector2 = Vector2.Zero;
        vector2.X = (float)vector.X;
        vector2.Y = (float)vector.Y;
        return vector2;
    }
    
}
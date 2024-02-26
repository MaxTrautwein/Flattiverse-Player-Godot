using Godot;

namespace Flattiverse.Utils;

public static class DisplayHelper
{
    private static Vector2 screensize;

    public static Vector2 Screensize
    {
        get => screensize;
        set => screensize = value;
    }

    public static Vector2 ScreenCenter => screensize / 2;

    private static Vector2 playerPos;

    public static Vector2 PlayerPos
    {
        get => playerPos;
        set => playerPos = value;
    }


    private static float zoom = 1;

    public static float Zoom
    {
        get => zoom;
        set
        {
            zoom = value;
            if (zoom <= 0f) zoom = 0.00001f;
        }
    }


    

    public static Vector2 TransformToDisplay(Vector2 GamePos)
    {
        var relativePos = GamePos - PlayerPos + ScreenCenter;

        relativePos *= Zoom;

        return relativePos;
    }
}
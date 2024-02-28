using Godot;

namespace Flattiverse.Utils;

public static class DisplayHelper
{
    private static Vector2 _screensize;

    public static Vector2 Screensize
    {
        get => _screensize;
        set => _screensize = value;
    }

    public static Vector2 ScreenCenter => _screensize / 2;

    private static Vector2 _playerPos;

    public static Vector2 PlayerPos
    {
        get => _playerPos;
        set => _playerPos = value;
    }


    private static float _zoom = 1;

    public static float Zoom
    {
        get => _zoom;
        set
        {
            _zoom = value;
            if (_zoom <= 0f) _zoom = 0.00001f;
        }
    }


    
    /// <summary>
    /// Transforms a Game Position to one on the Screen
    /// </summary>
    /// <param name="gamePos"></param>
    /// <returns></returns>
    public static Vector2 TransformToDisplay(Vector2 gamePos)
    {
        var relativePos = (gamePos - PlayerPos) * Zoom + ScreenCenter;
        
        return relativePos;
    }
    /// <summary>
    /// Gets the Displayed Mouse Position
    /// </summary>
    /// <param name="viewRef"></param>
    /// <returns></returns>
    public static Vector2 MouseDisplayPos(Node viewRef)
    {
        return viewRef.GetViewport().GetMousePosition();
    }
    
    /// <summary>
    /// Transforms a display Position to the Actual Position ingame
    /// </summary>
    /// <param name="displayPos"></param>
    /// <returns></returns>
    public static Vector2 TransformToGamePos(Vector2 displayPos)
    {
        Vector2 target = ((displayPos - ScreenCenter ) / _zoom) + _playerPos;

        return target;
    }

    public static void DrawDirectionIndicator(Node2D drawRef, float unitRadius, Vector2 unitPos, double direction,Color drawColor, float baseWidth = 4)
    {
        Vector2 dirvect = Vector2.Right * unitRadius;
        dirvect = dirvect.Rotated((float)Mathf.DegToRad(direction));
        dirvect += unitPos;

        //Inducate Direction with a Line into the Relevant Direction
        // Right is 0°
        drawRef.DrawLine(unitPos,dirvect,drawColor,width: baseWidth * DisplayHelper.Zoom);
    }
    
    public static void DrawSection(Node2D drawRef ,Vector2 pos ,double innerRadius, double outerRadius,double angleFrom, double angleTo, Color color)
    {
        var innerRad = (float)innerRadius * DisplayHelper.Zoom;
        var outerRad =    (float)outerRadius * DisplayHelper.Zoom; 
        var startAng = (float)Mathf.DegToRad(angleFrom);
        var stopAng = (float)Mathf.DegToRad(angleTo);
        var width = 5 * DisplayHelper.Zoom;
        drawRef.DrawArc(pos,innerRad, startAng,stopAng,100,color,width);
        drawRef.DrawArc(pos,outerRad, startAng,stopAng,100,color,width);
    }
    
}
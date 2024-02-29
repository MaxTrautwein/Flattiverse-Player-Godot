using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public partial class MovementMarker : Node2D
{
    public MovementMarker(Vector2 target)
    {
        drawPos = target;
    }
    private Vector2 drawPos;
    public Vector2 targetPos => drawPos;
    public override void _Process(double delta)
    {
        QueueRedraw();
    }
    public override void _Draw()
    {
        base._Draw();
        DrawCircle(DisplayHelper.TransformToDisplay(drawPos) , (8 * DisplayHelper.Zoom),Colors.DarkRed);
    }
}
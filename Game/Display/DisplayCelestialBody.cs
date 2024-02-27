using Flattiverse.Connector.Units;
using Godot;

namespace Flattiverse.Game;

public partial class DisplayCelestialBody : GameObject
{
    public DisplayCelestialBody(Unit unit) : base(unit)
    {
        
    }

    protected override Color UnitColor { get => Colors.DeepPink; }
}
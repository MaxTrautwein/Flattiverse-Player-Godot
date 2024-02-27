using Flattiverse.Connector.Units;
using Godot;

namespace Flattiverse.Game;

public partial class DisplayPlanet: DisplayHarvestable
{
    public DisplayPlanet(Unit unit) : base(unit)
    {
    }
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Plant)");

    protected override Color UnitColor { get => Colors.Burlywood; }
}
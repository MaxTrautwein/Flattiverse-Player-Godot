using Flattiverse.Connector.Units;

namespace Flattiverse.Game;

public partial class DisplayHarvestable : DisplayCelestialBody
{
    public DisplayHarvestable(Unit unit) : base(unit)
    {
    }
    
    protected override string DisplayString => string.Format($"{base.DisplayString}\nHarvestable");
}
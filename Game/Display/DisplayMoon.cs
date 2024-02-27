using Flattiverse.Connector.Units;

namespace Flattiverse.Game;

public partial class DisplayMoon : DisplayHarvestable
{
    public DisplayMoon(Unit unit) : base(unit)
    {
    }
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Moon)");

}
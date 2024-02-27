using Flattiverse.Connector.Units;

namespace Flattiverse.Game;

public partial class DisplayMeteoroid : DisplayHarvestable
{
    public DisplayMeteoroid(Unit unit) : base(unit)
    {
    }
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Meteoroid)");

}
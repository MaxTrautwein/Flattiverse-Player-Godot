using Flattiverse.Connector.Units;

namespace Flattiverse.Game;

public partial class DispalyBuoy : DisplayCelestialBody
{
    public DispalyBuoy(Unit unit) : base(unit)
    {
    }
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Buoy)");

}
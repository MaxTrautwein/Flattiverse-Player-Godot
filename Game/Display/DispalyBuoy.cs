using Flattiverse.Connector.Units;

namespace Flattiverse.Game;

public partial class DispalyBuoy : DisplayCelestialBody
{
    private Buoy _buoy;
    public DispalyBuoy(Unit unit) : base(unit)
    {
        _buoy = unit as Buoy;
    }
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Buoy)\n{_buoy.Message}");

}
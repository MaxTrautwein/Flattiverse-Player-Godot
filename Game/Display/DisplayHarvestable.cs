using Flattiverse.Connector.Units;
using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public partial class DisplayHarvestable : DisplayCelestialBody
{
    private Harvestable _harvestable;
    public DisplayHarvestable(Unit unit) : base(unit)
    {
        _harvestable = unit as Harvestable;
    }
    
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Harvestable)");

    public override void _Draw()
    {
        base._Draw();
        foreach (var section in _harvestable.Sections)
        {
            DisplayHelper.DrawSection(this,GoDotPos,section.InnerRadius, section.OuterRadius, section.AngleFrom, section.AngleTo, Colors.Silver);

        }
        
    }
}
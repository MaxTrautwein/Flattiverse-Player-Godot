using Flattiverse.Connector.Units;
using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public partial class DisplayBlackHole : DisplayCelestialBody
{
    private BlackHole _blackHole;
    public DisplayBlackHole(Unit unit) : base(unit)
    {
        _blackHole = unit as BlackHole;
    }
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(BlackHole)");

    protected override Color UnitColor { get => Colors.Black; }
    
    public override void _Draw()
    {
        base._Draw();
        
        if (_blackHole.Sections is null) return;
        foreach (var section in _blackHole.Sections)
        {
            DisplayHelper.DrawSection(this,GoDotPos,section.InnerRadius, section.OuterRadius, section.AngleFrom, section.AngleTo, Colors.Red);

        }
    }
}
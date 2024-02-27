using Flattiverse.Connector.Units;
using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public partial class DisplaySun : DisplayCelestialBody
{

    private Sun _sun;
    public DisplaySun(Unit unit) : base(unit)
    {
        _sun = unit as Sun;
    }

    protected override Color TextColor { get => Colors.Black; }

    protected override Color UnitColor { get => Colors.Yellow; }
    
    protected override string DisplayString => string.Format($"{base.DisplayString}\n(Sun)");

    
    //TODO Improve Draw Sections
    public override void _Draw()
    {
        base._Draw();

        var sunSections = _sun.Sections;
        foreach (var section in sunSections)
        {
            //Draw a Section
            DisplayHelper.DrawSection(this,GoDotPos,section.InnerRadius, section.OuterRadius, section.AngleFrom, section.AngleTo, Colors.Orange);
        }

    }

}
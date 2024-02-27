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
    
    //TODO Draw Sections
    public override void _Draw()
    {
        base._Draw();

        var sunSections = _sun.Sections;
        foreach (var section in sunSections)
        {
            //Draw a Section
            var innerRad = (float)section.InnerRadius * DisplayHelper.Zoom;
            var outerRad =    (float)section.OuterRadius * DisplayHelper.Zoom; 
            var StartAng = (float)Mathf.DegToRad(section.AngleFrom);
            var StopAng = (float)Mathf.DegToRad(section.AngleTo);
            var width = (float)(section.OuterRadius - section.InnerRadius) * DisplayHelper.Zoom;
            DrawArc(GoDotPos,innerRad, StartAng,StopAng,100,Colors.Orange,10);
            DrawArc(GoDotPos,outerRad, StartAng,StopAng,100,Colors.Orange,10);
            
            
            
        }

    }

}